using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 話しかけたりボタン押したりからプレーヤーを指定位置に転送する装置のくらす
/// </summary>
public class EquipTelepo : MonoBehaviour {
    
    [SerializeField] Transform to_telepo_pos;
    [SerializeField] GameObject Player;

    private void Start()
    {
        if (Player == null)
        {
            Player = GameObject.Find("PlayerRoot 1");
        }
    }


    public void Equiptelepo()
    {
        Player.transform.position = new Vector3 (to_telepo_pos.position.x, to_telepo_pos.position.y +1.0f, to_telepo_pos.position.z);
    }

}
