using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour {

    GameObject Camera;

	// Update is called once per frame
	void Update ()
    {
        Camera = GameObject.Find("OVRCameraRig");
        var camera_pos = Camera.GetComponent<Transform>().position;
        camera_pos.y = transform.position.y;
        transform.LookAt(camera_pos);
	}
}
