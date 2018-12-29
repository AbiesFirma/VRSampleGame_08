using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3 : MonoBehaviour
{

    public float speed = 2.0f;

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
#if UNITY_EDITOR
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector2 vector = new Vector2(x, z);
        Quaternion rot = Quaternion.identity;
#else
         //コントローラーのタッチパッドの入力値を取得
         //値はx,yそれぞれ0を中心とした-1.0～1.0のアナログ値
         Vector2 vector = OVRInput.Get (OVRInput.Axis2D.PrimaryTouchpad);
 
         //コントローラーの傾きを取得
         OVRInput.Controller activeController = OVRInput.GetActiveController();
         Quaternion rot = OVRInput.GetLocalControllerRotation(activeController);
#endif
        //Playerの自位置を取得
        Transform trans = GetComponent<Transform>();

        //ワールド空間でのコントローラーのforwardを取得、正規化
        Vector3 wforward = rot * Vector3.forward;
        wforward.Normalize();

        //ワールド空間でのコントローラーのrightを取得、正規化
        Vector3 wright = rot * Vector3.right;
        wright.Normalize();

        //コントローラーのforward, right方向に対してタッチパッドのx,y移動量を乗算 
        wforward *= vector.y * Time.deltaTime * speed;
        wright *= vector.x * Time.deltaTime * speed;

        //Player位置をforward+rightの加算結果位置に移動
        trans.Translate(wforward + wright);
    }
}
