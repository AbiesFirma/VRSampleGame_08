using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UIのボタンにより視点を左右に振るクラス
/// </summary>
public class OculusGo_ViewController : MonoBehaviour {
    
    [SerializeField] private float rotSpeed = 20.0f;
   
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
