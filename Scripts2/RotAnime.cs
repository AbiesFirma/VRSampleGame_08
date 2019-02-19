using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotAnime : MonoBehaviour {

    [SerializeField] float xSpeed = 1.0f;
    [SerializeField] float ySpeed = 1.0f;
    [SerializeField] float zSpeed = 1.0f;

    void Start () {
		
	}
	
	void Update () {
        transform.Rotate(new Vector3 (xSpeed, ySpeed, zSpeed));
	}
}
