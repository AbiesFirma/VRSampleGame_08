using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class UNITY_EDITOR_ViewControoler : MonoBehaviour
{
#if UNITY_EDITOR
    public Transform verRot;
    public Transform horRot;
    public float speed = 2;


    /////////////////////////////////////////////////////////////////////////////
    [SerializeField] private GameObject OVRCameraRig;
    [SerializeField] private GameObject SubOVRCameraRig;
    //////////////////////////////////////////////////////////////////////
    



    // Use this for initialization
    void Start()
    {
        //回転軸を親子で分けることで意図しない傾きを防ぐ.
        //Y軸回転は体ごと（プレイヤーRoot）X軸回りはカメラのみ（首だけに相当、そうしないと体も回ってしまう）
        verRot = transform.parent;
        horRot = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float X_Rotation = Input.GetAxis("Mouse X");
            float Y_Rotation = Input.GetAxis("Mouse Y");
            var X_Rot = X_Rotation * speed ;
            var Y_Rot = Y_Rotation * speed ;
            verRot.transform.Rotate(0, -X_Rot, 0);
            horRot.transform.Rotate(Y_Rot, 0, 0);
        }




        ///////////////////////////////////////////////////////////////////////////////////
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                //残手措置

                //両カメラのアクティブを反転させクリックのたびに切り替える　(!で反転)
                OVRCameraRig.SetActive(!OVRCameraRig.activeSelf);
                SubOVRCameraRig.SetActive(!SubOVRCameraRig.activeSelf);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////


    }

   
#endif
}