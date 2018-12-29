using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_select_S : MonoBehaviour {

    [SerializeField] GameObject Left_W;
    [SerializeField] GameObject Right_W;
    [SerializeField] GameObject HandChild_W; //念のためのキャラ右手のボーンの子にあるGun（キャラ/root/pelvis/spine01/spine02/spine03/clavicle_R/upperarm_R/lowerarm_R/hand_R/gunDirection）

    GameObject SceneChanger;
       
    void Start () {
        //activeなコントローラーを取得
        var controller = OVRInput.GetActiveController();

        //事前に入ってない場合に見つけられるように
        if (Left_W == null)
        {
            Left_W = GameObject.Find("PlayerRoot_Action/OVRCameraRig/TrackingSpace/LeftHandAnchor/Sword");
        }
        if (Right_W == null)
        {
            Right_W = GameObject.Find("PlayerRoot_Action/OVRCameraRig/TrackingSpace/RightHandAnchor/Sword");
        }

        //左右に応じてどちらかを非アクティブにする。うまくいかなかったらリスタート。
        if (controller == OVRInput.Controller.LTrackedRemote)
        {
            Left_W.SetActive(true);
            Right_W.SetActive(false);
        }
        else if (controller == OVRInput.Controller.RTrackedRemote)
        {
            Left_W.SetActive(false);
            Right_W.SetActive(true);
        }
        else
        {

            SceneChanger = GameObject.Find("SceneChanger");
            SceneChanger.GetComponent<Scene_Changer>().Restart();

        }


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
