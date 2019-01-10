using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵の攻撃用弾丸のクラス
[RequireComponent(typeof(Rigidbody))]
public class EFire : MonoBehaviour {

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
        if(other.tag == "player")
        {
            other.SendMessage("OnHitEnemyBullet", damage, SendMessageOptions.RequireReceiver);

            //演出自動再生のオブジェクトを生成
            Instantiate(hitParticlePrefab, Camera.transform.position, Camera.transform.rotation);
        }
        

        Destroy(gameObject);
    }
}
