using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApGageUIController : MonoBehaviour {

    [SerializeField] GameObject battleController;
    float fullAp;
    float curretAp;
    bool setReady = false;
    float apRatio;

    Image apGage;
    Text apText;
    [SerializeField] bool textOn = false;

	void Start () {
        if(textOn)
        {
            apText = GetComponent<Text>();
        }
        else
        {
            apGage = GetComponent<Image>();
        }


		if(battleController == null)
        {
            battleController = GameObject.Find("battleController");
        }

        if(battleController.GetComponent<BattleAPController>().setReady)
        {
            StartUP();
        }

	}
	
	void Update () {
        if(!setReady && battleController.GetComponent<BattleAPController>().setReady)
        {
            StartUP();
        }
        else if(setReady)
        {
            if (textOn)
            {
                curretAp = battleController.GetComponent<BattleAPController>().allAp;
                apText.text = string.Format("Ap: " + "{0}" + "/" + "{1}", curretAp, fullAp);
            }
            else
            {
                //更新
                apGage.fillAmount = battleController.GetComponent<BattleAPController>().allAp * apRatio;
            }
        }
		
	}

    void StartUP()
    {
        fullAp = battleController.GetComponent<BattleAPController>().fullAp;
        
        curretAp = battleController.GetComponent<BattleAPController>().allAp;

        if (textOn)
        {
            apText.text = string.Format("Ap: " + "{0}" + "/" + "{1}", curretAp, fullAp);
        }
        else
        { 
            apRatio = 1 / fullAp;
            apGage.fillAmount = curretAp * apRatio;
        }

        setReady = true;

    }

}
