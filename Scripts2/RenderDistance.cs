using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー（カメラ）との距離によってレンダーを消すクラス（主に敵のレンダーを消すため）
/// </summary>
public class RenderDistance : MonoBehaviour {

    [SerializeField] GameObject player;
    Vector3 player_pos;
    [SerializeField] Renderer ren;
    [SerializeField] float renderingDis = 100.0f;
    int hp;

	void Start () {

        if (player == null)
        {
            player = GameObject.Find("OVRCameraRig");   //どのシーンでも名前の変わらないOVRCameraRigを探す            
        }
        if (ren == null)
        {
            ren = GetComponent<Renderer>();
        }

    }
	
	void Update () {

        player_pos = player.transform.position;
        float dis = Vector3.Distance(this.transform.position, player_pos);
        
        if(dis >= renderingDis)
        {
            ren.enabled = false;
        }
        else
        {
            ren.enabled = true;
        }


        if(this.gameObject.tag == "enemy")
        {
            hp = GetComponent<Enemy>().currentHp;
            if(hp <= 0)
            {
                ren.enabled = false;
            }
        }
	}
}
