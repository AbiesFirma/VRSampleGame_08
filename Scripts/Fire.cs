using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 火の玉のクラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Fire : MonoBehaviour {

    [SerializeField] float speed = 10.0f;
    [SerializeField] ParticleSystem hitParticlePrefab;
    [SerializeField] int damage = 1;

    void Start()
    {
        var velocity = speed * transform.forward;

        var rb = GetComponent<Rigidbody>();

        rb.AddForce(velocity, ForceMode.VelocityChange);
    }

    void OnTriggerEnter(Collider other)
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
