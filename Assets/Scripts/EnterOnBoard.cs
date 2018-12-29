using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterOnBoard : MonoBehaviour {

    [SerializeField] GameObject Board;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Board.SetActive(true);
        }
            
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Board.SetActive(false);
        }
    }

}
