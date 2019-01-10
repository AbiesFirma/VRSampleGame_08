using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 破壊できない敵（砲台など）の処理
/// </summary>
public class Enemy_CannotDestroy : MonoBehaviour {

    [SerializeField] AudioClip hitClip;       //ヒット音
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnHitBullet(int damage)
    {
        //着弾音
        audioSource.PlayOneShot(hitClip);
        
    }

    //タイムアップで破壊
    void Timeup()
    {
        Destroy(gameObject);
    }
}
