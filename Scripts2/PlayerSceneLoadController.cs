using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンをロード,フェードを再現,するためのクラス
/// </summary>
public class PlayerSceneLoadController : MonoBehaviour {

    [SerializeField] float fadeOutTime = 1.0f;
    [SerializeField] float fadeInTime = 1.0f;

    [SerializeField] GameObject nowLoading;
    [SerializeField] Image fadeImage;
    [SerializeField] bool fadeOut;
    [SerializeField] bool fadeIn = true;

    [SerializeField] string battleScene;     //バトルシーン名
    [SerializeField] string feildScene;     //フィールドシーン名
    string currentScene;

    [SerializeField] GameObject player;
    [SerializeField] AudioSource playerRootAudioSouce;
    [SerializeField] AudioClip encountAudio;

    void Start() {
        if (fadeIn)
        {
            fadeImage.fillAmount = 1.0f;
        }
        else
        {
            fadeImage.fillAmount = 0.0f;
        }
        fadeOut = false;
        currentScene = SceneManager.GetActiveScene().name;

        if (player == null)
        {
            player = transform.root.gameObject;
        }
    }

    private void Update()
    {
        if (fadeOut)
        {
            var speed = Time.deltaTime / fadeOutTime;  //時間内にマックスになるためのフレーム毎の変化量係数
            var fade = 1.0f * speed;         //fill amountのマックス１.0に対してのフレーム毎の変化
            fadeImage.fillAmount += fade;    //変化量をプラス

            if (fadeImage.fillAmount > 0.8f)
            {
                nowLoading.SetActive(true);
            }

            if (fadeImage.fillAmount == 1.0f)
            {
                if (currentScene == battleScene)
                {
                    SceneManager.LoadScene(feildScene);
                }
                else if (currentScene == feildScene)
                {
                    FeildPlayerState.feildPlayerPos = player.transform.position;
                    SceneManager.LoadScene(battleScene);
                }
            }
        }


        if(fadeIn)
        {
            nowLoading.SetActive(true);
            var speed = Time.deltaTime / fadeInTime;  //時間内に0になるためのフレーム毎の変化量係数
            var fade = 1.0f * speed;         //fill amountのマックス１.0に対してのフレーム毎の変化
            fadeImage.fillAmount -= fade;    //変化量をマイナス

            if (fadeImage.fillAmount < 0.9f)
            {
                nowLoading.SetActive(false);
            }
            if (fadeImage.fillAmount <= 0.0f)
            {
                fadeIn = false;
            }

        }




    }

    //FeildEnemyEncountからのSendMessage
    void EnemyEncount(GameObject enemy)
    {
        playerRootAudioSouce.PlayOneShot(encountAudio);
        fadeOut = true; 
        
    }
    
    //BattleControllerからのSendMessage
    public void BattleEnd()
    {
        fadeOut = true;
    }
   
}

    

