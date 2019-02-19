using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトを点滅させるためのクラス
/// </summary>
public class tenmetsu : MonoBehaviour {

    MeshRenderer meshrender;
    [SerializeField] float speed = 0.3f;
    float _alpha;

    [SerializeField] bool setMat = false;
    [SerializeField] Material mat;

	// Use this for initialization
	void Start () {
        meshrender = GetComponent<MeshRenderer>();

        if(setMat)
        {
            meshrender.material = mat;
        }

        _alpha = meshrender.material.color.a;     
        
	}
	
	// Update is called once per frame
	void Update () {
        float current_a = meshrender.material.color.a;

        if (current_a > 0)
        {
            meshrender.material.color = new Color(meshrender.material.color.r,
                                                  meshrender.material.color.g,
                                                  meshrender.material.color.b,
                                                  meshrender.material.color.a - (0.1f * speed));
        }
        else
        {
            meshrender.material.color = new Color(meshrender.material.color.r,
                                                  meshrender.material.color.g,
                                                  meshrender.material.color.b,
                                                  _alpha);
        }
                
       
	}
}
