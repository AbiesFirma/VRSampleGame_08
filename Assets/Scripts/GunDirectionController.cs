using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDirectionController : MonoBehaviour
{
    /*
    Transform Controller;
    Transform _RightHandModel;
    Transform _LeftHandModel;
    */
    [SerializeField] Transform sphere;
    
    /*
    void Start()
    {
        var controller = OVRInput.GetActiveController();
        if (controller == OVRInput.Controller.RTrackedRemote)
        {
            Controller = _RightHandModel;
        }
        else if (controller == OVRInput.Controller.LTrackedRemote)
        {
            Controller = _LeftHandModel;
        }

    }
    */

    void Update()
    {
        this.transform.LookAt(sphere);
        //transform.rotation = Controller.rotation;
        //transform.position = Controller.position;
    }
}
