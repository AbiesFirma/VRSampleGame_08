using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 戦闘時の敵キャラクターを管理するクラス
/// </summary>

public class BattleEnemyState : MonoBehaviour {

    [SerializeField] float dif = 2.0f;     //防御力

    [SerializeField] float enemyMAXHP = 100.0f;       //最大HP
    public float currentHP { get; private set; }      //現在のHP
    [SerializeField] GameObject _HPSlider;            //頭上のHPスライダー
    Slider HpSlider;
    [SerializeField] Text HPValue;      //HP数値
    GameObject[] characters;         //キャラクターを格納する配列
    string enemyName;               // 敵の名前
    public float charaHeight = 2.0f;    //敵キャラの高さ
    Vector3 popupPos;                 //ダメージポップアップの位置

    AudioSource audioSource;
    [SerializeField] AudioClip[] se;   //[0]:被弾音

    Collider col;     //自身のコライダ

    [SerializeField] GameObject enemyDamageTextPrefab;      //ダメージ表示のための数字オブジェクト
    int _damage;                                           //ダメージ数値

    public bool bombHit { get; private set;}               //ボムがヒットしたときの連続ヒットを避けるbool
    float bombTime = 0.0f;                                 　　　

    void Start()
    {
        enemyName = gameObject.name;

        col = GetComponent<Collider>();
        //ren = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        HpSlider = _HPSlider.GetComponent<Slider>();

        //MaxHpがバーのMaxに
        HpSlider.maxValue = enemyMAXHP;
        //HPMaxに
        HpSlider.value = enemyMAXHP;
        currentHP = enemyMAXHP;

        bombHit = false;
    }

    void Update () {
        //ボムヒット時の連続ヒットを避ける処理
        if (bombHit)
        {
            bombTime += Time.deltaTime;
        }
        if(bombTime > 1.0f)
        {
            bombHit = false;
            bombTime = 0.0f;
        }
        popupPos = new Vector3(transform.position.x, transform.position.y + charaHeight, transform.position.z);
    }

    void OnHitAttack(int damage)
    {
        //着弾音
        audioSource.PlayOneShot(se[0]);

        //ダメージ計算(キャラ攻撃力*スキル攻撃力＝damage)
        //最終ダメージ_damage = (damage * ランダムに1.8～2.2)-(dif * ランダムに1.8～2.2) (考察中)
        var indexA = Random.Range(3.8f, 4.2f);
        var indexB = Random.Range(1.8f, 2.2f);
        var f_damage = (damage * indexA) - (dif * indexB);
        _damage = Mathf.RoundToInt(f_damage);
        if(_damage <= 0)
        {
            _damage = 1;
        }

        //hp-
        currentHP -= _damage;

        //スライダーも減少
        HpSlider.value -= _damage;
        

        //ダメージをポップアップ        
        EnemyDamagePopup();

        //hp0で死亡
        if (currentHP <= 0)
        {
            EnemyDead();
        }
    }
    void OnBombHit()
    {
        bombHit = true;
    }

    //死亡時
    void EnemyDead()
    {
        
        col.enabled = false;

        characters = GameObject.FindGameObjectsWithTag("OrderCharactor");

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SendMessage("EnemyDead", this.gameObject, SendMessageOptions.RequireReceiver);
        }

        var battleController = GameObject.Find("BattleController");
        battleController.SendMessage("EnemyDead", this.gameObject, SendMessageOptions.RequireReceiver);

        Destroy(gameObject, 1.5f);
        //ren.enabled = false;
    }

    //ダメージポップアップ
    void EnemyDamagePopup()
    {
        //var popupPos = new Vector3(transform.position.x, transform.position.y + charaHeight + 1.0f, transform.position.z);
        var damageText = Instantiate(enemyDamageTextPrefab, popupPos, transform.rotation);
       
        
        damageText.GetComponent<TextMeshPro>().text = string.Format("{0}", _damage);
    }
}
