using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformRigthController : MonoBehaviour
{
    public float startingPositionX;
    public float moveSpeed = 0.3f;
    public float moveRange = 1.0f;
    public bool isMovingRight = false;

    private void Update()
    {

        if (isMovingRight)
        {
            if (this.transform.position.x <= startingPositionX + moveRange)
            {
                MoveRight();
            }
            else
            {
                isMovingRight = false;
            }
        }
        else
        {
            if (this.transform.position.x >= startingPositionX - moveRange)
            {
                MoveLeft();
            }
            else
            {
                isMovingRight = true;
            }
        }
    }

    private void Awake()
    {
        startingPositionX = transform.position.x;
    }

    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
}
