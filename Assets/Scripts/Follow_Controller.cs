using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Controller : MonoBehaviour {
    Transform Controller;
    Transform _RightHand;
    Transform _LeftHand;
    Vector3 con_pos1;
    Vector3 con_pos2;
    Vector3 wep_pos;
    Vector3 dis1;
    //Quaternion con_rot1;


    // Use this for initialization
    void Start()
    {
        var controller = OVRInput.GetActiveController();
        if (controller == OVRInput.Controller.RTrackedRemote)
        {
            Controller = _RightHand;
        }
        else if (controller == OVRInput.Controller.LTrackedRemote)
        {
            Controller = _LeftHand;
        }

        con_pos1 = Controller.transform.position;
        
        wep_pos = transform.position;
        dis1 = wep_pos - con_pos1;
    }

    // Update is called once per frame
    void Update()
    {
        con_pos2 = Controller.transform.position;
        wep_pos = transform.position;
        var con_rot = Controller.transform.rotation;

        var con_move = con_pos2 - con_pos1;

        if (con_move != Vector3.zero)
        {
            transform.Translate(con_move);
        }

        var dis2 = wep_pos - con_pos2;

        if (dis1 != dis2)
        {
            var wep_move = dis1 - dis2;
            transform.Translate(wep_move);
        }

        transform.rotation = con_rot;

    }
}
