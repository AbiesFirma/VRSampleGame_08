using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

[RequireComponent(typeof(Rigidbody))]   //Rigidbody必須(Add時になければ自動で追加される)

public class RigidbodyMover5 : MonoBehaviour
{

    [SerializeField]
    private Rigidbody _rigidbody = null;

    //VR上で目の中心(見ている方向)を確認する用のアンカー
    [SerializeField]
    private Transform _centerEyeAnchor = null;

    //移動速度の係数
    [SerializeField]
    private float _moveSpeed = 5;

    //現在の移動速度
    private Vector3 _currentVelocity = Vector3.zero;

    //===現在の回転速度
    private Vector3 _currentRotation = Vector3.zero;

    //パッド押下中は@@ではなく@@モードと切り替える機能をOnにする
    public bool PadIsUpDown = true;


 
    //コンポーネントがAddされた時に実行される
    private void Reset()
    {
        //中心のアンカー取得
        _centerEyeAnchor = transform.Find("TrackingSpace/CenterEyeAnchor");

        //Rigidbody取得、初期設定
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _rigidbody.useGravity = true;
    }

    //=================================================================================
    //更新
    //=================================================================================


    //入力はUpdateで確認
    private void Update()
    {
        //コントローラ左右両対応==タッチパッドを触っている所の座標(-1 ~ 1)取得

        Vector2 primaryTouchpad = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
       
        //コントローラーの傾きを取得
        OVRInput.Controller activeController = OVRInput.GetActiveController();
        Quaternion rot = OVRInput.GetLocalControllerRotation(activeController);

        /*=================使わなそうなのでとりあえず無効化しとく===================================
         
        //視点HMDの向き取得
        Quaternion direction = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.Head);

        //Playerの自位置を取得(transform用)
        Transform trans = GetComponent<Transform>();

        //ワールド空間でのコントローラーのforwardを取得、正規化
        Vector3 wforward = rot * Vector3.forward;
        wforward.Normalize();
        
        //ワールド空間でのコントローラーのrightを取得、正規化
        Vector3 wright = rot * Vector3.right;
        wright.Normalize();
        
        =========================================================================================*/

       
        //Yの+の方向のMaxが0.5ぐらい(デバイスの不具合かも？)なので増やす
        if (primaryTouchpad.y > 0)
        {
            primaryTouchpad.y *= 2;
        }
        
        //向いてる方向、タッチパッドを触ってる場所から速度計算(クォータニオン＊ベクトル)
        _currentVelocity = _centerEyeAnchor.rotation * new Vector3(primaryTouchpad.x, 0, primaryTouchpad.y);

        //上向いてる時に上にいっちゃうので上下方向の速度0に
        _currentVelocity.y = 0;

        //上下方向の速度を減らした分を左右に振るために正規化
        float speedMagnitude = _moveSpeed * primaryTouchpad.magnitude;//速度の大きさ
        _currentVelocity = _currentVelocity.normalized * speedMagnitude;

        //コントローラーの向きの計算
        _currentRotation = new Vector3(0, rot[1], 0);
            
    }
    
    //物理演算関係(今回は速度)はFixedUpdateで設定 
    private void FixedUpdate()
    {
        bool PadPressing = OVRInput.Get(OVRInput.Button.One);　　　//パッド押してるかの取得

        if ((PadIsUpDown == true) && (PadPressing == true))   //PadIsUpDownがオンでかつパッドが押されてるとき
        {
            _rigidbody.velocity = _currentVelocity;   //押してる時の移動
            _rigidbody.angularVelocity = new Vector3(0, -_currentRotation.y, 0);　　//回転
        }
        else
        {
            _rigidbody.velocity = _currentVelocity;   //押してない時の移動
            _rigidbody.angularVelocity = new Vector3(0, 0, 0);   //回転しない
        }
        
    }
}