using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Raycast : MonoBehaviour {

    GameObject CameraRig;

    private void Start()
    {
        CameraRig = GameObject.Find("OVRCameraRig");
    }

    private void Update()
    {
        Ray ray = new Ray(this.transform.position, transform.forward);    //Ray作成
        RaycastHit hit = new RaycastHit();
        int layerMask = ~(1 << 15);                                   //プレイヤーレイヤーマスク
        float ray_distance = 10.0f;                                    //Rayの長さ
        Debug.DrawRay(ray.origin, ray.direction * ray_distance, Color.red);  //Rayの可視化

        if (Physics.Raycast(ray, out hit, ray_distance, layerMask))           //Rayが当たっていたら
        {
            string hit_name = hit.collider.gameObject.name;

            if (hit_name == "PlayerRoot 1/OVRCameraRig/TrackingSpace/CenterEyeAnchor/Rot Menu/Rot Canvas/Left")
            {
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                {
                    CameraRig.GetComponent<OculusGo_ViewController>().Left_Button_Click();
                }
            }
            else if (hit_name == "PlayerRoot 1/OVRCameraRig/TrackingSpace/CenterEyeAnchor/Rot Menu/Rot Canvas/Right")
            {
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                {
                    CameraRig.GetComponent<OculusGo_ViewController>().Right_Button_Click();
                }
            }
        }

    }
}
