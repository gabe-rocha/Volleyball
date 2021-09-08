using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    [SerializeField] private float leftBound = -25f, rightBound = 187f;
    [SerializeField] private float speed = 1f;

    void FixedUpdate()
    {
        transform.position += Vector3.left * speed * 1f/transform.position.y;

        if(transform.position.x < leftBound)
        {
            var newPos = transform.position;
            newPos.x = rightBound;
            transform.position = newPos;
        }
    }
}
