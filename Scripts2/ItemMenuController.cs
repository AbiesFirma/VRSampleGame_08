using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIアイテムのメニューを管理するクラス
/// </summary>
public class ItemMenuController : MonoBehaviour {

    List<ItemData> itemDataBase = new List<ItemData>();                  //アイテムデータベースリスト
    Dictionary<int, int> itemBag = new Dictionary<int, int>();           //アイテムIDと所持数のディクショナリー
    Dictionary<int, ItemData> itemBagData = new Dictionary<int, ItemData>();   //アイテムIDとアイテムデータのディクショナリー


    GameObject itemDataBaseObject;

    [SerializeField] GameObject itemMenuTextPartsPrefab;
    [SerializeField] GameObject itemCanvasContent;

    [SerializeField] GameObject yajirushi;        //メニューの上部に表示する矢印
    [SerializeField] float yajirushiTime = 5.0f;  //矢印を表示する時間
    float timer;

    void Start () {
        //データベースからアイテムのデータを取得
        itemDataBaseObject = GameObject.Find("ItemDataBase");
        itemDataBase = itemDataBaseObject.GetComponent<ItemDataBase>().items;

        itemBag = ItemBag.itemBag;
        itemBagData = ItemBag.itemBagData;

        
        //所持しているアイテムを表示
        foreach (int key in itemBag.Keys)
        {
            if (itemBag[key] != 0)
            {
                //UIアイテム欄に持っているものを表示
                var itemBar = Instantiate(itemMenuTextPartsPrefab, itemCanvasContent.transform.position, itemCanvasContent.transform.rotation);
                //Contentの子に
                itemBar.transform.parent = itemCanvasContent.transform;
                //変更する画像、名前、所持数をそれぞれ取得
                var image = itemBar.transform.Find("ItemImageSprite").gameObject.GetComponent<Image>();
                var name = itemBar.transform.Find("ItemNameText").gameObject.GetComponent<Text>();
                var num = itemBar.transform.Find("ItemNumberText").gameObject.GetComponent<Text>();

                //アイテムのデータをItemBagから取得
                var data = itemBagData[key];

                //それぞれをデータベースの情報とバックの情報に書き換え
                image.sprite = data.itemIcon;
                name.text = string.Format("{0}", data.itemName);
                num.text = string.Format("{0}", itemBag[key]);
                //PanelのItemコンポーネントにアイテムテータを書き込み
                itemBar.GetComponent<Item>().thisItemData = data;
            }
        }
    }

    //アクティブになるたびに矢印を表示し指定秒数で消える
    private void OnEnable()
    {
        if(!yajirushi.activeSelf)
        {
            yajirushi.SetActive(true);
        }
    }

    private void Update()
    {
        if(yajirushi.activeSelf)
        {
            timer += Time.deltaTime;

            if(timer > yajirushiTime)
            {
                timer = 0.0f;
                yajirushi.SetActive(false);
            }
        }
    }

}
