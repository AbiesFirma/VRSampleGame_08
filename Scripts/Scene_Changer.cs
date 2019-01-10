using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// UI等からシーンを移動、リロードを呼び出すためのクラス
/// </summary>
public class Scene_Changer : MonoBehaviour {

    [SerializeField] GameObject NowLoading;

    private void Start()
    {
        if(NowLoading == null)
        {
            NowLoading = GameObject.Find("NowLoading");
        }

        if (NowLoading.activeSelf)
        {
            NowLoading.SetActive(false);
        }    
    }

    //ほかのスクリプトから呼べるリスタート関数
    public void Restart()
    {
        NowLoading.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //ゲーム用リロード関数(上とほぼ同じ？)
    public void RelodScene()
    {
        NowLoading.SetActive(true);
        //アクティブシーン取得
        var scene = SceneManager.GetActiveScene();
        //リロード
        SceneManager.LoadScene(scene.name);
    }
   
    //指定されたシーンをロードする
    public void LoadScene(string sceneName)
    {
        NowLoading.SetActive(true);
        SceneManager.LoadScene(sceneName);
    }
     
}
