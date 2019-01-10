using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の出現時間と位置を管理するクラス
/// </summary>
public class SpawnController : MonoBehaviour {

    [SerializeField] float spawnInterval = 2.0f;

    EnemySpawner[] spawners;
    float timer = 0f;

	// Use this for initialization
	void Start ()
    {
        //子に存在するEnemySpawnerのリストを取得
        spawners = GetComponentsInChildren<EnemySpawner>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        
        //出現間隔の判定
        if(spawnInterval < timer)
        {
            //ランダムに選択して敵を出現させる
            var index = Random.Range(0, spawners.Length);
            spawners[index].Spawn();

            timer = 0f;
        }
	}
}
