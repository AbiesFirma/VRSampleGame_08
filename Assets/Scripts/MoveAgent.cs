using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class MoveAgent : MonoBehaviour {

    NavMeshAgent agent;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        GotoNextPoint();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
	}

    void GotoNextPoint()
    {
        var nextPoint = new Vector3(Random.Range(-20.0f, 20f), 0.0f, Random.Range(-20.0f, 20.0f));
        agent.SetDestination(nextPoint);
    }
}
