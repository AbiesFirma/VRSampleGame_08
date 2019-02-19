using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// メニュー画面のUI表示を管理するためのクラス
/// </summary>
public class Menu_Event : MonoBehaviour {

    [SerializeField] GameObject Menu;
    [SerializeField] GameObject GameMenu;
    [SerializeField] GameObject WarpMenu;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject rotButton;
    [SerializeField] GameObject HelpMenu;
    [SerializeField] GameObject MiniMap;
    [SerializeField] GameObject ItemMenu;

    public bool _rotButton;
    public bool _miniMap; 

    [SerializeField] GameObject viewMesure;
    string scene;
    [SerializeField] string feildScene = "VRSample08-summon-field";

    private void Start()
    {
        scene = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (scene == feildScene)
        {
            if (rotButton.activeSelf)
            {
                _rotButton = true;
            }
            else
            {
                _rotButton = false;
            }

            if (MiniMap.activeSelf)
            {
                _miniMap = true;
            }
            else
            {
                _miniMap = false;
            }

            FeildPlayerState.rotButton = _rotButton;
            FeildPlayerState.miniMap = _rotButton;
        }
    }


    public void Open_Menu()
    {
       if(GameMenu.activeSelf || HelpMenu.activeSelf)
       {
            GameMenu.SetActive(false);
            HelpMenu.SetActive(false);
        }

            Menu.SetActive(true);
        
    }


    public void Close_Menu()
    {
        //Menu = GameObject.Find("Menu");
        Menu.SetActive(false);
        GameMenu.SetActive(false);
        HelpMenu.SetActive(false);
    }


    public void Close_GameMenu()
    {
        //GameMenu = GameObject.Find("GamePanel");
        GameMenu.SetActive(false);
    }


    public void Open_GameMenu()
    {
        //GameMenu = GameObject.Find("GamePanel");

        GameMenu.SetActive(true);
        
    }

    public void Close_WarpMenu()
    {
        
        WarpMenu.SetActive(false);
    }


    public void Open_WarpMenu()
    {
        WarpMenu.SetActive(true);

    }

    public void Open_Help()
    {
        HelpMenu.SetActive(true);

    }

    public void Close_Help()
    {        
        HelpMenu.SetActive(false);

    }

    //ポーズ
    public void Pause()
    {       
        if (!pauseUI.activeSelf)
        {
            Menu.SetActive(false);
            GameMenu.SetActive(false);
            HelpMenu.SetActive(false);
            pauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    public void PauseBack()
    {
        Time.timeScale = 1.0f;
        pauseUI.SetActive(false);
        Menu.SetActive(false);
        GameMenu.SetActive(false);
        HelpMenu.SetActive(false);
    }

    public void RotButton()
    {
        if (!rotButton.activeSelf)
        {           
            rotButton.SetActive(true);         
        }
        else
        {
            rotButton.SetActive(false);
            
        }
    }

    public void MiniMapOC()
    {
        if(!MiniMap.activeSelf)
        {
            MiniMap.SetActive(true);
        }
        else
        {
            MiniMap.SetActive(false);
        }
    }

   public void ViewMesureOC ()
    {
        if (!viewMesure.activeSelf)
        {
            viewMesure.SetActive(true);
        }
        else
        {
            viewMesure.SetActive(false);
        }
    }

    public void ItemMenuOC()
    {
        if (!ItemMenu.activeSelf)
        {
            ItemMenu.SetActive(true);
        }
        else
        {
            ItemMenu.SetActive(false);
        }
    }
}
