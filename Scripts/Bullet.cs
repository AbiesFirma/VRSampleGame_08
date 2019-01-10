using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

/// <summary>
/// 弾丸用クラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

    [SerializeField] float speed = 40.0f;
    [SerializeField] ParticleSystem hitParticlePrefab;
    [SerializeField] int damage = 1;
        
    //生成されたら正面に飛ぶ
    void Start () {
        var velocity = speed * transform.forward;

        var rb = GetComponent<Rigidbody>();

        rb.AddForce(velocity, ForceMode.VelocityChange);
	}
	
    //ヒットしたときの処理、敵ならSendmessage
	void OnTriggerEnter (Collider other)
    {
        if (other.tag == "enemy")
        {
            other.SendMessage("OnHitBullet", damage, SendMessageOptions.RequireReceiver);
        }

        //着弾地点に演出自動再生のオブジェクトを生成
        Instantiate(hitParticlePrefab, transform.position, transform.rotation);

        Destroy(gameObject);
	}
}
