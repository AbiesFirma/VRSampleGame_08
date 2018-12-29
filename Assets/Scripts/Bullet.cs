using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

    [SerializeField] float speed = 40.0f;
    [SerializeField] ParticleSystem hitParticlePrefab;

	// Use this for initialization
	void Start () {
        var velocity = speed * transform.forward;

        var rb = GetComponent<Rigidbody>();

        rb.AddForce(velocity, ForceMode.VelocityChange);
	}
	
	void OnTriggerEnter (Collider other)
    {
        other.SendMessage("OnHitBullet");

        //着弾地点に演出自動再生のオブジェクトを生成
        Instantiate(hitParticlePrefab, transform.position, transform.rotation);

        Destroy(gameObject);
	}
}
