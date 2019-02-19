using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動による操作キャラのアニメーション変化のクラス
/// </summary>
/// 
public class Animation_Controller :  MonoBehaviour{ 

    private Animator animator;
    public GameObject player_root;
    GameObject main_camera;

    Vector3 player_pos;
    Vector3 transform_pos;
    Vector3 move_dir;
    [SerializeField] float run_speed = 3.0f;

    AudioSource audioSource;
    [SerializeField] AudioClip[] _se;

    bool _OnGround;

    enum AnimeState
    {
        Idle,
        Walk,
        Run,
        Side,
        Back,
        Jump,
    }
    AnimeState aState;
        
    
    
    void Start ()
    {
        player_root = transform.root.gameObject;
        animator = GetComponent<Animator>();
        main_camera = GameObject.Find("CenterEyeAnchor");        
        player_pos = this.transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //カメラの正面とキャラクターの向きを合わせる
        var camera_forward =main_camera.transform.forward;
        camera_forward.y = 0;
        this.transform.forward = camera_forward;

        _OnGround = player_root.GetComponent<OculusGo_Mover>()._OnGround;
#if UNITY_EDITOR
        _OnGround = player_root.GetComponent<UNITY_EDITOR_mover>()._OnGround;     //UNITY_EDITOR
#endif
    }

    private void FixedUpdate()
    {
        Vector3 cameraForward = main_camera.transform.forward;
        Vector3 cameraRight = main_camera.transform.right;
        Vector2 camara_horizontal_right = new Vector2(cameraRight.x, cameraRight.z);
        Vector2 camara_horizontal_forward = new Vector2(cameraForward.x, cameraForward.z);

        /*
        _OnGround = player_root.GetComponent<OculusGo_Mover>()._OnGround;
#if UNITY_EDITOR
        _OnGround = player_root.GetComponent<UNITY_EDITOR_mover>()._OnGround;     //UNITY_EDITOR
#endif
*/
        transform_pos = this.transform.position;
        move_dir = transform_pos - player_pos;

        Vector2 horizontal_vec = new Vector2(move_dir.x, move_dir.z);
        float horizontal_mag = horizontal_vec.magnitude * 100;   //水平方向の移動ベクトルの大きさ。runspeedとの比較のため100をかけて調整
        
        float move_angle = Vector2.Angle(camara_horizontal_right, horizontal_vec);      //向いている方向（カメラ）から真右と移動方向の角度差
        float move_angleF = Vector2.Angle(camara_horizontal_forward, horizontal_vec);   //向いている方向（カメラ）正面と移動方向の角度差
        
        //Stateを設定////////////////////////////////////////////
        //接地してたら
        if (_OnGround)
        {
            //animator.SetBool("Jump", false);
                        
            //少しでも動いてたら
            if (horizontal_mag > 0.001f)
            {
                //移動方向とカメラ真右の角度差がほぼ左右だったら（角度差は0-180）
                if (move_angle < 1 || move_angle > 179)
                {
                    aState = AnimeState.Side;
                }
                //移動方向とカメラ正面の角度差が９１度より多い、つまり真横より後ろ方向の場合
                else if (move_angleF > 91.0f)
                {
                    aState = AnimeState.Back;
                }
                //水平方向の移動ベクトルの大きさがrunspeed(係数)より小さい（遅めの移動）の場合
                else if (horizontal_mag < run_speed)
                {
                    aState = AnimeState.Walk;   
                }
                //その他（左右真横、後ろ、遅い以外の移動）
                else
                {
                    aState = AnimeState.Run;
                }
            }
            //止まってたら Idle状態
            else
            {
                aState = AnimeState.Idle;
            }
        }

        //接地してない
        else
        {
            aState = AnimeState.Jump;
        }
        

        //AnimeStateによるアニメーションの切り替え///////////////////////////////////
        switch (aState)
        {
            case AnimeState.Idle:
                animator.SetBool("Jump", false);
                animator.SetBool("walk", false);
                animator.SetBool("Run", false);
                animator.SetBool("Side", false);
                animator.SetBool("Back", false);
                break;

            case AnimeState.Walk:
                animator.SetBool("Jump", false);
                animator.SetBool("Run", false);
                animator.SetBool("Back", false);
                animator.SetBool("Side", false);

                animator.SetBool("walk", true);
                break;

            case AnimeState.Run:
                animator.SetBool("Jump", false);
                animator.SetBool("walk", false);
                animator.SetBool("Back", false);
                animator.SetBool("Side", false);

                animator.SetBool("Run", true);
                break;

            case AnimeState.Side:
                animator.SetBool("Jump", false);
                animator.SetBool("walk", false);
                animator.SetBool("Run", false);
                animator.SetBool("Back", false);

                animator.SetBool("Side", true);
                break;

            case AnimeState.Back:
                animator.SetBool("Jump", false);
                animator.SetBool("walk", false);
                animator.SetBool("Run", false);
                animator.SetBool("Side", false);

                animator.SetBool("Back", true);
                break;

            case AnimeState.Jump:
                animator.SetBool("Jump", true);

                animator.SetBool("walk", false);
                animator.SetBool("Run", false);
                animator.SetBool("Side", false);
                animator.SetBool("Back", false);
                break;
        }
        //次のフレームに向けての更新
        player_pos = this.transform.position;
    }




    //アニメーションイベントによるSE再生/////////////////////////////////////
    void step()
    {
        audioSource.PlayOneShot(_se[0]);
    }
    void wstep()
    {
        audioSource.PlayOneShot(_se[1]);
    }
    void jumpup()
    {
        audioSource.PlayOneShot(_se[2]);
    }
    void jumpdown()
    {
        audioSource.PlayOneShot(_se[3]);
    }
}
