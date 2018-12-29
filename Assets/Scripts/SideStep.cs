using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideStep : MonoBehaviour
{
    Vector2 pos;
    
    public bool Step_bool;
    bool OnG;

    public string dir;
    Vector3 add_step;
    [SerializeField] float add_force = 20.0f;
    float ForceMultiplier;

    //bool jump_bool;
    Rigidbody _rb;

    [SerializeField] Transform _centerEyeAnchor;

    /*
    [SerializeField] GameObject F;
    [SerializeField] GameObject B;
    [SerializeField] GameObject R;
    [SerializeField] GameObject L;
    */
    // Use this for initialization
    void Start()
    {
        Step_bool = false;
        add_step = Vector3.zero;
        add_force = 20.0f;
        ForceMultiplier = 5.0f;
        
        _rb = GetComponent<Rigidbody>();
        OnG = GetComponent<OculusGo_Mover>()._OnGround;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {
            Step_bool = true;

            //タッチパッドを触っている所の座標(-1 ~ 1)取得
            pos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            
            float posX = Mathf.Abs(pos.x);
            float posY = Mathf.Abs(pos.y);
           


            if (posX <= posY && pos.y > 0.5f)
            {
                dir = "forword";
              
                //F.SetActive(true);
                //B.SetActive(false);
                //R.SetActive(false);
                //L.SetActive(false);
            }
            else if (posX <= posY && pos.y < -0.5f)
            {
                dir = "back";
                
                //F.SetActive(false);
                //B.SetActive(true);
                //R.SetActive(false);
                //L.SetActive(false);
            }
            else if (posX >= posY && pos.x > 0.5f)
            {
                dir = "right";
                
                //F.SetActive(false);
                //B.SetActive(false);
                //R.SetActive(true);
                //L.SetActive(false);
            }
            else if (posX >= posY && pos.x < -0.5f)
            {
                dir = "left";
                
                //F.SetActive(false);
                //B.SetActive(false);
                //R.SetActive(false);
                //L.SetActive(true);
            }
            else
            {
                dir = "Jump";
                
                //F.SetActive(false);
                //B.SetActive(false);
                //R.SetActive(false);
                //L.SetActive(false);
            }
            
        }

        
        if (dir == "forward" || dir == "Jump")
        {
            add_step = _centerEyeAnchor.rotation * new Vector3(pos.x * add_force * 0.1f, 0, pos.y * add_force * 0.1f);
        }
        else
        {
            add_step = _centerEyeAnchor.rotation * new Vector3(pos.x * add_force, 0, pos.y * add_force);
        }

    }

    void FixedUpdate()
    {
        if (Step_bool)
        {
            //var jump_bool = GetComponent<OculusGo_Mover>().jump_bool;
            
            _rb.AddForce(ForceMultiplier * (add_step - _rb.velocity), ForceMode.Impulse);
            
            Step_bool = false;
            //jump_bool = false;
        }
    }
    
}
