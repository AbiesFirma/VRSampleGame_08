using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardPopuptext : MonoBehaviour {
    
    //カメラとおなじ方向を向くLookAtの逆、平面をカメラに
	void Update () {
        transform.forward = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>().transform.forward;
	}
}
