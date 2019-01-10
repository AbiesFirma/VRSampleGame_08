using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

/// <summary>
/// 剣による攻撃を行うためのクラス
/// </summary>
public class Sword : MonoBehaviour {

    [SerializeField] AudioClip spawnClip;     //出現音
    [SerializeField] AudioClip hitClip;       //ヒット音
    [SerializeField] AudioClip GoDownClip;    //死亡音
    [SerializeField] GameObject popupTextPrefab;   //ポイント表示をポップアップするプレハブ
    [SerializeField] GameObject HP;                //HP表示のためのオブジェクト

    [SerializeField] ParticleSystem hitParticlePrefab;
    bool Attack;
    [SerializeField] int damage = 1;
   

    // Use this for initialization
    void Start()
    {
        Attack = true;
    }

    void Update()
    {
       
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (Attack)
        {
            if (other.tag == "enemy")
            {
                other.SendMessage("OnHitBullet", damage, SendMessageOptions.RequireReceiver);
            }
            //着弾地点に演出自動再生のオブジェクトを生成
            Instantiate(hitParticlePrefab, transform.position, transform.rotation);
            Attack = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Attack = true;    
    }

}
