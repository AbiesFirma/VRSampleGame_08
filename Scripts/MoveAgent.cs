using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵ををナビメッシュにより徘徊させるためのクラス(指定範囲内からランダムに選択して移動、地形によっては使用不可)
/// </summary>

[RequireComponent(typeof(NavMeshAgent))]

public class MoveAgent : MonoBehaviour {

    protected NavMeshAgent agent;
    protected Vector3 nextPoint;

    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        GotoNextPoint();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(agent.remainingDistance < 0.5f )
        {
            GotoNextPoint();
        }
	}

    protected void GotoNextPoint()
    {
        nextPoint = new Vector3(Random.Range(-20.0f, 20f), 0.0f, Random.Range(-20.0f, 20.0f));
        agent.SetDestination(nextPoint);
    }
}
