using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の出現時間と位置を管理するクラス
/// </summary>
public class SpawnController : MonoBehaviour {

    [SerializeField] float spawnInterval = 2.0f;
    GameObject[] enemy;
    [SerializeField] int maxEnemyNumber = 15;

    EnemySpawner[] spawners;
    float timer = 0f;

	// Use this for initialization
	void Start ()
    {
        //子に存在するEnemySpawnerのリストを取得
        spawners = GetComponentsInChildren<EnemySpawner>();
	}
	
	void Update ()
    {
        enemy = GameObject.FindGameObjectsWithTag("enemy");

        timer += Time.deltaTime;
        
        //出現間隔と現在のマップ上での敵の合計数によって敵を出現させる
        if(spawnInterval < timer && enemy.Length <= maxEnemyNumber)
        {
            //ランダムに選択して敵を出現させる
            var index = Random.Range(0, spawners.Length);
            spawners[index].Spawn();

            timer = 0f;
        }
	}
}
