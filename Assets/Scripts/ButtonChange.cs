using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChange : MonoBehaviour {

    
    [SerializeField] private GameObject OVRCameraRig;
    [SerializeField] private GameObject SubOVRCameraRig;

    public void OnClickButton()
    {
        if (Input.GetMouseButton(0) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKey(KeyCode.RightShift))       //space,enterでは反応しないように
        {
            // Textコンポーネント郡を取得します。(Canvusの子のコンポーネント”群”なので配列になり[0]は最初のText)
            var components = this.gameObject.GetComponentsInChildren<Text>();

            // テキストを文字の状態によって変更するようにします。(A ? B : C  でAがTureならB。FalseならCを入れる)
            components[0].text = components[0].text == "FPCamera" ? "TPCamera" : "FPCamera";

            //両カメラのアクティブを反転させクリックのたびに切り替える　(!で反転)
            OVRCameraRig.SetActive(!OVRCameraRig.activeSelf);
            SubOVRCameraRig.SetActive(!SubOVRCameraRig.activeSelf);
        }
    }
       
}