using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の攻撃判定のための玉を管理するクラス
/// </summary>

public class EnemyAttackSphere : MonoBehaviour {

    [SerializeField] ParticleSystem hitParticlePrefab;
    [SerializeField] GameObject hitEffect;
    [SerializeField] int skillPower = 1;     //攻撃力
    [SerializeField] float speed = 1.0f;     //弾速
    float damage;
    public float enemyAttackPower = 1.0f;   //敵の攻撃力（敵側から上書きされる）


    void Start()
    {
        var velocity = speed * transform.forward;
        var rb = GetComponent<Rigidbody>();

        rb.AddForce(velocity, ForceMode.VelocityChange);
    }

    //ヒットしたときの処理、Sendmessage
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "OrderCharactor")
        {
            damage = skillPower * enemyAttackPower;
            other.SendMessage("OnHitEnemyAttack", damage, SendMessageOptions.RequireReceiver);
            //着弾地点に演出自動再生のオブジェクトを生成
            Instantiate(hitParticlePrefab, transform.position, transform.rotation);
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }


    }
}