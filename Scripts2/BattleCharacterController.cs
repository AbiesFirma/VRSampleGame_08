using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// 戦闘時の味方キャラクターの行動を管理するクラス
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class BattleCharacterController : MonoBehaviour {

    [SerializeField] string charaRootName;   //キャラクター自身のステータスを管理するRootオブジェクト
    GameObject charaRoot;

    GameObject character;           //キャラオブジェクト
    NavMeshAgent navAgent;
    private Animator animator;
    Vector3 nextPoint;             //次の移動先
    public bool ready { get; private set;}    //待機状態bool
    bool start;                               //バトルスタート時かどうか
    float battleTime;                        //戦闘開始からの時間

    GameObject battleController;

    [SerializeField] List<GameObject> enemysList;          //敵オブジェクトを格納するリスト

    GameObject player;
    bool order;              //プレーヤーからの命令時
    bool pubOrder;           //プレーヤーからの命令時（味方全体）
    float hp;                //現在のHP

    float attackRange;      //通常攻撃の射程距離
    float attackCool;       //通常攻撃のクールタイム
    float attackPower;      //通常攻撃の攻撃力
    float dex;              //器用さ（現状ではAP回収のみに影響）

    float attackTime;       //攻撃間隔計測のための時間

    public GameObject targetEnemy { get; private set; }    //現在標的にしている敵
    float targetDis;                                       //ターゲットまでの距離

    enum CharaState                 //キャラクターの現在の状態
    {
        Run,      //移動
        Attack,   //攻撃
        Order,    //プレーヤーからの命令
        Idle,     //待機
        Dead      //死亡
    };
    [SerializeField] CharaState charastate;

    Vector3 from_pos;
    Vector3 to_pos;

    [SerializeField] Transform attackPoint;              //攻撃判定発生位置
    [SerializeField] GameObject attackSpherePrefab;      //攻撃判定用の玉オブジェクトのプレハブ
    [SerializeField] GameObject battleTalkCanvasPrefab;  //スキル名などを表示するキャンバス
    [SerializeField] GameObject waitIcon;                //wait時に表示されるWaitの文字オブジェクト

    [SerializeField] GameObject skillParticlePrefab;     //スキル使用時のエフェクト
    string skillAnimeName;                               //スキルのアニメーション名
    AudioSource audioSource; 
    [SerializeField] AudioClip skillReadyAudio;          //スキル使用時のSE
    [SerializeField] AudioClip[] attackAudio = new AudioClip[3];  //0:kick, 1:Slash, 2:GunShot
    [SerializeField] AudioClip skillBulletAudio;


    void Start () {
        charaRoot = GameObject.Find(charaRootName);
        player = GameObject.FindWithTag("playerRoot");        
        character = this.gameObject;
        battleController = GameObject.Find("BattleController");

        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //敵をタグで検索し配列に入れ、それをリスト化
        var enemy = GameObject.FindGameObjectsWithTag("enemy");
        enemysList = new List<GameObject>();
        enemysList.AddRange(enemy);

        //DontDestroyのキャラステータスから各数値を取得
        attackRange = charaRoot.GetComponent<CharaStatus>().attackRange;
        attackCool = charaRoot.GetComponent<CharaStatus>().attackCool; ;
        attackPower = charaRoot.GetComponent<CharaStatus>().attackPower;
        dex = charaRoot.GetComponent<CharaStatus>().dex;

        order = false;

        from_pos = transform.position;
        to_pos = transform.position;

        //一旦敵をターゲット
        if (enemysList.Count != 0)
        {
            targetEnemy = enemysList[0];
            targetDis = Vector3.Distance(transform.position, targetEnemy.transform.position);
            attackTime = attackCool;
        }

        pubOrder = player.GetComponent<CharacterMoveOrder>().pubOrder;

        start = true;     //バトルスタートのための待機bool
        ready = true;     //硬直や指示などキャラを制止させるためのbool
        battleTime = 0.0f;

        audioSource = GetComponent<AudioSource>();
    }
		

	void Update () {
        //開始３秒まで待機しその後行動開始
        battleTime += Time.deltaTime;
        if (battleTime > 3.0f && start)
        {
            start = false;
            ready = false;
        }
        
        //プレイヤーからの全体命令
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) || Input.GetKeyDown(KeyCode.Space))
        {
            if (!pubOrder)
            {
                pubOrder = true;
            }
            else
            {
                pubOrder = false;
            }
        }

        //移動してるかどうかの判定のための計算
        to_pos = transform.position;
        var move = to_pos - from_pos;

        //待機戦闘前等の状態（オートバトルはせず指示には従う）
        if (ready)
        {
            charastate = CharaState.Idle;
            //Runアニメーション設定
            if (move.magnitude > 0.001f)
            {
                animator.SetBool("Run", true);
            }
            else
            {
                animator.SetBool("Run", false);
            }
        }

        //オートバトル
        else
        {
            hp = GetComponent<BattleCharacterState>().currentHP;
            if (hp <= 0)
            {
                charastate = CharaState.Dead;
            }
            
            else
            {

                if (enemysList.Count == 0)
                {
                    //戦闘終了
                    charastate = CharaState.Idle;
                    GetComponent<BattleCharacterState>().BattleWin();
                }
                else
                {

                    order = player.GetComponent<CharacterMoveOrder>().order;
                    attackTime += Time.deltaTime;

                    //to_pos = transform.position;
                    //var move = to_pos - from_pos;

                    //ターゲットを決めるためすべての敵との距離を計測し最近の敵を見つける
                    var nearestDis = 10000f;
                    for (int i = 0; i < enemysList.Count; i++)
                    {
                        var _dis = Vector3.Distance(transform.position, enemysList[i].transform.position);

                        if (_dis < nearestDis)
                        {
                            nearestDis = _dis;
                            targetEnemy = enemysList[i];
                        }
                    }

                    targetDis = Vector3.Distance(transform.position, targetEnemy.transform.position);

                    //Runアニメーション設定
                    if (move.magnitude > 0.001f)
                    {
                        animator.SetBool("Run", true);
                    }
                    else
                    {
                        animator.SetBool("Run", false);
                    }


                    //ステイト設定
                    if (pubOrder && order)
                    {
                        charastate = CharaState.Order;
                    }
                    else
                    {
                        if (attackTime < attackCool && !order)
                        {
                            if (targetDis < attackRange)
                            {
                                charastate = CharaState.Idle;
                            }
                            else
                            {
                                charastate = CharaState.Run;
                            }
                        }
                        else if (attackTime < attackCool && order)
                        {
                            charastate = CharaState.Order;
                        }
                        else
                        {
                            if (order)
                            {
                                charastate = CharaState.Order;
                            }
                            else
                            {
                                //_dis = Vector3.Distance(transform.position, targetEnemy.transform.position);
                                if (targetDis < attackRange)
                                {
                                    charastate = CharaState.Attack;
                                }
                                else
                                {
                                    charastate = CharaState.Run;
                                }
                            }
                        }
                    }
                }
            }
        }

            //
            switch (charastate)
            {
                case CharaState.Order:
                    //Debug.Log("Order");
                    break;

                case CharaState.Run:
                    AutoRun();
                    break;

                case CharaState.Attack:
                    StartCoroutine("cAttack");
                    //Debug.Log("Attack");
                    break;

                case CharaState.Idle:
                    navAgent.SetDestination(transform.position);
                    break;

                case CharaState.Dead:
                    navAgent.SetDestination(transform.position);
                    animator.SetBool("dead", true);
                    break;

            }

        from_pos = to_pos;


        //Ready状態ではWaitの文字アイコンをだす
        if(ready)
        {
            waitIcon.SetActive(true);
        }
        else
        {
            waitIcon.SetActive(false);
        }

    }

    //敵の攻撃がヒットしたときEnemyAttackShereから送られてくるSendMessage
    void OnHitEnemyAttack(int damage)
    {
        if (!ready)
        {
            StartCoroutine("HitStop", 0.0f);
        }
    }

    //ヒットストップのコルーチン
    IEnumerator HitStop(float time)
    {
        animator.SetBool("damage", true);
        yield return new WaitForSeconds(time);
        animator.SetBool("damage", false);
    }

    //オートでターゲットへ向かう
    void AutoRun()
    {
        nextPoint = targetEnemy.transform.position;
        navAgent.SetDestination(nextPoint);
    }

    //キャラクター攻撃行動コルーチン
    public IEnumerator cAttack()
    {
        navAgent.SetDestination(transform.position);
        var look = new Vector3(targetEnemy.transform.position.x, transform.position.y, targetEnemy.transform.position.z);
        transform.LookAt(look);
        animator.SetBool("attack", true);
        attackTime = 0.0f;
        
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("attack", false);
    }

    //敵が死んだとき、BattleEnemyStateから送られてくるSendMessage
    void EnemyDead(GameObject enemy)
    {
        enemysList.Remove(enemy);       
    }

    //各animationEvent
    void Kick()
    {
        audioSource.PlayOneShot(attackAudio[0]);
        battleController.GetComponent<BattleAPController>().allAp += 1.0f * dex;   //Apを加算
        GameObject _attackShere = Instantiate(attackSpherePrefab, attackPoint.position, attackPoint.rotation);
        _attackShere.GetComponent<AttackSphere>().charaAttackPower = attackPower;
        _attackShere.GetComponent<AttackSphere>().character = character;
        animator.SetBool("attack", false);
    }
    void Slash()
    {
        audioSource.PlayOneShot(attackAudio[1]);
        battleController.GetComponent<BattleAPController>().allAp += 1.0f * dex;
        GameObject _attackShere = Instantiate(attackSpherePrefab, attackPoint.position, attackPoint.rotation);
        _attackShere.GetComponent<AttackSphere>().charaAttackPower = attackPower;
        _attackShere.GetComponent<AttackSphere>().character = character;
        animator.SetBool("attack", false);
    }
    void GunShot()
    {
        audioSource.PlayOneShot(attackAudio[2]);
        battleController.GetComponent<BattleAPController>().allAp += 1.0f * dex;
        GameObject _attackShere = Instantiate(attackSpherePrefab, attackPoint.position, attackPoint.rotation);
        _attackShere.GetComponent<AttackSphere>().charaAttackPower = attackPower;
        _attackShere.GetComponent<AttackSphere>().character = character;
        animator.SetBool("attack", false);
    }


    //スキル発動時CharacterMoveOrderからのSendMessage
    void SetSkill(GameObject skillObj)
    {
        //オートバトルを一時停止
        ready = true;
        //スキルオブジェクトからスキル情報を取得
        skillAnimeName = skillObj.GetComponent<SkillSphere>()._animeName;
        var skillName = skillObj.GetComponent<SkillSphere>()._skillName;
        var useAp = skillObj.GetComponent<SkillSphere>()._useAp;
        battleController.GetComponent<BattleAPController>().allAp -= useAp;

        var height = charaRoot.GetComponent<CharaStatus>().charaHeight;

        //スキル名をポップアップ
        var talk = Instantiate(battleTalkCanvasPrefab, transform.position, transform.rotation);
        talk.transform.position = new Vector3(talk.transform.position.x, talk.transform.position.y + height, talk.transform.position.z);
        var text = talk.transform.Find("Text").gameObject.GetComponent<Text>();
        
        text.text = string.Format("{0}", skillName);
        
        var skillAnimeTime = skillObj.GetComponent<SkillSphere>()._animeTime;

        //スキル発動コルーチンスタート
        StartCoroutine("SetSkillCor", skillAnimeTime);
    }
    //スキルコルーチン
    IEnumerator SetSkillCor(float time)
    {
        //スキルをセットした位置を取得
        var setPos = player.GetComponent<CharacterMoveOrder>().setSkillPos;
        
        //移動を止め発動方向に向く
        navAgent.SetDestination(transform.position);
        var look = new Vector3(setPos.x, transform.position.y, setPos.z);
        transform.LookAt(look);

        //発動演出
        audioSource.PlayOneShot(skillReadyAudio);
        Instantiate(skillParticlePrefab, transform.position, transform.rotation);
                
        yield return new WaitForSeconds(2.0f);       

        //アニメーション開始（アニメーションイベントにて射出）
        animator.SetBool(skillAnimeName, true);
        //Instantiate(skillCoolParticlePrefab, transform.position, transform.rotation);
        yield return new WaitForSeconds(time);        

        animator.SetBool(skillAnimeName, false);
        ready = false;
    }



    //Bombアニメーションイベント、引数でボムオブジェクトを受け取り投げる
    void Bomb(GameObject bombPrefab)
    {
        //var target = player.GetComponent<CharacterMoveOrder>().setSkillPos;
        GameObject _bomb = Instantiate(bombPrefab, attackPoint.position, attackPoint.rotation);
        _bomb.GetComponent<ThrowObject>().target = player.GetComponent<CharacterMoveOrder>().setSkillPos;
        _bomb.GetComponent<ThrowObject>().targetSet = true;
        _bomb.GetComponent<ThrowObject>().charaAttackPower = attackPower;

        animator.SetBool(skillAnimeName, false);
    }
    //SkillBullet
    void SkillBulletShot(GameObject SkillBulletPrefab)
    {
        audioSource.PlayOneShot(skillBulletAudio);
        GameObject _bullet = Instantiate(SkillBulletPrefab, attackPoint.position, attackPoint.rotation);
        _bullet.GetComponent<AttackSphere>().charaAttackPower = attackPower;
        _bullet.GetComponent<AttackSphere>().character = character;

        animator.SetBool(skillAnimeName, false);
    }
    //ShockWave
    void ShockWave(GameObject ShockWavePrefab)
    {
        //audioSource.PlayOneShot(skillBulletAudio);
        GameObject _shock = Instantiate(ShockWavePrefab, attackPoint.position, attackPoint.rotation);
        _shock.GetComponent<AttackSphere>().charaAttackPower = attackPower;
        _shock.GetComponent<AttackSphere>().character = character;

        animator.SetBool(skillAnimeName, false);
    }


    //任意でキャラを止める
    public void Wait()
    {
        if (!ready)
        {
            ready = true;
        }
        else
        {
            ready = false;
        }
    }
}
