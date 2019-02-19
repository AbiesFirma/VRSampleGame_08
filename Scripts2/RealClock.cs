using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// 現在時刻をUI表示するためのクラス
/// </summary>
public class RealClock : MonoBehaviour {

    Text TimeText;
    
    void Start()
    {
        TimeText = GetComponent<Text>();
    }
	
	
	void Update () {
        TimeText.text = DateTime.Now.ToString("HH : mm : ss");
        //Debug.Log(TimeText.text);
    }
}
