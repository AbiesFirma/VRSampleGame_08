using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMover : MonoBehaviour {

	[SerializeField] float speed = 1.0f;
    [SerializeField] float upMoveLength = 20.0f;
    Vector3 currentPos;
    Vector3 movePos;
    bool move;

	void Start () {
        currentPos = transform.position;
        movePos = new Vector3(transform.position.x, transform.position.y + upMoveLength, transform.position.z);
    }

    private void Update()
    {
        Move();
    }


    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePos, speed * Time.deltaTime);
    }
}
