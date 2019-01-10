using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ポインターのヒットしているオブジェクトにアクションを起こさせるためのくらす
/// </summary>
public class PointerHitActionController : MonoBehaviour {

    //Rigidbody rb;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    void Update () {
        
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {            
            var HitObj = GetComponent<ControllerSelection.OVRPointerVisualizer>().hitObj;
            if (HitObj.tag == "CanTalkObject")
            {
                HitObj.GetComponent<TalkCanvusController>().Pointer_onClick();
                //HitObj.transform.position += new Vector3(0, 0.1f, 0);
            }

            if (HitObj.tag == "Ball")
            {
                HitObj.transform.position += new Vector3(0, 0.2f, 0);
            }
        }
    }
}
