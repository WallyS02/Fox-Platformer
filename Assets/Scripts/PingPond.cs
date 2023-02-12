using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPond : MonoBehaviour
{
    private float speed = 0.5f;
    private Transform target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = other.transform;
            Debug.Log(target);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = null;
            Debug.Log(target);
        }
    }

    private void Update()
    {
        if(target != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        }
    }
}
