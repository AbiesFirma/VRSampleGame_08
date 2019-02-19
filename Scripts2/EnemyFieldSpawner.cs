using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドでのエンカウント用敵の出現を管理するクラス
/// </summary>

public class EnemyFieldSpawner : MonoBehaviour {

    [SerializeField] GameObject[] enemyPrefabs;   //enemyのプレハブリスト
    [SerializeField] float spawnInterval = 2.0f;  //出現感覚
    List<GameObject> enemysList;
    [SerializeField] int maxEnemyNumber = 10;    //敵の最大数

    [SerializeField] GameObject[] spawnPoint;   //出現位置
    float timer = 0f;

    void Start()
    {
        //最初の敵の数を取得しリスト化
        var enemy = GameObject.FindGameObjectsWithTag("enemy");
        enemysList = new List<GameObject>();
        enemysList.AddRange(enemy);
    }

    void Update()
    {
        timer += Time.deltaTime;

        //出現間隔と現在のマップ上での敵の合計数によって敵を出現させる
        if (spawnInterval < timer && enemysList.Count <= maxEnemyNumber)
        {
            //ランダムに選択して敵を出現させる
            var pointIndex = Random.Range(0, spawnPoint.Length);
            var point = spawnPoint[pointIndex];
            Spawn(point);

            timer = 0f;
        }
    }


    public void Spawn(GameObject _point)
    {
        //敵を出現させリストに加える
        var index = Random.Range(0, enemyPrefabs.Length);
        var newEnemy = Instantiate(enemyPrefabs[index], _point.transform.position, _point.transform.rotation);
        enemysList.Add(newEnemy);
        //Debug.Log(enemysList.Count);
    }
}
