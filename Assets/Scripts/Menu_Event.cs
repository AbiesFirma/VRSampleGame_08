using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Event : MonoBehaviour {

    [SerializeField] GameObject Menu;
    [SerializeField] GameObject GameMenu;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject rotButton;
    [SerializeField] GameObject HelpMenu;
    


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
    
}
