using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// 敵の行動、状態のクラス
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{

    [SerializeField] AudioClip spawnClip;     //出現音
    [SerializeField] AudioClip hitClip;       //ヒット音
    [SerializeField] AudioClip GoDownClip;    //死亡音

    [SerializeField] Collider enemyCollider;   //コライダ
    [SerializeField] Renderer enemyRenderer;   //レンダー

    AudioSource audioSource;
    NavMeshAgent navmeshAgent;
    GameObject Timer;
    float currentTimer;

    float nav_speed;        //ナビメッシュの現在のスピードを保持する用

    Animator animator;

    [SerializeField] int point = 1;   //撃破時のポイント
    Score score;
    [SerializeField] int hp = 1;      //敵の最大HP

    [SerializeField] GameObject popupTextPrefab;     //得点をポップアップするためのプレハブ

    [SerializeField] GameObject HP;        //頭上のHP表示のためのオブジェクト
    Slider HPSlider;

    //敵の種類
    enum EnemyType
    {
        move,
        cannotmove
    }
    EnemyType enemytype;  


    void Start()
    {
        //ナビメッシュエージェント取得、現在（元の）スピードを保持
        //取得できなi場合（コンポーネントがAddされてない＝移動しない敵）とできた場合で敵の種類を定義
        navmeshAgent = GetComponent<NavMeshAgent>();
        if (navmeshAgent == null)
        {
            enemytype = EnemyType.cannotmove;
            //Debug.Log("cannotM");
        }
        else
        {
            enemytype = EnemyType.move;
            nav_speed = navmeshAgent.speed;
        }

        //アニメーター、オーディオソース取得
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        //出現音
        audioSource.PlayOneShot(spawnClip);

        var gameObj = GameObject.FindWithTag("Score");

        score = gameObj.GetComponent<Score>();

        HPSlider = HP.GetComponent<Slider>();

        //MaxHpがバーのMaxに
        HPSlider.maxValue = hp;
        //HPMaxに
        HPSlider.value = hp;
    }

    //SendMessageを受けとった時の被弾処理
    void OnHitBullet(int damage)
    {
        if (enemytype == EnemyType.move)
        {
            //HItStopコルーチン実行---0.25秒*2
            StartCoroutine("HitStop", 0.25f);
        }
        //着弾音
        audioSource.PlayOneShot(hitClip);
        //hp-damage
        hp -= damage;
        //スライダーも減少
        HPSlider.value -= damage; ;
        //hp0で死亡
        if (hp <= 0)
        {
            GoDown();
        }

    }

    //死亡時処理
    void GoDown()
    {
        //スコア加算
        score.AddScore(point);
        //popup作成
        CreatePopupText();
        //当たり判定と表示を消す（死亡音を再生するため）
        enemyCollider.enabled = false;
        enemyRenderer.enabled = false;
        //HPバーも消す
        HP.SetActive(false);
        audioSource.PlayOneShot(GoDownClip);

        //一定時間後に破壊
        Destroy(gameObject, 1f);
    }

    void CreatePopupText()
    {
        //ポップアップテキストのインスタンス作成
        var text = Instantiate(popupTextPrefab, transform.position, Quaternion.identity);
        //テキスト変更
        text.GetComponent<TextMesh>().text = string.Format("+{0}", point);
    }


    //ヒットストップのコルーチン
    public IEnumerator HitStop(float time)
    {
        //アニメーター、ナビスピードを低くする
        animator.SetBool("EnemyHit", true);
        //time分の時間まつ
        yield return new WaitForSeconds(time);
        animator.speed = 0.1f;
        navmeshAgent.speed = 0.1f;
        //time分の時間まつ
        yield return new WaitForSeconds(time);
        //スピードを元に戻す        
        animator.speed = 1.0f;
        navmeshAgent.speed = nav_speed;
        animator.SetBool("EnemyHit", false);
    }

    //SendMessageを受け取ってタイムアップで破壊
    void Timeup()
    {
        Destroy(gameObject);
    }

}
