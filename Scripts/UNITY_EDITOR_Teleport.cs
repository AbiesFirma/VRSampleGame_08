using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UNITYEDITOR上でのテレポートをするためのクラス。
/// </summary>
public class UNITY_EDITOR_Teleport : MonoBehaviour {

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
    private float _MaxDistance = 50.0f;  

    float hitdistance;                          //ヒットしたポイントまでの距離
    Vector3 hitpoint;                           //ヒットした座標

    [SerializeField] ParticleSystem telepoParticle;
    [SerializeField] AudioSource telepoAudioSource;


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
        telepoMode = false;
    }

    void Update()
    {
        var pointer = Pointer;
        if (pointer == null)
        {
            return;
        }

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

                if (hitObj.layer == 12)
                {
                    if (Input.GetMouseButton(1))
                    {
                        Teleposphere.SetActive(true);
                        Teleposphere.transform.position = hit.point;
                    }
                    //テレポ実行
                    if (Input.GetMouseButtonUp(1))
                    {
                        Telepoint = new Vector3(hit.point.x, hit.point.y + TelepoHeight, hit.point.z);
                        Teleposphere.SetActive(false);
                        //this.transform.position = Telepoint;
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
        if (Input.GetMouseButton(2))
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

    public IEnumerator Telepowait(float time)
    {

        //発射パーティクル
        telepoParticle.Play();
        //発射音
        telepoAudioSource.Play();
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
