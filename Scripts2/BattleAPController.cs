using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleAPController : MonoBehaviour {

    public float allAp = 0.0f;
    public float fullAp { get; private set; }

    [SerializeField] Image apGage;

    [SerializeField] GameObject player;
    public bool setReady{ get; private set;}

	void Start () {
        setReady = false;

        if (player == null)
        {
            player = GameObject.Find("PlayerRoot");
        }

        if (player.GetComponent<CharacterMoveOrder>().charaSetReady)
        {
            StartUP();
        }
    }
	

	void Update () {
        if (!setReady)
        {
            StartUP();
        }
        else
        {
            Debug.Log(allAp);
        }
	}

    void StartUP()
    {
        //キャラリストを取得
        var charaList = player.GetComponent<CharacterMoveOrder>().charactersListP;
        for(int i = 0; i < charaList.Count; i++)
        {
            allAp += charaList[i].GetComponent<BattleCharacterState>().ap;
            fullAp += charaList[i].GetComponent<BattleCharacterState>().ap * 5.0f;
        }
        setReady = true;

    }
}
