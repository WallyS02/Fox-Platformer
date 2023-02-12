using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformUpController : MonoBehaviour
{
    public float startingPositionY;
    public float moveSpeed = 0.3f;
    public float moveRange = 1.0f;
    public bool isMovingDown = false;

    private void Update()
    {
        /*       if(target != null)
               {
                   float step = moveSpeed * Time.deltaTime;
                   transform.position = Vector2.MoveTowards(transform.position, target.position, step);
               }*/

        if (isMovingDown)
        {
            if (this.transform.position.y <= startingPositionY + moveRange)
            {
                MoveUp();
            }
            else
            {
                isMovingDown = false;
            }
        }
        else
        {
            if (this.transform.position.y >= startingPositionY - moveRange)
            {
                MoveDown();
            }
            else
            {
                isMovingDown = true;
            }
        }
    }

    private void Awake()
    {
        startingPositionY = transform.position.y;
    }

    void MoveUp()
    {
        transform.Translate(0.0f, moveSpeed * Time.deltaTime, 0.0f, Space.World);
    }

    void MoveDown()
    {
        transform.Translate(0.0f, -moveSpeed * Time.deltaTime, 0.0f, Space.World);
    }
}
