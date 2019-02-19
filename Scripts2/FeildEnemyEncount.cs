using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// フィールドでエンカウントした際の演出ようのクラス
/// </summary>
public class FeildEnemyEncount : MonoBehaviour {

    //[SerializeField] string loadScene;
    GameObject enemy;
    GameObject playerLoadCanvasController;
    GameObject player;

	void Start () {
        enemy = this.gameObject;
        playerLoadCanvasController = GameObject.Find("LoadCanvasControllerRoot");
        player = playerLoadCanvasController.transform.root.gameObject;
	}	
	
	void Update () {
		
	}

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "player" || col.gameObject.tag == "playerRoot")
        {
            GetComponent<EnemyBattleFieldMoveAgent>().encount = true;
            playerLoadCanvasController.SendMessage("EnemyEncount", enemy, SendMessageOptions.RequireReceiver);
            player.SendMessage("EnemyEncount", enemy, SendMessageOptions.RequireReceiver);
            //SceneManager.LoadScene(loadScene);
        }
    }
}
