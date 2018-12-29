using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class MoveAgent_human : MonoBehaviour {

    NavMeshAgent agent;                      //ナビメッシュエージェント
    [SerializeField] Transform[] movePoint;   //移動先を設定する配列
    Vector3 nextPoint;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        nextPoint = transform.position;
        GotoNextPoint();       
    }
	
	void Update ()
    {
        //目的地に近づいたら
	    if( agent.remainingDistance < 1.0f)
        {            
            GotoNextPoint();
        }
	}
    
    //次の目的地を候補からランダムに選択し設定する
    void GotoNextPoint()
    {
        var movePoint_num = Random.Range(0, movePoint.Length);
        Vector3 nextPoint = movePoint[movePoint_num].position;

        agent.SetDestination(nextPoint);
    }
    
}
