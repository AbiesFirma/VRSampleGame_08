using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターを管理するためのシングルトン
/// </summary>
public class BattleCharactersManager : MonoBehaviour {

    public static GameObject singletonCharactorManagerSystem;
    public List<GameObject> battleCharactersList;

    void Awake()
    {
        if (singletonCharactorManagerSystem == null)
        {
            DontDestroyOnLoad(this.gameObject);
            singletonCharactorManagerSystem = this.gameObject;
        }
        else
        {
            Destroy(this.gameObject);
        }
               

    }

}
