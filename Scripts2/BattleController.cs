using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 戦闘の全体を管理するクラス
/// </summary>

public class BattleController : MonoBehaviour {

    [SerializeField] GameObject[] setEnemys;     //出現する敵の種類をセット
    [SerializeField] GameObject[] spawnPoints;   //出現位置をセット
    [SerializeField] int enemyMaxNumber = 7;     //敵の最大数
    int enemyNumber;

    List<GameObject> _charasList;       //キャラリスト
    List<GameObject> _enemysList;       //敵リスト
    [SerializeField] GameObject player;    //プレイヤー

    float ap;       //Ap

    List<string> enemysName;     //敵の名前リスト
    float battletime;           //戦闘時間
    bool pubOrder;              //プレイヤーからの全体命令

    [SerializeField] float endCountDown = 10.0f;     //戦闘終了後画面移動までの時間

    //各種UIのオブジェクト
    [SerializeField] GameObject readyUI;
    [SerializeField] GameObject battleStartUI;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loseUI;
    [SerializeField] GameObject pubOrderUI;
    [SerializeField] GameObject resultUI;
    [SerializeField] Text countDownTimer;
    [SerializeField] GameObject playerLoadCanvasController;

    void Awake () {

        enemyNumber = Random.Range(2, enemyMaxNumber + 1);  //敵の数をランダムに決定

        BattleSpawn();

        //敵の名前に番号つけ
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("enemy");
        
        for (int i = 0; i < enemys.Length; i++)
        {
            string num = i.ToString();
            enemys[i].name += num; 
        }

        //GazeWindowを閉じる
        var gazeWindow = GameObject.FindGameObjectsWithTag("GazeWindow");
        if(gazeWindow.Length != 0)
        {
            for (int i = 0; i < gazeWindow.Length; i++)
            {
                gazeWindow[i].SetActive(false);
            }
        }
    }

    private void Start()
    {
        battletime = 0.0f;
        readyUI.SetActive(true);
        battleStartUI.SetActive(false);
        winUI.SetActive(false);
        loseUI.SetActive(false);

        //敵味方のリストを作成
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("enemy");
        _enemysList = new List<GameObject>();
        _enemysList.AddRange(enemy);
        GameObject[] chara = GameObject.FindGameObjectsWithTag("OrderCharactor");
        _charasList = new List<GameObject>();
        _charasList.AddRange(chara);

        pubOrder = player.GetComponent<CharacterMoveOrder>().pubOrder;

       
    }
    
   
    void Update () {        

        battletime += Time.deltaTime;

        //時間によりUI表示
        //ready?
		if(battletime > 1.5f)
        {
            readyUI.SetActive(false);
            battleStartUI.SetActive(true);
        }
        //BattleStart
        if (battletime > 3.0f)
        {
            readyUI.SetActive(false);
            battleStartUI.SetActive(false);
        }


        //全滅Lose
        if (_charasList.Count == 0)
        {
            loseUI.SetActive(true);

        }

        //戦闘終了Win
        if (_enemysList.Count == 0)
        {
            StartCoroutine("BattleWinResultUI");
            endCountDown -= Time.deltaTime; 

            if (endCountDown <= 0.0f)
            {
                endCountDown = 0.0f;
                countDownTimer.text = string.Format("{0:F1}", 0.0);
                playerLoadCanvasController.SendMessage("BattleEnd"/*, enemy, SendMessageOptions.RequireReceiver*/);                
            }
        }

        //全体命令
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) || Input.GetKeyDown(KeyCode.Space))
        {
            if (!pubOrder)
            {
                pubOrder = true;
                pubOrderUI.SetActive(true);
                //Debug.Log("PO");
            }
            else
            {
                pubOrder = false;
                pubOrderUI.SetActive(false);
                //Debug.Log("POout");
            }
        } 
        

    }

    //敵が死んだとき、BattleEnemyStateから送られてくるSendMessage
    void EnemyDead(GameObject enemy)
    {
        _enemysList.Remove(enemy);
    }
    //キャラが死んだとき、BattleCharacterStateから送られてくるSendMessage
    void CharacterDead(GameObject chara)
    {
        _charasList.Remove(chara);
    }
    
    //戦闘開始時の敵生成
    void BattleSpawn ()
    {
        for (int i = 0; i < enemyNumber; i++)
        {
            //敵の種類をセットされた中からランダムに選択し順に生成
            var enemyIndex = Random.Range(0, setEnemys.Length);
            Instantiate(setEnemys[enemyIndex], spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
        }

    }

    //戦闘終了時のResult表示
    IEnumerator BattleWinResultUI()
    {
        winUI.SetActive(true);
        yield return new WaitForSeconds(2.0f);

        resultUI.SetActive(true);
        countDownTimer.text = string.Format("{0:F1}", endCountDown);
    }

}
