using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusGo_ViewController : MonoBehaviour {
    
    [SerializeField] private float rotSpeed = 20.0f;
   
    /*
    private Transform Controller
    {
        get
        {
            // 現在アクティブなコントローラーを取得
            var controller = OVRInput.GetActiveController();
            if (controller == OVRInput.Controller.RTrackedRemote)
            {
                return _RightHandAnchor;
            }
            else if (controller == OVRInput.Controller.LTrackedRemote)
            {
                return _LeftHandAnchor;
            }
            else
            {
                return null;
            }
        }
    }
    */

    void Start()
    {
                
    }

    void Update()
    {
               
    }

    public void Left_Button_Click()
    {
        this.transform.Rotate(0, -1.0f * 10.0f * rotSpeed * Time.deltaTime, 0);
    }
    public void Right_Button_Click()
    {
        this.transform.Rotate(0, 1.0f * 10.0f * rotSpeed * Time.deltaTime, 0);
    }

}
