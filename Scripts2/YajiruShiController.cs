using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YajiruShiController : MonoBehaviour {

    [SerializeField] GameObject yajirushi;
    [SerializeField] GameObject enterTarget;

    void Start () {
	}
	
	
	void Update () {

	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == enterTarget)
        {
            yajirushi.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == enterTarget)
        {
            yajirushi.SetActive(false);
        }
    }
}
