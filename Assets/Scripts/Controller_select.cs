using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_select : MonoBehaviour {

    //OVRCameraRig/TrackingSpace/のLeftかRightHandAnchor/TrackiedRemote/のgun
    [SerializeField] GameObject Left_Gun;
    [SerializeField] GameObject Right_Gun;
    [SerializeField] GameObject HandChild_Gun; //念のためのキャラ右手のボーンの子にあるGun（キャラ/root/pelvis/spine01/spine02/spine03/clavicle_R/upperarm_R/lowerarm_R/hand_R/gunDirection）

       
    void Start () {
        //activeなコントローラーを取得
        var controller = OVRInput.GetActiveController();

        //事前に入ってない場合に見つけられるように
        if (Left_Gun == null)
        {
            Left_Gun = GameObject.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor/gunDirection");
        }
        if (Right_Gun == null)
        {
            Right_Gun = GameObject.Find("OVRCameraRig/TrackingSpace/RightHandAnchor/gunDirection");
        }

        //左右に応じてどちらかを非アクティブにする。うまくいかなかったら手の子のGunが表示されるように。
        if (controller == OVRInput.Controller.LTrackedRemote)
        {
            Left_Gun.SetActive(true);
            Right_Gun.SetActive(false);
        }
        else if (controller == OVRInput.Controller.RTrackedRemote)
        {
            Left_Gun.SetActive(false);
            Right_Gun.SetActive(true);
        }
        else
        {
            Left_Gun.SetActive(false);
            Right_Gun.SetActive(false);
            HandChild_Gun.SetActive(true);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
