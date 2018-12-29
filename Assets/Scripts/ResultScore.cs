using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ResultScore : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        var gameObj = GameObject.FindWithTag("Score");
        var score = gameObj.GetComponent<Score>();
        var uiText = GetComponent<Text>();
        uiText.text = string.Format(" {0:D3}", score.Points);
	}
	
}
