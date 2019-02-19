using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoActiveFalse : MonoBehaviour {

    [SerializeField] float lifetime = 5.0f;
    
	void Update () {
        if(this.gameObject.activeSelf)
        {
            StartCoroutine("ActiveFalse", lifetime);
        }
	}

    public IEnumerator ActiveFalse(float lifetime)
    {        
        //timeの時間まつ
        yield return new WaitForSeconds(lifetime);
        //自分を非アクティブに
        this.gameObject.SetActive(false);
    }
}
