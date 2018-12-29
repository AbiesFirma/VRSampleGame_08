using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_moveCheck : MonoBehaviour {

    Animator animator;
    Vector3 from_pos;
    Vector3 to_pos;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        from_pos = this.transform.position;
        
    }
	
	// Update is called once per frame
	void Update () {
        to_pos = this.transform.position;
        var move_pos = to_pos - from_pos;

        this.transform.forward = move_pos;

        if (move_pos.magnitude > 0.001f)
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }

        from_pos = to_pos;
                
	}
}
