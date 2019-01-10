using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

/// <summary>
/// 参考用
/// </summary>
public class PlayerDirectionControll : MonoBehaviour {

    //VR上で目の中心(見ている方向)を確認する用のアンカー
    [SerializeField]
    private Transform _VRcenterEyeAnchor = null;

    //playerの正面を設定
    [SerializeField]
    private Transform _PlayerforwordAnchor = null;

    //初期化//コンポーネントがAddされた時に実行される
    private void Reset()
    {
        //中心のアンカー取得
        _VRcenterEyeAnchor = transform.Find("TrackingSpace/VRCenterEyeAnchor");
        
        //playerの正面アンカー取得
        _PlayerforwordAnchor = transform.Find("TrackingSpace/PlayerForwordAnchor");

    }

    //プレイヤーの向き変更のための設定項目

    private Vector3 Player_pos; //プレイヤーのポジション
    private Vector3 Player_forword; //プレイヤーの正面方向
    private Vector3 VRcenterEye_dif; //向いてる方向
    private Vector3 Forward_set; //カメラの向きとプレイヤー正面を合わせるためのもの

    //プレイヤーの向き変更のための初期値取得
    void Start()
    {
        Player_pos = GetComponent<Transform>().position; //最初の時点でのプレイヤーのポジションを取得
        Player_forword = _PlayerforwordAnchor.position - Player_pos;  //正面アンカーとプレイヤーの差分により正面方向ベクトルを取得
        Player_forword.y = 0;  //上下はいらないので0
     
        transform.rotation = Quaternion.LookRotation(Player_forword);  //まずはプレーヤー→アンカー方向を正面とする
    }

    //プレイヤーの向き変更計算
    private void FixedUpdate()
    {
        Vector3 diff = transform.position - Player_pos; //プレイヤーがどの方向に進んでいるかがわかるように、初期位置と現在地の座標差分を取得
               
        if (diff.magnitude > 0.01f && diff.z > 0 ) //ベクトルの長さが0.01fより大きいかつ前方ない時にプレイヤーの向きを変える処理を入れる(0、バックでは入れないので）
        {
            if (diff.y != 0)
            {
                diff.y = 0;  //上下方向に移動がある場合そのY方向のベクトルを０にする
            }

            Quaternion targetRotation = Quaternion.LookRotation(diff,Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);  //ゆっくり回転するためにSlerpを使う
        }
        Player_pos = transform.position; //プレイヤーの位置を更新
    }

}