using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.SceneManagement;

public class OculusGo_Mover : MonoBehaviour
{

    Rigidbody _rb;

    [SerializeField] private float moveSpeed = 28.0f;    // 移動速度
    [SerializeField] float moveForceMultiplier = 5.0f;    // 移動速度の入力に対する追従度
    private Vector3 _currentVelocity = Vector3.zero;   //現在の移動速度

    [SerializeField] private float jump_power = 1.5f;        //ジャンプ力
    private float ForceGravity = 1.5f;     //重力のかかり方調整
    public bool _OnGround = false;   //接地初期値＝してない
    public bool jump_bool = false;   //ジャンプ許可判定初期値オフ
    private float Jump_Timer = 0.0f;
    private float Pad_Timer = 0.0f;

   // private bool Step_bool;
    string dir;
    float current_sp;

    public GameObject Center_Eye_Anchor;
    public Transform _centerEyeAnchor = null;

    [SerializeField] private GameObject UIController;
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject Rot_Button;
    [SerializeField] GameObject PauseUI;

    string active_Scene;
    bool cannot_move;

    void Start()
    {
        active_Scene = SceneManager.GetActiveScene().name;
        if (active_Scene == "VRSample08-standshooting")
        {
            cannot_move = true;
        }
        else
        {
            cannot_move = false;
        }
        
        _rb = GetComponent<Rigidbody>();
        
        //現在のmovespeedを保持
        current_sp = moveSpeed;
    }


    void Update()
    {

        //移動計算/////////////////
        //コントローラ左右両対応==タッチパッドを触っている所の座標(-1 ~ 1)取得
        Vector2 primaryTouchpad = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

        //Yの+の方向のMaxが0.5ぐらい(デバイスの不具合かも？)なので増やす
        if (primaryTouchpad.y > 0)
        {
            primaryTouchpad.y *= 2;
        }


        //向いてる方向、タッチパッドを触ってる場所から速度計算(クォータニオン＊ベクトル)       
        _currentVelocity = _centerEyeAnchor.rotation * new Vector3(primaryTouchpad.x * moveSpeed, 0, primaryTouchpad.y * moveSpeed);


        //上向いてる時に上にいっちゃうので上下方向の速度0に
        _currentVelocity.y = 0;






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

        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) && _OnGround && Jump_Timer < 1.5f)
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

        /*
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
        */



        //Menu画面表示///////////////////////////////////////
        if (OVRInput.GetDown(OVRInput.Button.Back) || OVRInput.GetDown(OVRInput.Button.Two))
        {
            UIController = GameObject.Find("UIController");

            if (Menu.activeSelf)
            {
                //Menu.SetActive(false);
                UIController.GetComponent<Menu_Event>().Close_Menu();
            }
            else
            {
                //Menu.SetActive(true);
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
        //移動実行////////////////////////////

        if (!cannot_move)
        {
            _rb.AddForce(moveForceMultiplier * (_currentVelocity - _rb.velocity), ForceMode.Force);

        }

        //重力。空中時には時間とともに重力増加、接地時はForceGravityが０なので０////////////////////////////
        _rb.AddForce(Vector3.down * ForceGravity, ForceMode.Acceleration);


        //Jump実行////////////////////////////////////////////
        if (jump_bool)
        {
            if (!cannot_move)
            {
                dir = GetComponent<SideStep>().dir;
                if (dir == "left" || dir == "right" || dir == "back")
                {
                    _rb.AddForce(Vector3.up * jump_power * 0.05f, ForceMode.Impulse);
                }
                else
                {
                    _rb.AddForce(Vector3.up * jump_power * 100.0f, ForceMode.Impulse);
                }
            }
            jump_bool = false;
            Jump_Timer = 0.0f;

            StartCoroutine("Speeddown", 0.3f);

        }

    }


    //リセット/////////////////////////////////
    public void Re_set()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        this.transform.localRotation = Quaternion.identity;
    }


    
    //ジャンプ後硬直////////////////////////////////////////
    public IEnumerator Speeddown(float time)
    {
        //現在のmovespeedを保持
        //var current_sp = moveSpeed;
        //移動速度低下
        moveSpeed *= 0.1f;
        //timeの時間まつ(硬直時間)
        yield return new WaitForSeconds(time);
        //元に戻す
        moveSpeed = current_sp;
    }

}