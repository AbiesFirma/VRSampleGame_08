using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// フィールドでの敵の移動を管理するクラス
/// </summary>

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyBattleFieldMoveAgent : MonoBehaviour {

    NavMeshAgent agent;                      //ナビメッシュエージェント
    [SerializeField] GameObject[] movePoint;   //移動先を設定する配列
    Vector3 nextPoint;
    Animator animator;
    Rigidbody rb;
    float currentSpeed;

    [SerializeField] float chaseRange = 10.0f;
    float idelTimer;

    GameObject player;

    enum EnemyState
    {
        Walk,
        Chase,
        Idle
    };
    EnemyState enemyState;
    public bool encount;

    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject encountEffect;

    void Start()
    {
        player = GameObject.Find("PlayerRoot");

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        nextPoint = transform.position;
        idelTimer = 0.0f;
        currentSpeed = agent.speed;     //agentSpeed初期値   
        encount = false;

        movePoint = GameObject.FindGameObjectsWithTag("FeildEnemyMovePoint");

        agent.SetDestination(transform.position);
        GotoNextPoint();
    }

    void Update()
    {
        //アニメーション設定少しでも動いてたら////////////////
        var velocity = rb.velocity.magnitude;

        if (velocity != 0.0f)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }


        //state設定。プレイヤーとの距離
        Vector3 playerPos = player.transform.position;
        var currentDis = (playerPos - transform.position).sqrMagnitude;

        if (encount)
        {
            enemyState = EnemyState.Idle;
            //audioSource.Play();
            Instantiate(encountEffect, transform.position, Quaternion.identity);
        }
        else
        {
            if (currentDis < chaseRange * chaseRange)
            {
                enemyState = EnemyState.Chase;
            }
            else
            {
                //目的地に近づいたら
                if (agent.remainingDistance < 1.0f && idelTimer < 1.0f)
                {
                    enemyState = EnemyState.Idle;
                    idelTimer += Time.deltaTime;
                }
                else if (agent.remainingDistance < 1.0f && idelTimer >= 1.0f)
                {
                    enemyState = EnemyState.Walk;
                    idelTimer = 0.0f;
                }
            }
        }
        //stateによる行動変化
        switch (enemyState)
        {
            case EnemyState.Chase:
                agent.speed = currentSpeed * 1.5f;
                agent.SetDestination(player.transform.position);
                break;

            case EnemyState.Walk:
                agent.speed = currentSpeed;
                GotoNextPoint();
                break;

            case EnemyState.Idle:
                agent.speed = currentSpeed;
                agent.SetDestination(transform.position);
                break;

        }
        
    }

    //次の目的地を候補からランダムに選択し設定する
    void GotoNextPoint()
    {
        if (agent.remainingDistance < 0.5f)
        {
            var movePoint_num = Random.Range(0, movePoint.Length);
            Vector3 nextPoint = movePoint[movePoint_num].transform.position;

            agent.SetDestination(nextPoint);
        }
    }

}
