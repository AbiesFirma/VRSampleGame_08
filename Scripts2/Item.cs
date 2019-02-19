using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム自身とアイテムメニューのUIがもつ
/// </summary>
public class Item : MonoBehaviour {

    public ItemData thisItemData;    //このアイテムのItemData
    [SerializeField] string ItemName;     //アイテム名
    ItemData.ItemType itemtype;     //アイテムのタイプ

    [SerializeField] bool grabObjectMode = false;    //掴んで発動させるアイテム自体のコンポーネントな場合にTrue,UIはFalse
    Vector3 rayHitPoint;
    bool grabMode;
    GameObject player;

   

	void Start () {        
        //thisItemData = itemDataBase.GetComponent<ItemDataBase>().items[listValueOnItemDataBase];
        ItemName = thisItemData.itemName;
        
        if(grabObjectMode)
        {
            player = GameObject.FindWithTag("playerRoot");
        }

        itemtype = thisItemData.itemType;

        var c = GetComponent<Item>();
	}
	
	void Update () {

        //UIでなくアイテム自体の場合
        if (grabObjectMode)
        {
            rayHitPoint = player.GetComponent<CharacterMoveOrder>().hitpoint;
            transform.position = rayHitPoint;

            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonUp(1))
            {
                var releaseObj = player.GetComponent<CharacterMoveOrder>().hitObj;

                if (itemtype == ItemData.ItemType.Item)
                {
                    if (releaseObj.layer == 25) //キャラ
                    {
                        //キャラのBattleCharacterStateにSendMessage,引数はItemData
                        releaseObj.SendMessage("UseItem", thisItemData, SendMessageOptions.RequireReceiver);
                        Destroy(gameObject, 1.0f);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else if (itemtype == ItemData.ItemType.Other)
                {
                    GetComponent<Item>().enabled = false;
                }
                else
                {
                    Destroy(gameObject);
                }

                player.GetComponent<CharacterMoveOrder>().grabMode = false;
            }
        }
	}
}
