using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲーム中の残り時間を表示するタイマーのクラス
/// </summary>
[RequireComponent(typeof(Text))]
public class RemainTimer : MonoBehaviour
{

    [SerializeField] float gameTime = 180.0f;
    Text uiText;
     public float currentTime;
    GameObject[] enemy;

    // Use this for initialization
    void Start()
    {
        uiText = GetComponent<Text>();

        //残り時間を設定
        currentTime = gameTime;
    }

    // Update is called once per frame
    void Update()
    {

        //残り時間を計算
        currentTime -= Time.deltaTime;
        //０以下にならない
        if (currentTime <= 0.0f)
        {
            currentTime = 0.0f;
        }
        //残り時間テキスト更新
        uiText.text = string.Format("time : {0:F} s", currentTime);

        if(currentTime == 0.0f)
        {
            enemy = GameObject.FindGameObjectsWithTag("enemy");
            for (int i = 0; i< enemy.Length; i++)
            {
                enemy[i].SendMessage("Timeup");
            }
        }

    }

    //カウントダウン中か？
    public bool IsCountingDown()
    {
        //カウンターが0でなければカウント中
        return currentTime > 0.0f;
    }

    void GoDown()
    {
        currentTime = 0.01f;
    }
}
