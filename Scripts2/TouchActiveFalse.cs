using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 触れたら消えて数秒後に再び表示されるオブジェクトのクラス
/// </summary>
public class TouchActiveFalse : MonoBehaviour {

    Collider this_collider;
    Renderer this_renderer;
    [SerializeField] float reActiveTime = 5.0f;
    float Timer;

	// Use this for initialization
	void Start () {
        this_collider = GetComponent<Collider>();
        this_renderer = GetComponent<Renderer>();
        Timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        Timer += Time.deltaTime;

		if((!this_collider.enabled || !this_renderer.enabled) && reActiveTime <= Timer) 
        {
            this_collider.enabled = true;
            this_renderer.enabled = true;
        }
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("player"))
        {
            //this.gameObject.SetActive(false);
            this_collider.enabled = false;
            this_renderer.enabled = false;
            Timer  = 0.0f;
        }
    }
}
