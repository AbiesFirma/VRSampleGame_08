using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 視線によるコントロール（ゲイズ）を管理するクラス
/// </summary>

public class GazePointer : MonoBehaviour {

    [SerializeField] GameObject handStick;      //手の位置にある棒
    [SerializeField] GameObject handWindow;     //手の位置に表示されるウインドウ
    [SerializeField] GameObject footStick;      //足の位置にあるオブジェクト
    [SerializeField] GameObject footWindow;     //足に表示されるウインドウ

    [SerializeField] Transform centerEyeAnchor;

    //レイ用
    float maxRayDistance = 50.0f;
    float hitdistance;
    GameObject hitObj;
    public Vector3 hitpoint { get; private set; }
    Vector3 hitNormal;


    [SerializeField] GameObject gazePointer;    //ポインタオブジェクト
    [SerializeField] GameObject circleGage;     //円形ゲージ
    [SerializeField] Image circleBar;
    float circleGageRatio;
    //[SerializeField] float scaleRatio = 0.1f;   //距離によってポインタの大きさを変えるための割合
    RectTransform gageTrans;
    RectTransform gazePointerTrans;

    GameObject gazeMenu;                       //ゲイズによって開くメニュー
    [SerializeField] float gazeMenuOpenTime = 2.0f;           //注視に必要な時間
    [SerializeField] float gazePointerScaleToDistance = 0.1f;     //ポインタがヒットした距離によって大きさが変わる係数
    float gazeOnTimer;
    float gazeOffTimer;
    float gazeStandOnTimer;
    float handTimer;
    Vector3 currentG;
    Vector3 currentP;

    AudioSource audioSource;
    [SerializeField] AudioClip gazeMenuOpenAudio;
    [SerializeField] AudioClip gazeMenuCloseAudio;
    bool gazeAudio;


    void Start () {
        audioSource = GetComponent<AudioSource>();

        gazeOnTimer = 0.0f;
        gazeOffTimer = 0.0f;
        gazeStandOnTimer = 0.0f;
        gageTrans = circleGage.GetComponent<RectTransform>();
        gazePointerTrans = gazePointer.GetComponent<RectTransform>();
        gazeAudio = true;

        //開いてるものは閉じる
        var allgazeMenu = GameObject.FindGameObjectsWithTag("GazeMenu");
        
        //gazeMenu = allgazeMenu[0];
        if (allgazeMenu.Length != 0)
        {
            gazeMenu = allgazeMenu[0];
            for (int i = 0; i < allgazeMenu.Length; i++)
            {
                allgazeMenu[i].SetActive(false);
            }
        }

        //円形ゲージのための比率
        circleGageRatio = 1 / gazeMenuOpenTime;
        circleBar.fillAmount = gazeOnTimer * circleGageRatio;
        circleGage.SetActive(false);
        currentG = gageTrans.localScale;
        currentP = gazePointerTrans.localScale;
    }

    void Update()
    {
        // 視線方向にRayを飛ばす
        Ray gazeRay = new Ray(centerEyeAnchor.position, centerEyeAnchor.forward);
        //可視化
        Debug.DrawRay(gazeRay.origin, gazeRay.direction * maxRayDistance, Color.red);

        //int layerMask = (1 << 25 | 1 << 26 | 1 << 19 | 1 << 17);
        RaycastHit hit;

        if (Physics.Raycast(gazeRay, out hit, maxRayDistance/*, layerMask*/))
        {
            hitdistance = hit.distance;
            hitObj = hit.collider.gameObject;
            hitpoint = hit.point;
            hitNormal = hit.normal;
            

            gazePointer.transform.position = hitpoint;
            gazePointer.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hitNormal);

            GazePointerScalseChange();
            HandWindowControll();
            FootWindowControll();

            //Gazeメニューが存在しない場合以外
            if (gazeMenu != null)
            {
                //キャラ
                if (hitObj.layer == 25)
                {
                    var lookChara = hitObj;
                    var lookCharaComp = lookChara.GetComponent<BattleCharacterState>();

                    if (lookCharaComp != null)
                    {
                        GameObject charaGaze = lookChara.GetComponent<BattleCharacterState>().gazeMenu;

                        //一つメニューが出てる状態で他のキャラを見たとき前のメニューをけす
                        if (charaGaze != gazeMenu)
                        {
                            gazeMenu.SetActive(false);
                        }
                        gazeMenu = charaGaze;

                        //メニューが出てなかったらタイマースタート円形ゲージオン
                        if (!gazeMenu.activeSelf)
                        {
                            gazeOnTimer += Time.deltaTime;
                            circleGage.SetActive(true);
                            circleGage.transform.position = hitpoint;
                            circleBar.fillAmount = gazeOnTimer * circleGageRatio;
                        }

                        //タイマーが溜まったらメニューオン
                        if (gazeOnTimer >= gazeMenuOpenTime)
                        {
                            gazeMenu.SetActive(true);
                            gazeOnTimer = 0.0f;
                            circleGage.SetActive(false);
                        }
                    }
                }

                //他に目をやったとき～メニューが出てたら
                else if (hitObj.layer != 25 && hitObj.layer != 26 && hitObj.layer != 17 && gazeMenu.activeSelf)
                {
                    circleGage.SetActive(false);

                    gazeOffTimer += Time.deltaTime;
                    if (gazeOffTimer >= 2.0f)
                    {
                        gazeMenu.SetActive(false);
                        gazeOffTimer = 0.0f;

                        var allgazeMenu = GameObject.FindGameObjectsWithTag("GazeMenu");
                        if (allgazeMenu != null)
                        {
                            for (int i = 0; i < allgazeMenu.Length; i++)
                            {
                                allgazeMenu[i].SetActive(false);
                            }
                        }
                    }

                }

                //Gazeスタンド（注視によりポップアップするテキスト、キャンバスなど）
                else if (hitObj.layer == 27)
                {
                    GazeStand();
                }


                else
                {
                    GazeReset();
                }

            }

        }
        //Rayが何にもヒットしてない
        else
        {
            GazeReset();
        }

        

    }   

    //注視することにより何かを表示するオブジェクト（GazeStand）
    void GazeStand()
    {          
        gazeStandOnTimer += Time.deltaTime;
        circleGage.SetActive(true);
        circleGage.transform.position = hitpoint;
        circleBar.fillAmount = gazeStandOnTimer * circleGageRatio;

        GazePointerScalseChange();

        if (gazeStandOnTimer >= gazeMenuOpenTime)
        {            
            var openItem = hitObj.GetComponent<GazeStandController>().gazeOpenItem;

            if (!openItem.activeSelf)
            {
                openItem.SetActive(true);                
            }
            else
            {
                openItem.SetActive(false);
            }
            gazeStandOnTimer = 0.0f;
            circleGage.SetActive(false);
        }
    }

    //ポインタのヒットした距離による大きさ変化
    void GazePointerScalseChange()
    {
        gageTrans.localScale = currentG * gazePointerScaleToDistance * hitdistance;
        gazePointerTrans.localScale = currentP * gazePointerScaleToDistance * hitdistance;
        
    }


    void GazeReset()
    {
        circleGage.SetActive(false);
        gazeOnTimer = 0.0f;
        gazeOffTimer = 0.0f;
        gazeStandOnTimer = 0.0f;
        
    }

    //手足のウインドウ（注視ではなくその辺を見たら開く、はずしたら少しして閉じる）
    void HandWindowControll()
    {
        if (hitObj == handStick || hitObj == handWindow || hitObj.name == "HandBattleStateController" || hitObj.tag == "HandWindowMenu")
        {
            handWindow.SetActive(true);
            handStick.SetActive(false);

            if (gazeAudio)
            {
                audioSource.PlayOneShot(gazeMenuOpenAudio);
                gazeAudio = false;
            }
        }        
        else
        {
            if (handWindow.activeSelf)
            {
                handTimer += Time.deltaTime;
                if (handTimer > 2.0f)
                {                    
                    handWindow.SetActive(false);
                    handStick.SetActive(true);
                    handTimer = 0.0f;
                    audioSource.PlayOneShot(gazeMenuCloseAudio);
                    gazeAudio = true;
                }
            }
            
        }
    }
    void FootWindowControll()
    {
        if (hitObj == footStick || hitObj == footWindow || hitObj.name == "HandBattleStateController")
        {
            footWindow.SetActive(true);
            footStick.SetActive(false);
            
            if (gazeAudio)
            {
                audioSource.PlayOneShot(gazeMenuOpenAudio);
                gazeAudio = false;
            }
        }
        else
        {
            if (footWindow.activeSelf)
            {
                handTimer += Time.deltaTime;
                if (handTimer > 2.0f)
                {
                    footWindow.SetActive(false);
                    footStick.SetActive(true);
                    handTimer = 0.0f;
                    audioSource.PlayOneShot(gazeMenuCloseAudio);
                    gazeAudio = true;
                }
            }

        }
    }

}
