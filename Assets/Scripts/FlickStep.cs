using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickStep : MonoBehaviour {

    Vector2 start_pos;
    Vector2 end_pos;
    bool Flick_bool;
    bool OnTouch;
    string dir;
    Vector3 add_step;
    float add_force;
    float stop_force;
    Rigidbody _rb;

    [SerializeField] Transform _centerEyeAnchor;

    [SerializeField] GameObject F;
    [SerializeField] GameObject B;
    [SerializeField] GameObject R;
    [SerializeField] GameObject L;

    // Use this for initialization
    void Start ()
    {
        Flick_bool = false;
        add_step = Vector3.zero;
        add_force = 30.0f;
        stop_force = 10.0f;
        _rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
        {
            Flick_bool = true;
            OnTouch = true;
            //タッチパッドを触っている所の座標(-1 ~ 1)取得
            start_pos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            //
            StartCoroutine("FlickCheck", 1.0f);
        }
        else
        {
            OnTouch = false;
        }

        if (Flick_bool && !OnTouch)
        {            
            //タッチパッドを触っている所の座標(-1 ~ 1)取得
            end_pos = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            var move = end_pos - start_pos;
            float moveX = Mathf.Abs(move.x);
            float moveY = Mathf.Abs(move.y);


            if(moveX <= moveY && move.y > 0.01f)
            {
                dir = "forword";
                add_step = _centerEyeAnchor.rotation * transform.forward;

                F.SetActive(true);
                B.SetActive(false);
                R.SetActive(false);
                L.SetActive(false);
            }
            else if(moveX <= moveY && move.y < -0.01f)
            {
                dir = "back";
                add_step = _centerEyeAnchor.rotation * -transform.forward;

                F.SetActive(false);
                B.SetActive(true);
                R.SetActive(false);
                L.SetActive(false);
            }
            else if(moveX >= moveY && move.x > 0.01f)
            {
                dir = "right";
                add_step = _centerEyeAnchor.rotation * transform.right;

                F.SetActive(false);
                B.SetActive(false);
                R.SetActive(true);
                L.SetActive(false);
            }
            else if(moveX >= moveY && move.x < -0.01f)
            {
                dir = "left";
                add_step = _centerEyeAnchor.rotation * -transform.right;
                F.SetActive(false);
                B.SetActive(false);
                R.SetActive(false);
                L.SetActive(true);
            }
            else if(moveX <0.01f && moveY < 0.01f)
            {
                dir = "TAP";
                add_step = Vector3.zero;
                F.SetActive(false);
                B.SetActive(false);
                R.SetActive(false);
                L.SetActive(false);
            }
            else
            {
                add_step = Vector3.zero;
            }

        }
    }

    void FixedUpdate()
    {
            _rb.AddForce(add_step * add_force - (_rb.velocity * stop_force), ForceMode.Impulse);
            Flick_bool = false;
    }


    //呼び出し用コルーチン
    public IEnumerator FlickCheck(float time)
    {
        //timeの時間まつ
        yield return new WaitForSeconds(time);
        if (OnTouch)
        {
            Flick_bool = false;
        }
    }
}





