using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardPopuptext : MonoBehaviour {
    
    //カメラとおなじ方向を向くLookAtの逆、背面をカメラに
	void Update () {
        transform.forward = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>().transform.forward;
	}
}
