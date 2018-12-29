using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class Score : MonoBehaviour
{

    Text uiText;
    public int Points { get; private set; }

    // Use this for initialization
    void Start()
    {
        uiText = GetComponent<Text>();
    }

    public void AddScore(int addPoint)
    {
        //ポイント加算
        Points += addPoint;
        //テキスト更新
        uiText.text = string.Format("Score : {0:D3} ", Points);
    }
}