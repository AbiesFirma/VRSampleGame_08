using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの持っているアイテムのバックについてのクラス
/// </summary>
public class ItemBag : MonoBehaviour {

    public static Dictionary<int, int> itemBag = new Dictionary<int, int>();      //<アイテムID,　所持数>
    public static Dictionary<int, ItemData> itemBagData = new Dictionary<int, ItemData>();      //<アイテムID,　そのデータ>

    //アイテムのID、名前、所持数のリスト
    [SerializeField] List<int> itemIDInBag = new List<int>();
    [SerializeField] List<string> itemNameInBag = new List<string>();
    [SerializeField] List<int> itemNumberInBag = new List<int>();

    [SerializeField] GameObject itemDataBase;
    bool itemSetEnd = false;


    void Start()
    {
        //ItemDataBaseのAwakeでのアイテムバックへのセットが終わってたらStart内で中身を設定
        //終わってなかったらアップデートの最初で設定
        
        if (ItemDataBase.itemSet)
        {
            ItemSet();
        }
    }

    void Update()
    {
        if (!itemSetEnd)
        {
            ItemSet();
        }
        
           
    }



    void ItemSet()
    {
        //デバッグ用の初期所持アイテム
        itemBag[0] = 5;
        itemBag[1] = 10;
        itemBag[2] = 0;

        foreach (int key in itemBag.Keys)
        {
            if (itemBag[key] != 0)
            {
                //持っている（所持数が０でない）アイテムの名前とID,所持数をそれぞれリストに登録
                itemIDInBag.Add(key);
                itemNameInBag.Add(itemBagData[key].itemName);
                itemNumberInBag.Add(itemBag[key]);

            }
        }
        

        itemSetEnd = true;
    }
}
