using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour {

    public static GameObject singletonGameSystem;

    void Awake()
    {
        if (singletonGameSystem == null)
        {
            DontDestroyOnLoad(this.gameObject);
            singletonGameSystem = this.gameObject;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

}
