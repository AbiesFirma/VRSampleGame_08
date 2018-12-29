using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{

    //トリガー押下中は前後ではなく上下方向の移動モードと切り替える機能をOnにする
    public bool TriggerIsUpDown = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        OVRInput.FixedUpdate();
#if UNITY_EDITOR
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector2 vector = new Vector2(x, z);
        bool triggerPressing = Input.GetButton("Fire1");
#else
         //コントローラ左右両対応
         Vector2 vector =  OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
 		bool triggerPressing = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
#endif
        //Playerの自位置を取得
        Transform trans = GetComponent<Transform>();

        //ワールド空間でのカメラのforward(up)を取得、正規化
        var padUpDir = (TriggerIsUpDown && triggerPressing) ? Vector3.up : Vector3.forward;
        Vector3 forward = Camera.main.transform.TransformDirection(padUpDir);
        forward.Normalize();

        //ワールド空間でのカメラのrightを取得、正規化
        Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);
        right.Normalize();

        //カメラのforward(up), rightに対してタッチパッドのx,yを加算 
        forward *= vector.y * Time.deltaTime;
        right *= vector.x * Time.deltaTime;

        //Player位置をright+forward(up)の加算結果位置に移動
        trans.Translate(right + forward);
    }
}