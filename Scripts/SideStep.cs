using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動用クラスに結合したので未使用のサイドステップのクラス
/// </summary>
public class SideStep : MonoBehaviour
{
    Vector2 pos;

    public bool Step_bool;
    bool OnG;

    public string dir;
    Vector3 add_step;
    [SerializeField] float add_force = 20.0f;
    float ForceMultiplier;
    
    Rigidbody _rb;

    [SerializeField] Transform _centerEyeAnchor;

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
                
            }
            else if (posX <= posY && pos.y < -0.5f)
            {
                dir = "back";
                
            }
            else if (posX >= posY && pos.x > 0.5f)
            {
                dir = "right";
                
            }
            else if (posX >= posY && pos.x < -0.5f)
            {
                dir = "left";
                
            }
            else
            {
                dir = "Jump";
                
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
        if (Step_bool && OnG)
        {
            _rb.AddForce(ForceMultiplier * (add_step - _rb.velocity), ForceMode.Impulse);

            Step_bool = false;      
        }
    }

}