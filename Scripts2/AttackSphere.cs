using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラが攻撃する際にのアニメーションで発射される玉のクラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class AttackSphere : MonoBehaviour {

    Rigidbody rb;
    Vector3 currentVel;
    public GameObject character = null; //発動時キャラから上書きされる
    [SerializeField] ParticleSystem hitParticlePrefab;
    [SerializeField] GameObject hitEffect;

    [SerializeField] int skillPower = 1;   //スキルの攻撃力
    [SerializeField] float speed = 1.0f;   //玉が飛ぶスピード
    float damage;
    public float charaAttackPower = 1.0f;   //発動時キャラから上書きされる

    [SerializeField] bool hitDestroy = true;   //ヒットしたら消えるかどうか（貫通ならfalse）
    [SerializeField] bool Gun = false;    //小さい球を飛ばす銃撃などは敵の高さにより正面に飛ばすだけだと当たらないので生成位置と敵の位置を合わせて飛ばす
                                          //パンチキックなどは大きいのを前に飛ばすのでfalse  
    [SerializeField] bool skillBullet = false;      //スキルでターゲットを決めるときtrue,通常攻撃はfalse
    [SerializeField] bool hitAddForce = false;     //ヒットした敵に力を加える
    [SerializeField] float addPower = 1.0f;       //その時の力係数
    Vector3 velocity;
    AudioSource audioSource;
    Vector3 target;

    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("playerRoot");
        if (Gun)
        {
            if (skillBullet)
            {
                target = player.GetComponent<CharacterMoveOrder>().setSkillPos;
            }
            else
            {
                target = character.GetComponent<BattleCharacterController>().targetEnemy.transform.position;
            }
            var dir = target - transform.position;

            velocity = speed * dir.normalized;
        }
        else
        {
            velocity = speed * transform.forward;
        }

        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        rb.AddForce(velocity, ForceMode.VelocityChange);
    }

    void Update()
    {
        currentVel = rb.velocity;
    }

    //ヒットしたときの処理、敵ならSendmessage
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            damage = skillPower * charaAttackPower;

            if (!hitDestroy)
            {
                var bombHit = other.GetComponent<BattleEnemyState>().bombHit;

                if (!bombHit)
                {
                    audioSource.Play();
                    other.SendMessage("OnHitAttack", damage, SendMessageOptions.RequireReceiver);
                    other.SendMessage("OnBombHit");
                    //着弾地点に演出自動再生のオブジェクトを生成
                    Instantiate(hitParticlePrefab, transform.position, transform.rotation);
                    Instantiate(hitEffect, transform.position, transform.rotation);
                    
                }
            }
            else
            {
                audioSource.Play();
                other.SendMessage("OnHitAttack", damage, SendMessageOptions.RequireReceiver);
                //着弾地点に演出自動再生のオブジェクトを生成
                Instantiate(hitParticlePrefab, transform.position, transform.rotation);
                Instantiate(hitEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }

        }

        
    }



    //ヒットしたときの処理、敵ならSendmessage(Collision)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            damage = skillPower * charaAttackPower;

            if (!hitDestroy)
            {
                var bombHit = collision.gameObject.GetComponent<BattleEnemyState>().bombHit;

                if (!bombHit)
                {
                    audioSource.Play();
                    collision.gameObject.SendMessage("OnHitAttack", damage, SendMessageOptions.RequireReceiver);
                    collision.gameObject.SendMessage("OnBombHit");
                    //着弾地点に演出自動再生のオブジェクトを生成
                    Instantiate(hitParticlePrefab, transform.position, transform.rotation);
                    Instantiate(hitEffect, transform.position, transform.rotation);
                }

            }
            else
            {
                audioSource.Play();
                collision.gameObject.SendMessage("OnHitAttack", damage, SendMessageOptions.RequireReceiver);
                //着弾地点に演出自動再生のオブジェクトを生成
                Instantiate(hitParticlePrefab, transform.position, transform.rotation);
                Instantiate(hitEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }

            if(hitAddForce)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(currentVel * addPower);
            }

        }


    }
}