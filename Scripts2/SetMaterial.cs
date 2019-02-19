using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム開始時にマテリアルを設定するためのクラス
/// </summary>
public class SetMaterial : MonoBehaviour {

    MeshRenderer meshrender;
    //Renderer renderer;
    [SerializeField] Material mat;

    void Start () {
        meshrender = GetComponent<MeshRenderer>();
        meshrender.material = mat;
    }
	
	void Update () {
		
	}
}
