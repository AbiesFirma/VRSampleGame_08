using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeStandController : MonoBehaviour {

    [SerializeField] GameObject GazeOpenItem;
    public GameObject gazeOpenItem { get; private set;}

	void Start () {
        gazeOpenItem = GazeOpenItem;
        
	}
	
	void Update () {
		
	}
}
