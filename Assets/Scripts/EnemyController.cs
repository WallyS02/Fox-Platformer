using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float startingPositionX;
    private bool isFacingRight = false;
    public float moveSpeed = 0.1f;
    private Animator animator;
    private Transform target;
    public float moveRange = 1.0f;
    public bool isMovingRight = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            /*target = other.transform;
            Debug.Log(target);*/
            if(other.gameObject.transform.position.y > this.transform.position.y)
            {
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }
    }

    /*private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = null;
            Debug.Log(target);
        }
    }*/

    private void Update()
    {
 /*       if(target != null)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        }*/

        if(isMovingRight)
        {
            if (this.transform.position.x <= startingPositionX + moveRange)
            {
                MoveRight();
            } else
            {
                isMovingRight = false;
                Flip();
            }
        } else
        {
            if (this.transform.position.x >= startingPositionX - moveRange)
            {
                MoveLeft();
            }
            else
            {
                isMovingRight = true;
                Flip();
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        startingPositionX = transform.position.x;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * (-1);
        transform.localScale = theScale;
    }

    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
}
