using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// フィールド時のUI管理くらす
/// </summary>
public class CharacterFieldUIController : MonoBehaviour {
    //各HPのUI
    [SerializeField] Image circleHPBar;

    [SerializeField] GameObject charaNameTMP;
    [SerializeField] GameObject charaHPValueTMP;
    TextMeshPro hpValue;
    string nameJanpanese;

    [SerializeField] string charaRootName;
    GameObject characterRoot;
    float maxHP;
    float hp;

    float hpRatio;  //BarのMax1とHpMaxの割合

    void Start()
    {
        characterRoot = GameObject.Find(charaRootName);

        hpValue = charaHPValueTMP.GetComponent<TextMeshPro>();

        maxHP = characterRoot.GetComponent<CharaStatus>().maxHP;
        hp = characterRoot.GetComponent<CharaStatus>().currentHp;
        nameJanpanese = characterRoot.GetComponent<CharaStatus>().charaName;

        charaNameTMP.GetComponent<TextMeshPro>().text = string.Format("{0}", nameJanpanese);

        hpRatio = 1 / maxHP;

        circleHPBar.fillAmount = hp * hpRatio;

        hpValue.text = string.Format("Hp " + "{0}" + " / " + "{1}", hp, maxHP);
    }

    void Update()
    {
        hp = characterRoot.GetComponent<CharaStatus>().currentHp;
        circleHPBar.fillAmount = hp * hpRatio;
        hpValue.text = string.Format("Hp " + "{0}" + " / " + "{1}", hp, maxHP);
    }

}
