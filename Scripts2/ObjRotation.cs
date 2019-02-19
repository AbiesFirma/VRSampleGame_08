using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotation : MonoBehaviour {
    [SerializeField] float speed = 1;
	
	void Start () {
		
	}
	
	
	void Update () {
        transform.Rotate(0, speed, 0);
	}
}
