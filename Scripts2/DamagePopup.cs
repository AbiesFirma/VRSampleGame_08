using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour {

    [SerializeField] float destroyTime = 1.0f;
    [SerializeField] float popPower = 10.0f;

	void Start () {
        var rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * popPower);
        Destroy(gameObject, destroyTime);
	}
	
	
}
