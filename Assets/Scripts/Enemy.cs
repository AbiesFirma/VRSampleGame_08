using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour {

    [SerializeField] AudioClip spawnClip;     //出現音
    [SerializeField] AudioClip hitClip;       //ヒット音
    [SerializeField] AudioClip GoDownClip;    //死亡音

    [SerializeField] Collider enemyCollider;   //コライダ
    [SerializeField] Renderer enemyRenderer;   //レンダー

    AudioSource audioSource;      
    NavMeshAgent navmeshAgent;

    float nav_speed;        //ナビメッシュの現在のスピードを保持する用

    Animator animator;
    
    [SerializeField] int point = 1;
    Score score;

    [SerializeField] int hp = 1;

    [SerializeField] GameObject popupTextPrefab;

    [SerializeField] GameObject HP;
    Slider HPSlider;
    

    void Start()
    {
        //ナビメッシュエージェント取得、現在（元の）スピードを保持
        navmeshAgent = GetComponent<NavMeshAgent>();
        nav_speed = navmeshAgent.speed;

        //アニメーター、オーディオソース取得
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        //出現音
        audioSource.PlayOneShot(spawnClip);

        var gameObj = GameObject.FindWithTag("Score");

        score = gameObj.GetComponent<Score>();

        //HP = transform.Find("Slider").gameObject;
        HPSlider = HP.GetComponent<Slider>();

        //MaxHpがバーのMaxに
        HPSlider.maxValue = hp;
        //HPMaxに
        HPSlider.value = hp;
    }


    void OnHitBullet()
    {
        //HItStopコルーチン実行---0.25秒*2
        StartCoroutine("HitStop", 0.25f);
        
        //着弾音
        audioSource.PlayOneShot(hitClip);

        //hp-1
        --hp;

        //スライダーも減少
        --HPSlider.value; 

        //hp0で死亡
        if (hp <= 0)
        {
            GoDown();
        }

    }

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


    //呼び出し用ヒットストップのコルーチン
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
}
