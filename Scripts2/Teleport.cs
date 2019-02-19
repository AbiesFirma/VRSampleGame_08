using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレーヤーがテレポートするためのクラス
/// </summary>
public class Teleport : MonoBehaviour {

    Vector3 Telepoint;      //テレポートするポイント
    GameObject hitObj;      //Rayがヒットしたオブジェクト
    [SerializeField] GameObject Teleposphere;      //ヒットした場所に出る光
    [SerializeField] float TelepoHeight = 1.0f;    //テレポした際の高さ

    float Pad_Timer = 0.0f;                        //モードをオンオフするためのタイマー
    [SerializeField] GameObject TelpoMode;         //UIテレポモードの文字オブジェクト  
    public bool telepoMode;                        //テレポモードオンオフのブール値
    [SerializeField] float time = 0.1f;               //テレポのウェイトタイム

    [SerializeField]
    private Transform _RightHandAnchor;
    [SerializeField]
    private Transform _LeftHandAnchor;
    [SerializeField]
    private Transform _CenterEyeAnchor;

    [SerializeField]
    private float _MaxDistance = 50.0f;          //レイの最高長

    float hitdistance;                          //ヒットしたポイントまでの距離
    Vector3 hitpoint;                           //ヒットした座標

    [SerializeField] ParticleSystem telepoParticle;
    AudioSource AudioSource;
    [SerializeField] AudioClip telepoAudioClip;

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

    void Start()
    {
        telepoMode = FeildPlayerState.TelepoMode;
        if(Teleposphere == null)
        {
            Teleposphere = GameObject.Find("TelepoSphere");
        }

        AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        var pointer = Pointer;
        if (pointer == null)
        {
            return;
        }

        //戦闘後保持するために書き込み
        FeildPlayerState.TelepoMode = telepoMode;

        if (telepoMode)
        {
            // コントローラー位置からRayを飛ばす
            Ray pointerRay = new Ray(pointer.position, pointer.forward);

            //可視化
            Debug.DrawRay(pointerRay.origin, pointerRay.direction * _MaxDistance, Color.yellow);
            int layerMask = (1 << 12 | 1 << 11);
            RaycastHit hit;
            if (Physics.Raycast(pointerRay, out hit, _MaxDistance, layerMask))
            {
                hitdistance = hit.distance;
                hitObj = hit.collider.gameObject;
                hitpoint = hit.point;

                //テレポ可能なオブジェクトにヒットしたとき
                if (hitObj.layer == 12)
                {
                    if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
                    {
                        //トリガーを押している間Rayの当たったところにポインタ（黄色のもやもや）を出す
                        Teleposphere.SetActive(true);
                        Teleposphere.transform.position = hit.point;
                    }
                    //トリガーを離したときテレポ
                    if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                    {
                        //ポイントの少し上に移動先を設定
                        Telepoint = new Vector3(hit.point.x, hit.point.y + TelepoHeight, hit.point.z);
                        Teleposphere.SetActive(false);
                        //少し硬直してコルーチン内で実行
                        StartCoroutine("Telepowait", time);
                    }
                }
                else
                {
                    Teleposphere.SetActive(false);
                    Teleposphere.transform.position = hit.point;
                }
            }
            else
            {
                Teleposphere.SetActive(false);
            }
        }

        //テレポモードオンオフ
        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            Pad_Timer += Time.deltaTime;
        }
        else
        {
            Pad_Timer = 0.0f;
        }

        if (Pad_Timer > 1.5f)
        {
            if (!telepoMode)
            {
                TelpoMode.SetActive(true);
                telepoMode = true;
            }
            else
            {
                TelpoMode.SetActive(false);
                telepoMode = false;
            }
            Pad_Timer = 0.0f;
        }
    }

    //メニューからモードをオンオフするため
    public void TelepoON()
    {
        if (!telepoMode)
        {
            TelpoMode.SetActive(true);
            telepoMode = true;
        }
        else
        {
            TelpoMode.SetActive(false);
            telepoMode = false;
        }
    }

    //演出と硬直のコルーチン
    public IEnumerator Telepowait(float time)
    {

        //発射パーティクル
        telepoParticle.Play();
        //発射音
        AudioSource.PlayOneShot(telepoAudioClip);
        //timeの時間まつ
        yield return new WaitForSeconds(time);
        //実行
        this.transform.position = Telepoint;
        //発射パーティクル
        telepoParticle.Play();
        //timeの時間まつ(硬直時間)
        yield return new WaitForSeconds(time);

    }
 }
