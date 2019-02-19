using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// プレイヤーからのコントローラーによる操作を管理するクラス
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class CharacterMoveOrder : MonoBehaviour {

    [SerializeField]
    private Transform _RightHandAnchor;
    [SerializeField]
    private Transform _LeftHandAnchor;
    [SerializeField]
    private Transform _CenterEyeAnchor;

    //コントローラーの取得
    private Transform Pointer
    {
        get
        {
            var controller = OVRInput.GetActiveController();
            if (controller == OVRInput.Controller.RTrackedRemote)
            {
                return _RightHandAnchor;
            }
            else if (controller == OVRInput.Controller.LTrackedRemote)
            {
                return _LeftHandAnchor;
            }
            // どちらも取れなければ
            return _CenterEyeAnchor;
        }
    }

    //レイ用
    float maxRayDistance = 50.0f;

    float hitdistance;
    public GameObject hitObj { get; private set; }
    public Vector3 hitpoint { get; private set; }
    Vector3 hitNormal;

    GameObject sObj;                //スキルオブジェクト
    GameObject grabskill =  null;   //掴んでいるオブジェクト
    GameObject grabs = null;
    float grabDis;                  //掴んだ距離
    public GameObject sChara { get; private set; }              //スキルを持つキャラ
    float sClamp;                   //スキルの射程制限
    public Vector3 setSkillPos { get; private set; }      //スキルをセットした位置

    public bool grabMode;          //掴んでいる状態かどうか

    //マーカー（インスペクターから接続）
    [SerializeField] GameObject orderMarker;
    [SerializeField] GameObject moveOrderSphere;
    [SerializeField] GameObject selectedMarker;
    [SerializeField] GameObject charaSelectRing;
    [SerializeField] GameObject enemySelectRing;
    [SerializeField] GameObject skillSelectHandP;
    [SerializeField] GameObject skillSelectHandG;
    [SerializeField] GameObject handMarkerText;

    RectTransform orderMarkerTrans;
    Vector3 currentMarkerScale;

    float markerHeight = 2.0f;     //マーカーを表示する高さ

    //GameObject[] characters;
    public List<GameObject> charactersListP { get; private set; }          //キャラのリスト
    [SerializeField] GameObject[] charaStartPoint = new GameObject[4];     //キャラのスタート位置
    public bool charaSetReady { get; private set; }

    GameObject selectedCharacter;                       //現在選択しているキャラクター
    NavMeshAgent characterNav;                          //そのナビメッシュ
    float movespeed = 0.1f;            //移動速度

    public bool order;//{ get; private set; }              //命令中か
    public bool pubOrder; /*{ get; private set; }*/        //全体命令ちゅうか

    LineRenderer lineRenderer;           //コントローラーからでるレーザー

    [SerializeField] GameObject battleController;
    [SerializeField] GameObject destroyParticle;


    void Start () {
        charaSetReady = false;

        lineRenderer = GetComponent<LineRenderer>();

        //各ポインタの初期状態
        orderMarker.SetActive(false);
        moveOrderSphere.SetActive(false);
        charaSelectRing.SetActive(false);
        selectedMarker.SetActive(true);
        skillSelectHandG.SetActive(false);
        skillSelectHandP.SetActive(false);


        order = false;
        grabMode = false;

        orderMarkerTrans = orderMarker.GetComponent<RectTransform>();
        currentMarkerScale = orderMarkerTrans.localScale;

        //キャラリストを取得
        var charaManager = GameObject.Find("GameCharactorManager");
        List<GameObject> _charactersList = charaManager.GetComponent<BattleCharactersManager>().battleCharactersList;
        charactersListP = new List<GameObject>();
        //キャラを生成
        for(int i =0; i < _charactersList.Count; i++)
        {
            var _chara = Instantiate(_charactersList[i], charaStartPoint[i].transform.position, charaStartPoint[i].transform.rotation);
            charactersListP.Add(_chara);
        }
        charaSetReady = true;
        
        /*       
        //キャラクターを取得しリストに入れる
        var characters = GameObject.FindGameObjectsWithTag("OrderCharactor");
        charactersListP = new List<GameObject>();
        charactersListP.AddRange(characters);
        */

        //初期選択キャラに戦闘のキャラを
        selectedCharacter = charactersListP[0];
        characterNav = selectedCharacter.GetComponent<NavMeshAgent>();
        //その位置にマーカーを移動
        FollowMarker();
        
    }
        
    void Update()
    {
        var pointer = Pointer;
        if (pointer == null)
        {
            return;
        }
        
        FollowMarker();

        if(!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || !Input.GetMouseButton(1))
        {
            order = false;
        }

        //パッドを押すと味方全体に指示を出せる(publicOrder)
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) || Input.GetKeyDown(KeyCode.Space))
        {
            PublicOrderOnOff();
        }

        // コントローラー位置からRayを飛ばす
        Ray pointerRay = new Ray(pointer.position, pointer.forward);        

        //可視化
        Debug.DrawRay(pointerRay.origin, pointerRay.direction * maxRayDistance, Color.green);
        int layerMask = (1 << 23 | 1 << 24 | 1 << 25 | 1 << 19 | 1 << 17 | 1 << 16);
        RaycastHit hit;

        //レイが当たった時、ヒットしたものによっての処理
        if (Physics.Raycast(pointerRay, out hit, maxRayDistance, layerMask))
        {
            hitdistance = hit.distance;
            hitObj = hit.collider.gameObject;
            hitpoint = hit.point;
            hitNormal = hit.normal;

            orderMarker.SetActive(true);
            orderMarker.transform.position = hit.point;
            orderMarker.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hitNormal);

            OrderMarkerScalseChange();

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, pointerRay.origin);
            lineRenderer.SetPosition(1, hitpoint);
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.01f;

            

            //移動可能なオブジェクト(床など)にヒットしたとき
            if (hitObj.layer == 23 && !grabMode)
            {
                charaSelectRing.SetActive(false);
                enemySelectRing.SetActive(false);
                skillSelectHandP.SetActive(false);
                skillSelectHandG.SetActive(false);

                if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButton(1))
                {
                    order = true;
                    //トリガーを押している間Rayの当たったところにmoveポインタを出す                    
                    moveOrderSphere.SetActive(true);
                    //orderMarker.SetActive(false);
                    moveOrderSphere.transform.position = hit.point;

                    if (pubOrder)
                    {
                        var _selctedC = selectedCharacter;
                        for(int i = 0; i < charactersListP.Count; i++)
                        {
                            characterNav = charactersListP[i].GetComponent<NavMeshAgent>();
                            characterNav.SetDestination(hitpoint);
                        }
                        characterNav = _selctedC.GetComponent<NavMeshAgent>();
                    }
                    else
                    {
                        characterNav.SetDestination(hitpoint);
                    }
                }
                //トリガーを離したとき
                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonUp(1))
                {
                    order = false;
                    moveOrderSphere.SetActive(false);
                    //orderMarker.SetActive(true);
                    characterNav.SetDestination(selectedCharacter.transform.position);
                }

            }
            //キャラ
            else if (hitObj.layer == 25 && !grabMode)
            {
                charaSelectRing.SetActive(true);
                enemySelectRing.SetActive(false);
                skillSelectHandP.SetActive(false);
                skillSelectHandG.SetActive(false);
                //orderMarker.SetActive(false);

                charaSelectRing.transform.position = orderMarker.transform.position;
                charaSelectRing.transform.rotation = orderMarker.transform.rotation;

                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonDown(1))
                {
                    //既に選択されているキャラをもう一度選択したとき
                    if (selectedCharacter == hitObj)
                    {
                        selectedCharacter.GetComponent<BattleCharacterController>().Wait();
                    }
                    else
                    {
                        selectedCharacter = hitObj;
                        characterNav = selectedCharacter.GetComponent<NavMeshAgent>();
                        FollowMarker();
                    }
                }
                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonUp(1))
                {
                    order = false;
                    moveOrderSphere.SetActive(false);
                    //orderMarker.SetActive(true);
                }
            }

            //敵
            else if (hitObj.layer == 19 && !grabMode)
            {
                charaSelectRing.SetActive(false);
                enemySelectRing.SetActive(true);
                skillSelectHandP.SetActive(false);
                skillSelectHandG.SetActive(false);
                //orderMarker.SetActive(false);

                enemySelectRing.transform.position = orderMarker.transform.position;
                enemySelectRing.transform.rotation = orderMarker.transform.rotation;

                if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButton(1))
                {
                    order = true;
                    //トリガーを押している間Rayの当たったところにmoveポインタを出す                    
                    moveOrderSphere.SetActive(true);
                    moveOrderSphere.transform.position = hit.point;

                    if (pubOrder)
                    {
                        var _selctedC = selectedCharacter;
                        for (int i = 0; i < charactersListP.Count; i++)
                        {
                            characterNav = charactersListP[i].GetComponent<NavMeshAgent>();
                            characterNav.SetDestination(hitpoint);
                        }
                        characterNav = _selctedC.GetComponent<NavMeshAgent>();
                    }
                    else
                    {
                        characterNav.SetDestination(hitpoint);
                    }
                }
                //トリガーを離したとき
                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonUp(1))
                {
                    order = false;
                    moveOrderSphere.SetActive(false);
                    characterNav.SetDestination(selectedCharacter.transform.position);
                }
            }

            //スキル
            else if (hitObj.layer == 17 && !grabMode)
            {
                charaSelectRing.SetActive(false);
                enemySelectRing.SetActive(false);
                skillSelectHandP.SetActive(true);
                skillSelectHandG.SetActive(false);

                skillSelectHandP.transform.position = orderMarker.transform.position;

                sObj = hitObj;
                GameObject sRange = hitObj.GetComponent<SkillSphere>()._skillRange;
                sClamp = hitObj.GetComponent<SkillSphere>()._clamp;
                string sName = hitObj.GetComponent<SkillSphere>()._skillName;

                var text = handMarkerText.GetComponent<Text>();
                text.text = string.Format("{0}", sName);                


                //下記のスキル発動時に使用
                var _sChara = hitObj.transform.parent.gameObject;
                sChara = _sChara.transform.parent.gameObject;   //親の親。Rootの下にあるキャラオブジェクト

                grabDis = hitdistance;
               
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonDown(1))
                {
                    //スキル配置用のオブジェクトを生成
                    grabskill = Instantiate(sRange, hitpoint, Quaternion.identity);
                    
                }

                if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButton(1))
                {
                    skillSelectHandP.SetActive(false);
                    skillSelectHandG.SetActive(true);
                    skillSelectHandG.transform.position = orderMarker.transform.position;

                    grabMode = true;
                   
                }
                else
                {
                    skillSelectHandP.SetActive(true);
                    skillSelectHandG.SetActive(false);
                    skillSelectHandP.transform.position = orderMarker.transform.position;
                    
                }
            }
            //アイテム(レイヤーは16:Menu)
            else if (hitObj.layer == 16 && hitObj.tag == "MenuItem" && !grabMode) 
            {
                skillSelectHandP.SetActive(true);
                skillSelectHandG.SetActive(false);
                skillSelectHandP.transform.position = orderMarker.transform.position;

                //アイテムの各データを取得
                var itemName = hitObj.GetComponent<Item>().thisItemData.itemName;
                var text = handMarkerText.GetComponent<Text>();
                text.text = string.Format("{0}", itemName);

                GameObject grabItem = hitObj.GetComponent<Item>().thisItemData.grabItem;


                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonDown(1))
                {
                    var itemPanel = hitObj;
                    grabs = Instantiate(grabItem, hitpoint, Quaternion.identity);
                    //PanelのアイテムデータをインスタンティエイトしたオブジェクトのItemコンポーネントに書き込む
                    grabs.GetComponent<Item>().thisItemData = itemPanel.GetComponent<Item>().thisItemData;
                    //アイテムを選択し掴んだ時にアイテムメニューを消す
                    var itemMenu = transform.Find("OVRCameraRig").gameObject.transform.Find("TrackingSpace").gameObject
                                  .transform.Find("CenterEyeAnchor").gameObject.transform.Find("ItemMenu").gameObject;
                    if(itemMenu.activeSelf)
                    {
                        itemMenu.SetActive(false);
                    }
                }

                if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButton(1))
                {
                    skillSelectHandP.SetActive(false);
                    skillSelectHandG.SetActive(true);
                    skillSelectHandG.transform.position = orderMarker.transform.position;

                    grabs.transform.position = hitpoint;

                    grabMode = true;

                }
                else
                {
                    skillSelectHandP.SetActive(true);
                    skillSelectHandG.SetActive(false);
                    skillSelectHandP.transform.position = orderMarker.transform.position;

                }

                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonUp(1))
                {
                    grabMode = false;
                }


            }
            else
            {
                //orderMarker.SetActive(true);
                charaSelectRing.SetActive(false);
                enemySelectRing.SetActive(false);
                skillSelectHandP.SetActive(false);
                skillSelectHandG.SetActive(false);

                
            }
            
            
            
        }
        else
        {
            charaSelectRing.SetActive(false);
            enemySelectRing.SetActive(false);
            skillSelectHandP.SetActive(false);
            skillSelectHandG.SetActive(false);

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, pointerRay.origin);
            lineRenderer.SetPosition(1, pointerRay.origin + pointerRay.direction * maxRayDistance);
            lineRenderer.startWidth = 0.02f;
            lineRenderer.endWidth = 0.02f;
        }





        ///////////////////////////////////////////////////////
        //スキルを掴んでる時の移動制限（射程）
        if(grabskill != null && grabMode)
        {        
            if (Physics.Raycast(pointerRay, out hit, maxRayDistance, layerMask))
            {
                grabskill.transform.position = hit.point;
                grabskill.transform.position = new Vector3(Mathf.Clamp(grabskill.transform.position.x, sChara.transform.position.x - sClamp, sChara.transform.position.x + sClamp),
                                                      Mathf.Clamp(grabskill.transform.position.y, sChara.transform.position.y - sClamp, sChara.transform.position.y + sClamp),
                                                      Mathf.Clamp(grabskill.transform.position.z, sChara.transform.position.z - sClamp, sChara.transform.position.z + sClamp));
                
            }
            else
            {
                var grabPos = pointer.position + pointer.forward * grabDis;
                grabskill.transform.position = grabPos;
            }

            //離したときにスキルをセットしSendMesageして破壊
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonUp(1))
            {
                var useAp = sObj.GetComponent<SkillSphere>()._useAp;
                var currentAp = battleController.GetComponent<BattleAPController>().allAp;

                if (useAp > currentAp)
                {
                    Instantiate(destroyParticle, grabskill.transform.position, Quaternion.identity);
                    Destroy(grabskill, 0.3f);
                }
                else
                {
                    setSkillPos = grabskill.transform.position;

                    sChara.SendMessage("SetSkill", sObj, SendMessageOptions.RequireReceiver);

                    Destroy(grabskill, 0.1f);
                }
                grabMode = false;

            }
        }
    }

    //選択しているキャラにマーカーを追従させる
    void FollowMarker()
    {
        markerHeight = selectedCharacter.GetComponent<BattleCharacterState>().charaHeight;
        selectedMarker.transform.position = new Vector3(selectedCharacter.transform.position.x,
                                                        selectedCharacter.transform.position.y + markerHeight,
                                                        selectedCharacter.transform.position.z);
        
    }

    //マーカーの距離による大きさ変化
    void OrderMarkerScalseChange()
    {
        orderMarkerTrans.localScale = currentMarkerScale * 0.1f * hitdistance;
    }


    //キャラが死んだとき、BattleCharacterStateから送られてくるSendMessage
    void CharacterDead(GameObject chara)
    {
        charactersListP.Remove(chara);
    }

    //UIなど外部からのPublicOrder変更
    public void PublicOrderOnOff()
    {
        if (!pubOrder)
        {
            pubOrder = true;
        }
        else
        {
            pubOrder = false;
        }
    }
   
}
