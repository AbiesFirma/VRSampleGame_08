using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム開始から指定秒数で指定のオブジェクトをアクティブにするクラス
/// </summary>
public class AutoActive : MonoBehaviour {

    [SerializeField] float[] time;
    public GameObject[] Objs;
    float GameTime;
        
    void Start()
    {
        for (int i = 0; i < Objs.Length; i++)
        {
            Objs[i].SetActive(false);
        }
        GameTime = 0.0f;
        
    }

    void Update()
    {
        GameTime += Time.deltaTime;
        
        for (int i = 0; i < Objs.Length; i++)
        {           
            if (time[i] < GameTime && !Objs[i].activeSelf)
            {
                Objs[i].SetActive(true);
            }
;        }
    }
    
}
