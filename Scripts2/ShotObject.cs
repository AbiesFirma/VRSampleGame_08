using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ちょっとした演出。生成され少しはねてすぐ消える。のクラス
/// </summary>
/// 
[RequireComponent(typeof(Rigidbody))]
public class ShotObject : MonoBehaviour {

    [SerializeField] float speed = 0.5f;
    [SerializeField] float lifetime = 0.5f;

	void Start () {
        var velocity = speed * transform.up;

        var rb = GetComponent<Rigidbody>();

        rb.AddForce(velocity, ForceMode.VelocityChange);

        Destroy(gameObject, lifetime);
    }
	
	
	void Update () {
		
	}
}
