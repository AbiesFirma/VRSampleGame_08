using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Controller : MonoBehaviour {

    private Animator animator;
    public GameObject player_root;
    GameObject main_camera;
    
    Vector3 player_pos;
    Vector3 transform_pos;
    Vector3 move_dir;
    public float run_speed =3.0f;

    public AudioSource audioSource;
    public AudioClip[] _se;

    


    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();

        main_camera = GameObject.Find("CenterEyeAnchor");
        
        player_pos = this.transform.position;
       
        /*        
        #if UNITY_EDITOR
                main_camera = player_root.GetComponent<UNITY_EDITOR_mover>().mainCamera;
        #else
                main_camera = player_root.GetComponent<OculusGo_Mover>().Center_Eye_Anchor;
        #endif
        */
    }

    private void Update()
    {
        var camera_forward =main_camera.transform.forward;
        camera_forward.y = 0;
        this.transform.forward = camera_forward;       
    }

    private void FixedUpdate()
    {
        Vector3 cameraForward = main_camera.transform.forward;
        Vector3 cameraRight = main_camera.transform.right;
        Vector2 camara_horizontal_right = new Vector2(cameraRight.x, cameraRight.z);
        Vector2 camara_horizontal_forward = new Vector2(cameraForward.x, cameraForward.z);

#if UNITY_EDITOR
        bool OnG = player_root.GetComponent<UNITY_EDITOR_mover>()._OnGround;
#else
        bool OnG = player_root.GetComponent<OculusGo_Mover>()._OnGround;
#endif

        transform_pos = this.transform.position;
        move_dir = transform_pos - player_pos;

        Vector2 horizontal_vec = new Vector2(move_dir.x, move_dir.z);
        float horizontal_mag = horizontal_vec.magnitude * 100;   //runspeedとの比較のため100をかけて調整

        //Debug.Log(horizontal_mag);

        float move_angle = Vector2.Angle(camara_horizontal_right, horizontal_vec);
        float move_angleF = Vector2.Angle(camara_horizontal_forward, horizontal_vec);
        

        //接地してたら
        if (OnG)
        {
            animator.SetBool("Jump", false);
                        
            //少しでも動いてたら
            if (horizontal_mag > 0.001f)
            {
                if (move_angle < 1 || move_angle > 179)
                {
                    animator.SetBool("walk", false);
                    animator.SetBool("Run", false);
                    animator.SetBool("Back", false);

                    animator.SetBool("Side", true);
                    //Debug.Log("OnGround-Sidewalk-2");
                }
                else if (move_angleF > 91.0f)
                {
                    animator.SetBool("walk", false);
                    animator.SetBool("Run", false);
                    animator.SetBool("Side", false);

                    animator.SetBool("Back", true);
                    //Debug.Log("OnGround-Buckwalk-2");
                }
                else if (horizontal_mag < run_speed)
                {
                    animator.SetBool("Run", false);
                    animator.SetBool("Back", false);
                    animator.SetBool("Side", false);

                    animator.SetBool("walk", true);
                    //Debug.Log("OnGround-Walk!!");                      
                }
                else
                {
                    animator.SetBool("walk", false);
                    animator.SetBool("Back", false);
                    animator.SetBool("Side", false);

                    animator.SetBool("Run", true);
                    //Debug.Log("OnGround-Run--2-!!");
                }
                
                player_pos = this.transform.position;
            }

            //止まってたら Idle状態
            else
            {
                animator.SetBool("walk", false);
                animator.SetBool("Run", false);
                animator.SetBool("Side", false);
                animator.SetBool("Back", false);
                //Debug.Log("OnGround-Stop--Idle-!!");
              
                player_pos = this.transform.position;
            }
        }

        //接地してない
        else
        {
            animator.SetBool("Jump", true);
                        
            animator.SetBool("walk", false);
            animator.SetBool("Run", false);
            animator.SetBool("Side", false);
            animator.SetBool("Back", false);

            //Debug.Log("Jumpppppppp----!!!");

            player_pos = this.transform.position;            
        }       
    }

    void step()
    {
        //Debug.Log("step");
        audioSource.PlayOneShot(_se[0]);
    }
    void wstep()
    {
        //Debug.Log("w");
        audioSource.PlayOneShot(_se[1]);
    }
    void jumpup()
    {
        //Debug.Log("up");
        audioSource.PlayOneShot(_se[2]);
    }
    void jumpdown()
    {
        //Debug.Log("down");
        audioSource.PlayOneShot(_se[3]);
    }
}
