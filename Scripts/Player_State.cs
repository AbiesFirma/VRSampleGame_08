using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの状態をコントロールするクラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Player_State : MonoBehaviour {

    [SerializeField] float PlayerMAXHP = 100.0f;
    float currentHP;
    [SerializeField] GameObject _HPSlider;
    Slider HpSlider;
    [SerializeField] Text HPValue;    
    Score score;

    [SerializeField] AudioClip hitClip;
    AudioSource audioSource;
    [SerializeField] GameObject Timer;

    Rigidbody rb;
        
    
    void Start () {
        var gameObj = GameObject.FindWithTag("Score");
        score = gameObj.GetComponent<Score>();
        audioSource = GetComponent<AudioSource>();

        if (_HPSlider == null)
        {
            _HPSlider = GameObject.Find("HPSlider");
        }
        HpSlider = _HPSlider.GetComponentInChildren<Slider>();

        //MaxHpがバーのMaxに
        HpSlider.maxValue = PlayerMAXHP;
        //HPMaxに
        HpSlider.value = PlayerMAXHP;
        currentHP = PlayerMAXHP;

        if (HPValue == null)
        {
            var _HPValue = GameObject.Find("HPValue");
            HPValue = _HPValue.GetComponent<Text>();
        }
        
        rb = GetComponent<Rigidbody>();
    }


    void Update () {
        HPValue.text = string.Format("{0}", currentHP);
    }

    void OnHitEnemyBullet(int damage)
    {
        //HItStopコルーチン実行
        StartCoroutine("HitStop", 0.3f);
        //着弾音
        audioSource.PlayOneShot(hitClip);
        //hp-
        currentHP -= damage;
        //Debug.Log(currentHP);
        //スライダーも減少
        HpSlider.value -= damage;
        //数字も更新
        HPValue.text = string.Format("{0}", currentHP);
        //スコア減算
        score.AddScore(-damage);

        //hp0で死亡
        if (currentHP <= 0)
        {
            GoDown();
        }

    }

    //死亡時タイムアップ
    void GoDown()
    {
        if (Timer == null)
        {
            Timer = GameObject.Find("Timer");
        }
        Timer.SendMessage("GoDown");
    }

    
    //呼び出し用ヒットストップのコルーチン
    public IEnumerator HitStop(float time)
    {
        var cur_vel = rb.velocity;
        //遅く
        rb.velocity *= 0.1f;
        //time分の時間まつ
        yield return new WaitForSeconds(time);
        //スピードを元に戻す        
        rb.velocity = cur_vel;
    }
}
