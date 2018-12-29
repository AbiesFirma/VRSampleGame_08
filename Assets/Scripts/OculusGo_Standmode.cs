using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusGo_Standmode : MonoBehaviour {
    
    Rigidbody _rb;
    
    [SerializeField] private float jump_power = 5.0f;        //ジャンプ力
    [SerializeField] private float ForceGravity = 5.0f;     //重力のかかり方調整
    public bool _OnGround = false;   //接地初期値＝してない
    private bool jump_bool = false;   //ジャンプ許可判定初期値オフ
    private float Jump_Timer = 0.0f;
    private float Pad_Timer = 0.0f;

    public GameObject Center_Eye_Anchor;
    public Transform _centerEyeAnchor = null;

    [SerializeField] private GameObject UIController;
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject Rot_Button;
    [SerializeField] GameObject PauseUI;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        
        //接地判定////////////////////////////
        Ray ray = new Ray(this.transform.position, -transform.up);    //Ray作成
        RaycastHit hit; 　　　　　　　　　　　　　　　　　　　　　　　//ヒットしたオブジェクト情報
        int layerMask = ~(1 << 15);                                   //レイヤーにてプレイヤー自身のレイヤーをマスクして判定が出ないようにする
        float ray_distance = 0.8f;                                    //Rayの長さ
        Debug.DrawRay(ray.origin, ray.direction * ray_distance, Color.red);  //Rayの可視化

        if (Physics.Raycast(ray, out hit, ray_distance, layerMask))           //Rayが当たっていたら
        {
            _OnGround = true;
            //Debug.Log(hit.point);//デバッグログにヒットした場所を出す
        }
        else
        {
            _OnGround = false;
        }



        //ジャンプ計算/////////////////////////////////
        if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad) && _OnGround && Jump_Timer < 1.5f)
        //if ((OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad) && !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) && _OnGround)
        {
            jump_bool = true;
        }


        //空中時での重力計算
        if (!_OnGround)
        {
            ForceGravity += -Physics.gravity.y * Time.deltaTime * 100.0f;     //空中にいるだけどんどん重力を増加.をForceGravityに入れる
            //Debug.Log("floaaaaat");
        }
        else
        {
            ForceGravity = 0;
            //Debug.Log("OnGround!!!!");
        }

        //回転画面表示
        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            Jump_Timer += Time.deltaTime;
            Pad_Timer += Time.deltaTime;
        }
        else
        {
            Jump_Timer = 0.0f;
            Pad_Timer = 0.0f;
        }

        if (Pad_Timer > 1.5f && !Rot_Button.activeSelf)
        {
            Rot_Button.SetActive(true);
            Pad_Timer = 0.0f;
        }
        else if (Pad_Timer > 1.5f && Rot_Button.activeSelf)
        {
            Rot_Button.SetActive(false);
            Pad_Timer = 0.0f;
        }



        //Menu画面表示///////////////////////////////////////
        if (OVRInput.GetDown(OVRInput.Button.Back) || OVRInput.GetDown(OVRInput.Button.Two))
        {
            UIController = GameObject.Find("UIController");

            if (Menu.activeSelf)
            {
                UIController.GetComponent<Menu_Event>().Close_Menu();
            }
            else
            {
                UIController.GetComponent<Menu_Event>().Open_Menu();
            }

            if (PauseUI.activeSelf)
            {
                UIController.GetComponent<Menu_Event>().PauseBack();
            }
        }

    }



    void FixedUpdate()
    {
        //重力。空中時には時間とともに重力増加、接地時はForceGravityが０なので０////////////////////////////
        _rb.AddForce(Vector3.down * ForceGravity, ForceMode.Acceleration);


        //Jump実行////////////////////////////////////////////
        if (jump_bool)
        {
            _rb.AddForce(Vector3.up * jump_power * 30.0f, ForceMode.Impulse);
            jump_bool = false;
            Jump_Timer = 0.0f;
        }

    }
    
}