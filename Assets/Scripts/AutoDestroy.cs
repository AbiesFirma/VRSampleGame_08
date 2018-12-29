using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class AutoDestroy : MonoBehaviour {

    [SerializeField] float lifetime = 5.0f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
