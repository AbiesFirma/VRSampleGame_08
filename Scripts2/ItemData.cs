using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムデータ型を定義するクラス
/// </summary>

[System.Serializable]

public class ItemData
{

    public string itemName;   //名前
    public int itemID;        //リスト内でのID
    public string itemDesc;   //アイテム説明
    public Sprite itemIcon;    //アイテムのアイコン画像

    public enum ItemType    //アイテムの種類
    {
        Item,
        Weapon,
        Armor,
        Other
    }
    public ItemType itemType;

    public int attack;    //攻撃力
    public int diffence;   //防御力
    public int speed;   //スピード
    public int healPower;    //回復力

    public GameObject grabItem;    //掴んだ時に表示されるオブジェクト



    //引数設定
    public ItemData(string name, int id, string desc, Sprite icon, ItemType type,
                    int atk, int dif, int spd, int heal, GameObject grab)
    {
        itemName = name;
        itemID = id;
        itemDesc = desc;
        itemIcon = icon;
        itemType = type;

        attack = atk;
        diffence = dif;
        speed = spd;
        healPower = heal;

        grabItem = grab;

    }


}
