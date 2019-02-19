using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelepoModeChage : MonoBehaviour {

    [SerializeField] GameObject player;

	void Start () {
        player.GetComponent<Teleport>().TelepoON();
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
