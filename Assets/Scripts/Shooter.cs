using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;


public class Shooter : MonoBehaviour {

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform gunBarrelEnd;

    [SerializeField] ParticleSystem gunParticle;
    [SerializeField] AudioSource gunAudioSource;


    void Update()
    {
        //入力に応じて玉を発射
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && Time.timeScale != 0)
        {
            Shoot();
        }

#if UNITY_EDITOR
        if (Input.GetButtonDown("Fire1") && Time.timeScale != 0)
        {
            Shoot();
        }
#endif
    }

    void Shoot()
    {
        //プレハブから球を作成
        Instantiate(bulletPrefab, gunBarrelEnd.position, gunBarrelEnd.rotation);

        //発射パーティクル
        gunParticle.Play();
        //発射音
        gunAudioSource.Play();
    }
}
