using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 戦闘時敵の行動管理クラス
/// </summary>

[RequireComponent(typeof(NavMeshAgent))]
public class BattleEnemyController : MonoBehaviour {

    NavMeshAgent navAgent;
    private Animator animator;
    float battleTime;              //戦闘時間
    bool ready;                    //準備状態 
    Vector3 nextPoint;            //移動時の次のポイント
    [SerializeField] List<GameObject> charasList;      //キャラを格納するリスト

    [SerializeField] float attackRange = 2.0f;         //攻撃射程距離
    [SerializeField] float attackCool = 3.0f;         //攻撃クールタイム
    [SerializeField] float attackPower = 2.0f;         //攻撃力
    float attackTime;                                 //攻撃間隔制御のための数値

    GameObject targetChara;                          //現在の標的キャラ
    float targetDis;                                 //ターゲットまでの距離

    enum EnemyState            //現在の状態
    {
        Run,     //移動
        Attack,  //攻撃
        Idle,    //待機
        Dead     //死亡
    };
    [SerializeField] EnemyState enemystate;
    float hp;         //現在のHP

    Vector3 from_pos;
    Vector3 to_pos;

    [SerializeField] Transform attackPoint;     //攻撃判定の発射位置
    [SerializeField] GameObject attackSpherePrefab;     //攻撃判定のための玉オブジェクトのプレハブ

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //キャラをタグで取得し配列にいれ、リスト化
        var chara = GameObject.FindGameObjectsWithTag("OrderCharactor");
        charasList = new List<GameObject>();
        charasList.AddRange(chara);

        from_pos = transform.position;
        to_pos = transform.position;

        //一旦ターゲットを決定
        targetChara = charasList[0];
        targetDis = Vector3.Distance(transform.position, targetChara.transform.position);
        attackTime = attackCool;

        //その方向を向く
        var look = new Vector3(targetChara.transform.position.x, transform.position.y, targetChara.transform.position.z);
        transform.LookAt(look);

        ready = true;
    }

    void Update()
    {
        battleTime += Time.deltaTime;
        if (battleTime >= 3.0f)
        {
            ready = false;
        }

        //状態判定
        if (ready)
        {
            enemystate = EnemyState.Idle;
        }
        else
        {

            hp = GetComponent<BattleEnemyState>().currentHP;
            if (hp <= 0.0f)
            {
                enemystate = EnemyState.Dead;
            }
            else
            {
                if (charasList.Count == 0)
                {
                    //戦闘終了
                    enemystate = EnemyState.Idle;
                    Debug.Log("LOSE");
                }
                else
                {

                    attackTime += Time.deltaTime;

                    to_pos = transform.position;
                    var move = to_pos - from_pos;

                    var nearestDis = 10000f;
                    for (int i = 0; i < charasList.Count; i++)
                    {
                        var _dis = Vector3.Distance(transform.position, charasList[i].transform.position);

                        if (_dis < nearestDis)
                        {
                            nearestDis = _dis;
                            targetChara = charasList[i];
                        }
                    }

                    targetDis = Vector3.Distance(transform.position, targetChara.transform.position);

                    //
                    if (move.magnitude > 0.01f)
                    {
                        animator.SetBool("Run", true);
                        
                    }
                    else
                    {
                        animator.SetBool("Run", false);
                    }


                    //ステイト設定
                    if (attackTime < attackCool)
                    {
                        enemystate = EnemyState.Idle;

                    }
                    else
                    {
                        if (targetDis < attackRange)
                        {
                            enemystate = EnemyState.Attack;
                        }
                        else
                        {
                            enemystate = EnemyState.Run;
                        }

                    }
                }
            }
        }

        //状態による行動変化
        switch (enemystate)
        {
            case EnemyState.Run:
                AutoRun();
                //Debug.Log("Run");
                break;

            case EnemyState.Attack:
                StartCoroutine("EnemyAttack");
                //Attack();
                //Debug.Log("Attack");
                break;

            case EnemyState.Idle:
                navAgent.SetDestination(transform.position);
                //Debug.Log("Idle");
                break;

            case EnemyState.Dead:
                navAgent.SetDestination(transform.position);
                //Debug.Log("Dead");
                animator.SetBool("dead", true);
                break;

        }

        from_pos = to_pos;

    }

    //攻撃がヒットしたときAttackShereから送られてくるSendMessage
    void OnHitAttack(int damage)
    {
        StartCoroutine("HitStop",0.0f);
    }

    //ヒットストップ
    IEnumerator HitStop(float time)
    {
        animator.SetBool("damage", true);
        yield return new WaitForSeconds(time);
        animator.SetBool("damage", false);
    }


    //自動でキャラへ移動
    void AutoRun()
    {        
        nextPoint = targetChara.transform.position;
        navAgent.SetDestination(nextPoint);
    }

    //攻撃コルーチン
    public IEnumerator EnemyAttack()
    {
        //Debug.Log("Attack");
        navAgent.SetDestination(transform.position);

        var look = new Vector3(targetChara.transform.position.x, transform.position.y, targetChara.transform.position.z);
        transform.LookAt(look);

        animator.SetBool("attack", true);
        attackTime = 0.0f;

        yield return new WaitForSeconds(1.0f);
        animator.SetBool("attack", false);
    }

    //キャラが死んだとき、BattleCharacterStateから送られてくるSendMessage
    void CharacterDead(GameObject chara)
    {
        charasList.Remove(chara);
        //Debug.Log(charasList.Count);
    }

    //AnimationEvent
    void Punch()
    {
        Instantiate(attackSpherePrefab, attackPoint.position, attackPoint.rotation);
        animator.SetBool("attack", false);
    }

    void Attack()
    {
        GameObject _attackShere = Instantiate(attackSpherePrefab, attackPoint.position, attackPoint.rotation);
        _attackShere.GetComponent<EnemyAttackSphere>().enemyAttackPower = attackPower;
        animator.SetBool("attack", false);
    }
}

