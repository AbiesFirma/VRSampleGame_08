using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャラの頭上に表示されるウインドウの管理クラス
/// </summary>
public class GazeMenuEvent : MonoBehaviour {

    [SerializeField] GameObject chara;

    private void Start()
    {
        if (chara == null)
        {
            var _chara = transform.parent.gameObject;
            var __chara = _chara.transform.parent.gameObject;
            chara = __chara.transform.parent.gameObject;
        }
    }

    private void Update()
    {
        var ready = chara.GetComponent<BattleCharacterController>().ready;
        Text text = transform.Find("Text").gameObject.GetComponent<Text>();
        text.text = ready == true ? "Wait" : "AutoMove";
    }

    public void WaitButtonAction()
    {
        chara.GetComponent<BattleCharacterController>().Wait();
    }
           
    

}
