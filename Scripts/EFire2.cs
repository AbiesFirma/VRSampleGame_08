using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// HPを持つ敵の弾丸用
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class EFire2 : MonoBehaviour {

    [SerializeField] float speed = 5.0f;
    [SerializeField] ParticleSystem hitParticlePrefab;
    [SerializeField] int damage = 1;
    public Transform Camera;

    void Start()
    {
        var velocity = speed * transform.forward;
        var rb = GetComponent<Rigidbody>();
        rb.AddForce(velocity, ForceMode.VelocityChange);
        Camera = GameObject.Find("OVRCameraRig").GetComponent<Transform>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            other.SendMessage("OnHitEnemyBullet", damage, SendMessageOptions.RequireReceiver);

            //演出自動再生のオブジェクトを生成
            Instantiate(hitParticlePrefab, Camera.transform.position, Camera.transform.rotation);
        }

        if (other.tag != "Bullet")
        {
            //演出自動再生のオブジェクトを生成
            Instantiate(hitParticlePrefab, Camera.transform.position, Camera.transform.rotation);
            Destroy(gameObject, 0.1f);
        }
    }
}
