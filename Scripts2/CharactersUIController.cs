using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 名前、HpなどUI表示を管理するクラス
/// </summary>

public class CharactersUIController : MonoBehaviour {

    [SerializeField] Image circleHPBar;     //円形HPゲージ
    
    [SerializeField] GameObject charaNameTMP;      //キャラネームオブジェクト
    [SerializeField] GameObject charaHPValueTMP;   //HPの数値オブジェクト
    TextMeshPro hpValue;
    string nameJanpanese;

    [SerializeField] GameObject player;
    GameObject character;
    [SerializeField, Range(0, 3)] int charaNumber;     //キャラの番号づけ
    float maxHP;
    float hp;
    bool setReady = false;

    float hpRatio;  //BarのMax1とHpMaxの割合

    [SerializeField] bool smallUI = false;
	
	void Start () {

        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("playerRoot");
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
            //更新
            hp = character.GetComponent<BattleCharacterState>().currentHP;
            if (!smallUI)
            {
                circleHPBar.fillAmount = hp * hpRatio;
            }
            hpValue.text = string.Format("Hp " + "{0}", hp);
        }
    }
    

    void StartUP()
    {
        //キャラリストを取得
        var charaList = player.GetComponent<CharacterMoveOrder>().charactersListP;
        character = charaList[charaNumber];

        //各数値の取得と初期表示
        hpValue = charaHPValueTMP.GetComponent<TextMeshPro>();

        maxHP = character.GetComponent<BattleCharacterState>().maxhp;
        hp = character.GetComponent<BattleCharacterState>().currentHP;
        nameJanpanese = character.GetComponent<BattleCharacterState>().charaName;

        charaNameTMP.GetComponent<TextMeshPro>().text = string.Format("{0}", nameJanpanese);

        if (!smallUI)
        {
            hpRatio = 1 / maxHP;

            circleHPBar.fillAmount = hp * hpRatio;
        }

        hpValue.text = string.Format("Hp " + "{0}", hp);

        setReady = true;
    }
}
