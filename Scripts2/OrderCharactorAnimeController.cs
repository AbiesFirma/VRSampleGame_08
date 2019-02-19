using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 結合したので未使用
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class OrderCharactorAnimeController : MonoBehaviour {

    private Animator animator;
    Rigidbody rb;

    Vector3 from_pos;
    Vector3 to_pos;

    //AudioSource audioSource;
    //[SerializeField] AudioClip[] _se;


    enum AnimeState
    {
        idle,
        run,
        attack,
        damage,
        dead
    }
    AnimeState aState;
    


    void Start () {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        from_pos = transform.position;
        to_pos = transform.position;
    }


    void Update()
    {
        to_pos = transform.position;
        var move = to_pos - from_pos;
                
        if (move.magnitude > 0.01f)
        {
            aState = AnimeState.run;
        }
        else
        {
            aState = AnimeState.idle;
        }


        switch (aState)
        {
            case AnimeState.idle:
                animator.SetBool("Run", false);
                break;

            case AnimeState.run:

                animator.SetBool("Run", true);
                break;

        }

        from_pos = to_pos;

    }

}
