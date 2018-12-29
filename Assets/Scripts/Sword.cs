using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;


public class Sword : MonoBehaviour {
   
    [SerializeField] ParticleSystem hitParticlePrefab;
   

    // Use this for initialization
    void Start()
    {
        
    }

    void Update()
    {
        /*
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            lr = LineObj.GetComponent<LineRenderer>();
            lr.startWidth = 0.008f;
            lr.endWidth = 0.005f;
        }
        else
        {
            lr = LineObj.GetComponent<LineRenderer>();
            lr.startWidth = 0.0f;
            lr.endWidth = 0.0f;
        } 
        */
    }

    void OnTriggerEnter(Collider other)
    {
       
         other.SendMessage("OnHitBullet");

         //着弾地点に演出自動再生のオブジェクトを生成
         Instantiate(hitParticlePrefab, transform.position, transform.rotation);
        
    }
    
}
