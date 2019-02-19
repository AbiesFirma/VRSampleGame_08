using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// OculusGoでキャラクターを移動するための基本となるクラス
/// </summary>
public class OculusGo_Mover : MonoBehaviour
{
    Rigidbody _rb;
    float rbMass;
    [SerializeField] private float moveSpeed = 28.0f;    // 移動速度
    float moveForceMultiplier = 5.0f;    // 移動速度の入力に対する追従度
    Vector3 _currentVelocity = Vector3.zero;   //現在の移動速度

    [SerializeField] private float jump_power = 1.5f;        //ジャンプ力
    float ForceGravity = 1.1f;     //重力のかかり方調整
    public bool _OnGround          //接地
    {
        get;
        private set;
    }                             
    bool jump_bool = false;   //ジャンプ許可判定
    float current_sp;    //現在のスピードを保持するため

    bool Step_bool;   //ステップ許可
    enum Stepdir        //ステップ方向
    {
        forward,
        back,
        right,
        left,
        jump
    }
    Stepdir stepdir;    

    float step_addforce = 20.0f;　　　　//ステップ係数
    float step_ForceMultiplier = 5.0f;           //追従度
    float foward_power = 1.0f;    //前方向へのジャンプの際の水平方向の力（1.0ですってぷと同じ）

    Vector3 add_step;       //実際に与えるすてっぷ力,方向
    float add_jump;         //実際に与えるジャンプ力（方向は真上）


    public GameObject Center_Eye_Anchor;
    public Transform _centerEyeAnchor = null;

    [SerializeField] private GameObject UIController;
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject PauseUI;
    AudioSource audioSource;
    [SerializeField] AudioClip menuAudio;
    
    bool cannot_move;       //移動できないゲームなどでtrueにする


    private void Awake()
    {
        var stage = SceneManager.GetActiveScene().name;
        if (stage == "VRSample08-summon-field")
        {
            if (FeildPlayerState.feildPlayerPos != Vector3.zero)
            {
                transform.position = FeildPlayerState.feildPlayerPos;
            }
        }
    }

    void Start()
    {
        string active_Scene = SceneManager.GetActiveScene().name;
        if (active_Scene == "VRSample08-standshooting")
        {
            cannot_move = true;
        }
        else
        {
            cannot_move = false;
        }

        _rb = GetComponent<Rigidbody>();
        rbMass = _rb.mass;
        //現在のmovespeedを保持
        current_sp = moveSpeed;

        audioSource = GetComponent<AudioSource>();

        _OnGround = false;
    }




    void Update()
    {

        //移動計算//////////////////////////
        //コントローラ左右両対応==タッチパッドを触っている所の座標(-1 ~ 1)取得
        Vector2 primaryTouchpad = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

        //Yの+の方向のMaxが0.5ぐらい(デバイスの不具合？)なので増やす
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
        }
        else
        {
            _OnGround = false;
        }
        



        //ステップ計算////////////////////////////////////////////////
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) && _OnGround)
        {
            Step_bool = true;

            //タッチパッドを触っている所の座標を移動とは別途取得
            Vector2 pad_pos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            //方向判定のためパッド座標の絶対値を計算
            float posX = Mathf.Abs(pad_pos.x);
            float posY = Mathf.Abs(pad_pos.y);

            //ステップとジャンプのための方向定義
            //XYの絶対値比較とパッド位置により判定
            if (posX <= posY && pad_pos.y > 0.5f)
            {
                stepdir = Stepdir.forward;
            }
            else if (posX <= posY && pad_pos.y < -0.5f)
            {
                stepdir = Stepdir.back;
            }
            else if (posX >= posY && pad_pos.x > 0.5f)
            {
                stepdir = Stepdir.right;
            }
            else if (posX >= posY && pad_pos.x < -0.5f)
            {
                stepdir = Stepdir.left;
            }
            else
            {
                stepdir = Stepdir.jump;
            }

            //方向によりステップの力を変化させる（カメラrotクオータニオン×入力位置Vector3）
            if (stepdir == Stepdir.forward || stepdir == Stepdir.jump)　　　　//上方向にも力があるとき
            {
                add_step = _centerEyeAnchor.rotation * new Vector3(pad_pos.x * step_addforce * foward_power, 0, pad_pos.y * step_addforce * 0.1f);
            }
            else                                                              //上方向には力がなく素早く移動する感じのとき
            {
                add_step = _centerEyeAnchor.rotation * new Vector3(pad_pos.x * step_addforce, 0, pad_pos.y * step_addforce);
            }

        }




        //ジャンプ計算/////////////////////////////////
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) && _OnGround)
        {
            jump_bool = true;

            //ステップでの方向判定によりジャンプも与える力を変化させる
            if (stepdir == Stepdir.left || stepdir == Stepdir.right || stepdir == Stepdir.back)
            {
                 add_jump =jump_power * 0.05f;
            }
            else
            {
                add_jump = jump_power * 100.0f;
            }
        }




        //空中時での重力計算
        if (!_OnGround)
        {
            ForceGravity += -Physics.gravity.y * Time.deltaTime * 100.0f;     //空中にいるだけ少し重力を増加
        }
        else
        {
            ForceGravity = 0;
        }

        



        //Menu画面表示///////////////////////////////////////
        if (OVRInput.GetDown(OVRInput.Button.Back) || OVRInput.GetDown(OVRInput.Button.Two))
        {
            UIController = GameObject.Find("UIController");

            if (Menu.activeSelf)
            {
                //Menu.SetActive(false);
                UIController.GetComponent<Menu_Event>().Close_Menu();
                //audioSource.PlayOneShot(menuAudio);
            }
            else
            {
                //Menu.SetActive(true);
                UIController.GetComponent<Menu_Event>().Open_Menu();
                audioSource.PlayOneShot(menuAudio);
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
                _rb.AddForce(Vector3.up * add_jump, ForceMode.Impulse);
                StartCoroutine("Speeddown", 0.3f);   //ジャンプ後硬直
            }
            jump_bool = false;

        }

        //Step実行///////////////////////////////////////////
        if(Step_bool)
        {
            if(!cannot_move)
            {
                _rb.AddForce(step_ForceMultiplier * (add_step - _rb.velocity), ForceMode.Impulse);
            }
            Step_bool = false;
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

    void EnemyEncount(GameObject enemy)
    {
        //_rb.mass *= 100.0f;
    }

}