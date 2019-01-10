using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵の攻撃に関する処理を管理するクラス,
/// アニメーションイベントによって攻撃を行う
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Attack : MoveAgent {      //ナビメッシュのMoveAgentを継承

    [SerializeField] GameObject _player;
    Vector3 Player_pos;
    [SerializeField] float AttackRange = 6.0f;  //攻撃距離
    [SerializeField] float AttackMode = 20.0f;    //索敵距離

    Animator animator;
    [SerializeField] GameObject EFirePrefab;
    [SerializeField] GameObject EFirePoint;
    float currentSpeed;
    Rigidbody rb;

    float attackTime;    //攻撃してからの時間
    [SerializeField] float Can_attack = 8.0f;    //再攻撃可能時間

    enum EnemyState
    {
        Walk,
        Chase,
        Attack,
        Freeze
    };

    EnemyState state;


    void Start () {
        if (_player == null)
        {
            _player =GameObject.FindWithTag("player");
        }
        if (EFirePoint == null)
        {
            EFirePoint = GameObject.Find("EFirePoint");
        }

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentSpeed = agent.speed;
        attackTime = 5.0f;
    }

    void Update() {
        
        //アニメーション設定（下半身）少しでも動いてたら////////////////
        var velocity = rb.velocity.magnitude;
        
        if (velocity != 0.0f)
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }

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
            ////前回の攻撃（orゲームスタート）直後
            else if (attackTime < 2.0f)
            {
                state = EnemyState.Freeze;
            }
            //前回の攻撃（orゲームスタート）から十分な時間経過していない
            else
            {
                state = EnemyState.Walk;
            }
        }                

        //索敵範囲内,攻撃範囲外にプレーヤーがいる
        if (current_dis < AttackMode * AttackMode && current_dis > AttackRange * AttackRange)
        {
            //アタックタイムが経過している
            if (attackTime > Can_attack)
            {
                state = EnemyState.Chase;
            }
            //前回の攻撃（orゲームスタート）から十分な時間経過していない
            else
            {
                state = EnemyState.Walk;
            }
        }
        
        //十分に離れている
        if(current_dis > AttackMode * AttackMode)
        {
            state = EnemyState.Walk;
        }


        //stateによる行動変化////////////////////////////////
        switch (state)
        {
            case EnemyState.Walk:
                //MoveAgentにお任せでランダム移動,nextPointにプレーヤーが設定されていたら別のランダムポイントへ
                agent.speed = currentSpeed;
                animator.speed = 1.0f;
                animator.SetBool("EnemyAttack", false);
                if (nextPoint == Player_pos)
                {
                    GotoNextPoint();
                }
                break;

            case EnemyState.Chase:
                agent.speed = currentSpeed;
                animator.speed = 1.0f;
                //プレイヤーを追いかける
                GotoPlayer();
                break;

            case EnemyState.Attack:
                //立ち止まって攻撃
                agent.speed = 0.0f;
                animator.speed = 1.0f;
                var LookPos = new Vector3(Player_pos.x, transform.position.y, Player_pos.z);
                transform.LookAt(LookPos);
                //アニメーションイベントEnemyAttack
                animator.SetBool("EnemyAttack", true);
                attackTime = 0.0f;
                break;

            case EnemyState.Freeze:
                var _LookPos = new Vector3(Player_pos.x, transform.position.y, Player_pos.z);
                transform.LookAt(_LookPos);
                agent.speed = 0.0f;
                animator.speed = 0.5f;
                break;
        }
    }

    //プレイヤーへ向かう
    void GotoPlayer()
    {
        var nextPoint = new Vector3(Player_pos.x, Player_pos.y, Player_pos.z);
        agent.SetDestination(nextPoint);
    }


    //アニメーションイベント攻撃
    public void EnemyAttack()
    {     
        //プレハブから球を作成
        Instantiate(EFirePrefab, EFirePoint.transform.position, EFirePoint.transform.rotation);
        animator.SetBool("EnemyAttack", false);
    }
    
}
