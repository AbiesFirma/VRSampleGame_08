using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Controller : MonoBehaviour {

    private Animator animator;
    [SerializeField] AudioSource DooraudioSource;
    
    void Start () {

        animator = GetComponent<Animator>();
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("open", true);
            DooraudioSource.Play();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("open", false);
            DooraudioSource.Play();
        }
    }
    
}
