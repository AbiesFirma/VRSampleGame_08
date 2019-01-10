using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 動かない敵の攻撃のクラス
/// 弾丸の発射などアニメーションとは関係なく攻撃を行う
/// </summary>
public class Enemy_Attack_DontMove : MonoBehaviour {


    [SerializeField] GameObject _player;
    Vector3 Player_pos;
    [SerializeField] float AttackRange = 10.0f;  //攻撃距離
    
    [SerializeField] GameObject[] EFirePrefab;  //攻撃の種類発射場所を複数セットできる
    [SerializeField] GameObject[] EFirePoint;
    
    float attackTime;    //攻撃してからの時間
    [SerializeField] float Can_attack = 8.0f;    //再攻撃可能時間

    enum EnemyState
    {
        Attack,
        Wait
    };

    EnemyState state;


    void Start()
    {
        if (_player == null)
        {
            _player = GameObject.FindWithTag("player");
        }
        
        attackTime = 0.0f;
    }

    void Update()
    {
        //state設定。プレイヤーとの距離と攻撃してからの時間///////////////
        attackTime += Time.deltaTime;

        Player_pos = _player.transform.position;
        var Enemy_pos = this.transform.position;
        var current_dis = (Player_pos - Enemy_pos).sqrMagnitude;


        //攻撃可能距離内にプレーヤーがいる
        if (current_dis < AttackRange * AttackRange)
        {
            //アタックタイムが経過している
            if (attackTime > Can_attack)
            {
                state = EnemyState.Attack;
            }
            //前回の攻撃（orゲームスタート）から十分な時間経過していない
            else
            {
                state = EnemyState.Wait;
            }
        }
        
        //十分に離れている
        if (current_dis > AttackRange * AttackRange)
        {
            state = EnemyState.Wait;
        }


        //stateによる行動変化////////////////////////////////
        switch(state)
        {
            case EnemyState.Attack:
                //transform.LookAt(Player_pos);
                EnemyAttack();
                attackTime = 0.0f;
                break;

            case EnemyState.Wait:
                break;

        }
        
    }
    
    
    //攻撃、発射位置と攻撃方法をランダムに選択
    public void EnemyAttack()
    {
        var FirePoint_num = Random.Range(0, EFirePoint.Length);
        var Fire_num = Random.Range(0, EFirePrefab.Length);
        //プレハブから球を作成
        Instantiate(EFirePrefab[Fire_num], EFirePoint[FirePoint_num].transform.position, EFirePoint[FirePoint_num].transform.rotation);
        
    }

}