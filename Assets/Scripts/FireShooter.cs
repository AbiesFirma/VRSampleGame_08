using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;


public class FireShooter : MonoBehaviour {
 
    [SerializeField] GameObject firePrefab;
    [SerializeField] Transform fireBarrelEnd;

    [SerializeField] ParticleSystem gunParticle;
    [SerializeField] AudioSource gunAudioSource;

    [SerializeField] float fire_limit = 5.0f;
    float timer;

    private void Start()
    {
        timer = fire_limit;
    }

    void Update()
    {
        timer += Time.deltaTime;

        //入力に応じて玉を発射
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && timer > fire_limit && Time.timeScale != 0)
        {
            Shoot();
            timer = 0.0f;
        }

#if UNITY_EDITOR
        if (Input.GetButtonUp("Fire1") && timer > fire_limit && Time.timeScale != 0)
        {
            Shoot();
            timer = 0.0f;
        }
#endif
    }

    void Shoot()
    {
        //プレハブから球を作成
        Instantiate(firePrefab, fireBarrelEnd.position, fireBarrelEnd.rotation);

        //発射パーティクル
        gunParticle.Play();
        //発射音
        gunAudioSource.Play();
    }
}
