using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_DontMove : MonoBehaviour {
    [SerializeField] AudioClip spawnClip;     //出現音
    [SerializeField] AudioClip hitClip;       //ヒット音
    [SerializeField] AudioClip GoDownClip;    //死亡音

    [SerializeField] Collider enemyCollider;   //コライダ
    [SerializeField] Renderer enemyRenderer;   //レンダー

    AudioSource audioSource;
    
    Animator animator;

    [SerializeField] int point = 1;
    Score score;

    [SerializeField] int hp = 1;

    [SerializeField] GameObject popupTextPrefab;

    [SerializeField] GameObject HP;
    Slider HPSlider;

    void Start()
    {        
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
        //着弾音
        audioSource.PlayOneShot(hitClip);

        //hpが０で死亡
        --hp;

        //スライダーも減少
        --HPSlider.value;

        if (hp <= 0)
        {
            //死亡
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
    
}