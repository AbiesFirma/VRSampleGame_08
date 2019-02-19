using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 戦闘時の味方キャラクターの状態を管理するクラス
/// </summary>
public class BattleCharacterState : MonoBehaviour {

    [SerializeField] string charaRootName;   //キャラクター自身のステータスを管理するRootオブジェクト
    GameObject charaRoot;
    float dif;       //ダメージ計算のためのキャラ防御力    

    public float maxhp { get; private set; }         //最大HP
    public float currentHP { get; private set; }     //現在のHP
    public float charaHeight { get; private set; }    //キャラの高さ（身長＋アルファ、マーカー表示位置）
    public string charaName { get; private set; }    //キャラの名前
    public float ap { get; private set; }

    [SerializeField] GameObject _HPSlider;      //頭上HPのゲージ
    Slider HpSlider;
    //[SerializeField] Text HPValue;
    
    public GameObject gazeMenu;          //頭上のスキルなどが表示されるGazeのメニュー

    GameObject[] enemys;            //敵を格納する配列

    Collider col;             //自身のコライダー
    GameObject playerRoot;     //プレーヤー

    AudioSource audioSource;
    [SerializeField] AudioClip[] se; //[0]:被弾音

    [SerializeField] GameObject damageTextPrefab;    //ダメージを表示する数字オブジェクト
    int _damage;                       //ダメージ数値
    Vector3 popupPos;                  //表示位置
    [SerializeField] GameObject healTextPrefab;       //回復の数字オブジェクト
    [SerializeField] GameObject healEffect;          //回復エフェクト

    GameObject battleController;



    private void Awake()
    {
        //DontDestroyのキャラステータスから各情報をとってくる
        charaRoot = GameObject.Find(charaRootName);

        maxhp = charaRoot.GetComponent<CharaStatus>().maxHP;
        currentHP = charaRoot.GetComponent<CharaStatus>().currentHp;
        charaHeight = charaRoot.GetComponent<CharaStatus>().charaHeight;
        charaName = charaRoot.GetComponent<CharaStatus>().charaName;
        dif = charaRoot.GetComponent<CharaStatus>().dif;
        ap = charaRoot.GetComponent<CharaStatus>().ap;

    }

    void Start () {
        
        playerRoot = GameObject.FindWithTag("playerRoot");

        battleController = GameObject.Find("BattleController");

        audioSource = GetComponent<AudioSource>();

        col = GetComponent<Collider>();

        HpSlider = _HPSlider.GetComponent<Slider>();
        
        //MaxHpがバーのMaxに
        HpSlider.maxValue = maxhp;
        //HPをセット
        HpSlider.value = currentHP;
        _damage = 0;
    }
	
	
	void Update () {
        popupPos = new Vector3(transform.position.x, transform.position.y + charaHeight, transform.position.z);

        if(currentHP > maxhp)
        {
            currentHP = maxhp;
        }
        HpSlider.value = currentHP;
    }

    //敵の攻撃がヒットしたときEnemyAttackShereから送られてくるSendMessage
    void OnHitEnemyAttack(int damage)
    {
        //着弾音
        audioSource.PlayOneShot(se[0]);

        //ダメージ計算(キャラ攻撃力*スキル攻撃力＝damage)
        //最終ダメージ_damage = (damage * ランダムに1.8～2.2)-(dif * ランダムに1.8～2.2) (考察中)
        var indexA = Random.Range(3.8f, 4.2f);
        var indexB = Random.Range(1.8f, 2.2f);
        var f_damage = (damage * indexA) - (dif * indexB);
        _damage = Mathf.RoundToInt(f_damage);
        if (_damage <= 0)
        {
            _damage = 1;
        }

        //hp-
        currentHP -= _damage;
        
        //スライダーも減少
        //HpSlider.value -= _damage;
        
        //ダメージをポップアップ
        DamagePopup();

        //apを加算
        battleController.GetComponent<BattleAPController>().allAp += 1.0f;

        //hp0で死亡
        if (currentHP <= 0)
        {
            Dead();
        }

       
    }
    
    //自身の死亡時
    void Dead()
    {
        currentHP = 0;
        //コライダを無効に
        col.enabled = false;

        //敵全体に自分をターゲットから外すためのメッセージ
        enemys = GameObject.FindGameObjectsWithTag("enemy");

        //各敵にメッセージ
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].SendMessage("CharacterDead", this.gameObject, SendMessageOptions.RequireReceiver);
        }

        //バトルコントローラー,プレーヤーにも
        battleController.SendMessage("CharacterDead", this.gameObject, SendMessageOptions.RequireReceiver);

        if(playerRoot == null)
        {
            playerRoot = GameObject.Find("PlayerRoot");
        }
        playerRoot.SendMessage("CharacterDead", this.gameObject, SendMessageOptions.RequireReceiver);
    }

    //ダメージをポップアップし数値を更新
    void DamagePopup()
    {
        var damageText = Instantiate(damageTextPrefab, popupPos, transform.rotation);
        
        damageText.GetComponent<TextMeshPro>().text = string.Format("{0}", _damage);
    }

    //BattleCharacterControllerで実行。勝利時のHPを書き込み
    public void BattleWin()
    {
        charaRoot.GetComponent<CharaStatus>().currentHp = currentHP;
    }

    //アイテム使用時Itemコンポーネントから送られてくるSendMessage
    void UseItem(ItemData itemData)
    {
        var _itemData = itemData;
        Instantiate(healEffect, transform.position, Quaternion.identity);
        int effect = _itemData.healPower;
        var healPopPos = popupPos - new Vector3(0, 1.0f, 0);
        var healText = Instantiate(healTextPrefab, healPopPos, transform.rotation);

        healText.GetComponent<TextMeshPro>().text = string.Format("{0}", effect);

        currentHP += effect;
        if (currentHP > maxhp)
        {
            currentHP = maxhp;
        }
    }
}
