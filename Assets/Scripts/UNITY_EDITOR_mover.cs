using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UNITY_EDITOR_mover : MonoBehaviour
{
#if UNITY_EDITOR
    Rigidbody _rb;
    public bool isUseCameraDirection;    // カメラの向きに合わせて移動させたい場合はtrue
    public float moveSpeed = 50.0f;    // 移動速度
    public float moveForceMultiplier = 1.0f;    // 移動速度の入力に対する追従度
    public float stopForce = 25.0f;    //減衰力（あげると移動速度も上がりにくくなるので適宜上げる）
    public GameObject mainCamera;
    public bool _OnGround = false;   //接地初期値＝してない
    bool jump_bool = false;
    
    float _horizontalInput;
    float _verticalInput;
    Transform _pos;

    public float jump_power = 5.0f;    //ジャンプ力
    public float ForceGravity = 10.0f;     //重力のかかり方調整

    [SerializeField] private GameObject UIController;
    [SerializeField] GameObject Menu;
    //[SerializeField] GameObject Rot_Button;
    [SerializeField] GameObject PauseUI;

    string activeSceneName;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _pos = GetComponent<Transform>();
        Vector3 pos = transform.position; 

        if (mainCamera == null)
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        

    }


    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        //接地判定ブロック
        Ray ray = new Ray(this.transform.position, -transform.up);    //Ray作成
        RaycastHit hit; 　　　　　　　　　　　　　　　　　　　　　　　//ヒットしたオブジェクト情報
        int layerMask = ~(1 << 15);                                   //レイヤーにてプレイヤーのレイヤーをマスクして判定が出ないようにする
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


        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && _OnGround )
        {
            jump_bool = true;            
        }
        
        //空中時での重力計算
        if (_OnGround == false)
        {
            ForceGravity += -Physics.gravity.y * Time.deltaTime * 100.0f;     //重力を増加
            //Debug.Log("floaaaaat");
        }
        else
        {
            ForceGravity = 0;
            //Debug.Log("OnGround!!!!");
        }

        //Menu画面表示///////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
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
        // 移動速度の入力
        Vector3 moveVector = Vector3.zero;

        if (isUseCameraDirection)
        {
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;
            cameraForward.y = 0.0f;    // 水平方向に移動させたい場合はy方向成分を0にする
            cameraRight.y = 0.0f;

            moveVector = moveSpeed * (cameraRight.normalized * _horizontalInput + cameraForward.normalized * _verticalInput);     //ベクトルの加算で移動方向を算出
        }
        else
        {
            moveVector.x = moveSpeed * _horizontalInput;
            moveVector.z = moveSpeed * _verticalInput;
        }

        _rb.AddForce(moveForceMultiplier * (moveVector - _rb.velocity * stopForce), ForceMode.Acceleration);          //AddForceに入力移動速度と現在の移動速度の差に応じて力を加える

        _rb.AddForce(Vector3.down * ForceGravity, ForceMode.Acceleration);            //空中時には時間とともに重力増加、接地時は０


        /*テスト用///////////////////////////////////////////////////////
        if(Input.GetMouseButton(1))
        {
            _rb.AddTorque(transform.up * 100);
        }
        ///////////////////////////////////////////////////////
        */

        //Jump
        if (jump_bool)
        {
            _rb.AddForce(Vector3.up * jump_power * 500.0f);
            jump_bool= false;
        }


        //意図しない回転を防ぐためなんのキー入力もないときは回転ベクトルを０にする
        //if (!Input.anyKey)
        //{
        //    _rb.angularVelocity = Vector3.zero;
        //}
        
    }

    //ほかのスクリプトから呼べるリセット関数
    public void Re_set()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        this.transform.localRotation = Quaternion.identity;
    }

    
        
#endif
}                              