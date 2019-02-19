using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ポインタ表示を切り替えるクラス（シーンなどにより切り替える場合）
/// </summary>
public class PointerController : MonoBehaviour {

    [SerializeField] GameObject menu;
    [SerializeField] GameObject sphere;
    [SerializeField] GameObject LineObj;
    LineRenderer lr;

    enum PointerVer
    {
        walk,           //平常時メニュー画面時ともにレーザーもポインタも表示されてる
        shooting,       //平常時はレーザーオン、ポインタは表示しない、メニューを開いてるとポインタが表示される
        gunsword,       //平常時はレーザーもポインタもオフ、メニュー画面が開いてるとポインタとレーザーがオン 
        battle
    }
    [SerializeField] PointerVer pointerver;

	void Start () {
        if(menu == null)
        {
            menu = GameObject.Find("Menu");
        }

        lr = LineObj.GetComponent<LineRenderer>();

    }

    
    void Update()
    {
        if (pointerver == PointerVer.walk)
        {
            sphere.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            //レーザーはUI操作に従う
        }

        else if (pointerver == PointerVer.shooting)
        {
            if (menu.activeSelf)
            {
                sphere.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

                lr.startWidth = 0.008f;
                lr.endWidth = 0.005f;
            }
            else
            {
                sphere.transform.localScale = new Vector3(0, 0, 0f);

                lr.startWidth = 0.008f;
                lr.endWidth = 0.005f;
            }
        }

        else if (pointerver == PointerVer.gunsword)
        {
            if (menu.activeSelf)
            {
                sphere.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

                lr.startWidth = 0.008f;
                lr.endWidth = 0.005f;
            }
            else
            {
                sphere.transform.localScale = new Vector3(0, 0, 0f);

                lr.startWidth = 0f;
                lr.endWidth = 0f;
            }
        }

        else if (pointerver == PointerVer.battle)
        {
            if (menu.activeSelf)
            {
                lr.startWidth = 0f;
                lr.endWidth = 0f;
            }
            else
            {
                lr.startWidth = 0.008f;
                lr.endWidth = 0.005f;
            }
        }
    }

    
}
