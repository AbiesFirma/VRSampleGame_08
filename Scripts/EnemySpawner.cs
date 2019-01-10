using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵の出現を管理するクラス。ランダムに指定ポイントにプレハブから生成
public class EnemySpawner : MonoBehaviour {

    [SerializeField] GameObject[] enemyPrefabs;
    //[SerializeField] Enemy[] enemyPrefabs;

    GameObject enemy;
    //Enemy enemy;

	public void Spawn()
    {
        if(enemy == null)
        {
            var index = Random.Range(0, enemyPrefabs.Length);

            enemy = Instantiate(enemyPrefabs[index], transform.position, transform.rotation);
        }
    }
}
