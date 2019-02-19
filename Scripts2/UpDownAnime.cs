using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 上下するアニメーションクラス
/// </summary>
public class UpDownAnime : MonoBehaviour {

    [SerializeField] float speed = 0.5f;
    float time;
    float length;

	void Start () {
        time = 0.0f;
        length = 0.0f;
	}
		
	void Update () {
        time += Time.deltaTime;
        if (time <= 0.5f)
        {
            this.gameObject.transform.Translate(0, -0.5f * Time.deltaTime * speed, 0);
            length += 0.5f * Time.deltaTime * speed;
        }
        else
        {
            this.gameObject.transform.Translate(0, length, 0);
            time = 0.0f;
            length = 0.0f;
        }
	}
}
