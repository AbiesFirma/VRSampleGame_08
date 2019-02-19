using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全てのアイテムのデータベース
/// </summary>
public class ItemDataBase : MonoBehaviour {

    public List<ItemData> items = new List<ItemData>();
    public static bool itemSet = false;

    /*
    ○ListID:名前
    0:石, 1:ヒールポッド小, 2:ヒールポッド中,

    ○引数
    (名前, ID, 説明文, アイコン画像, アイテムタイプ, 攻撃力, 防御力, スピード, 回復量)
    */


    private void Awake()
    {
        if (!itemSet)
        {
            items.Add(new ItemData("石", 0, "ただの石", Resources.Load("ItemSprite/Stone1", typeof(Sprite)) as Sprite, ItemData.ItemType.Other,
                                   1, 0, 0, 0, Resources.Load("ItemGrabPrefab/Stone1") as GameObject));
            items.Add(new ItemData("ヒールポッド小", 1, "HPを500回復する", Resources.Load("ItemSprite/HealPod1", typeof(Sprite)) as Sprite, ItemData.ItemType.Item,
                                   0, 0, 0, 500, Resources.Load("ItemGrabPrefab/HealPod1", typeof(GameObject)) as GameObject));
            items.Add(new ItemData("ヒールポッド中", 2, "HPを1000回復する", Resources.Load("ItemSprite/HealPod2", typeof(Sprite)) as Sprite, ItemData.ItemType.Item,
                                   0, 0, 0, 1000, Resources.Load("ItemGrabPrefab/HealPod2", typeof(GameObject)) as GameObject));

            //リストをID昇順に並べ替え
            items.Sort((a, b) => a.itemID - b.itemID);

            //itembagのDictionaryにアイテムIDを追加しvalueを０に
            //itembagのDictionaryにアイテムIDとそのデータを
            for (int i = 0; i < items.Count; i++)
            {
                ItemBag.itemBag.Add(items[i].itemID, 0);
                ItemBag.itemBagData.Add(items[i].itemID, items[i]);
            }

            //アイテムセット完了
            itemSet = true;
        }
        
    }

    
}
