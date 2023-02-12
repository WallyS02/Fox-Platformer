using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoxController : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float jumpForce = 6.0f;
    private Rigidbody2D rigidBody;
    private Animator animator;
    public LayerMask groundLayer;
    public float rayLength = 0.5f;
    private bool isFacingRight = true;
    private bool isWalking;
    public Vector2 startPosition;
    //public int keyFound = 0;
    public int keysNumber = 3;
    public AudioClip cherrySound;
    public AudioClip doorSound;
    public AudioClip enemySound;
    public AudioClip deathSound;
    public AudioClip keySound;
    public AudioClip heartSound;
    public AudioClip jumpSound;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            isWalking = false;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                isWalking = true;
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                if (!isFacingRight)
                {
                    Flip();
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                isWalking = true;
                transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                if (isFacingRight)
                {
                    Flip();
                }
            }

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            //Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1f, false);
        }
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetBool("isWalking", isWalking);
        animator.SetFloat("yVelocity", rigidBody.velocity.y);
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        source = GetComponent<AudioSource>();
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    void Jump()
    {
        if(IsGrounded())
        {
            source.PlayOneShot(jumpSound, AudioListener.volume);
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * (-1);
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            source.PlayOneShot(cherrySound, AudioListener.volume);
            GameManager.instance.AddPoints(1);
            AutoDestruct(other);
        }
        else if (other.CompareTag("WinningDoor")) 
        {
            if(GameManager.instance.keysFound == keysNumber)
            {
                source.PlayOneShot(doorSound, AudioListener.volume);
                for(int i = GameManager.instance.currentHearts; i < 6; i++)
                    GameManager.instance.AddHeart();
                GameManager.instance.LevelCompleted();
            } else
            {
                Debug.Log("Find all keys!");
            }
            
        }
        else if(other.CompareTag("Enemy"))
        {
            if(transform.position.y > other.gameObject.transform.position.y)
            {
                source.PlayOneShot(enemySound, AudioListener.volume);
                GameManager.instance.AddEnemies();
                GameManager.instance.AddPoints(3);
            }
            else
            {
                Death();
            }
        }
        else if(other.CompareTag("Key"))
        {
            source.PlayOneShot(keySound, AudioListener.volume);
            string name = other.name;
            int nr = 0;
            switch (name)
            {
                case "Gem (0)":
                    nr = 0;
                    break;
                case "Gem (1)":
                    nr = 1;
                    break;
                case "Gem (2)":
                    nr = 2;
                    break;
            }
            GameManager.instance.AddKeys(nr);
            AutoDestruct(other);
        }
        else if(other.CompareTag("Heart"))
        {
            source.PlayOneShot(heartSound, AudioListener.volume);
            GameManager.instance.AddHeart();
            AutoDestruct(other);
        }
        else if(other.CompareTag("FallLevel"))
        {
            Death();
        }
        else if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
    }

    void Death()
    {
        source.PlayOneShot(deathSound, AudioListener.volume);
        GameManager.instance.RemoveHeart();
        if (GameManager.instance.currentHearts == 0)
        {
            SceneManager.LoadScene("Lose");
        }
        else
        {
            this.transform.position = startPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        transform.SetParent(null);
    }

    void AutoDestruct(Collider2D other)
    {
        Animator bonusAnimator;
        bonusAnimator = other.GetComponent<Animator>();
        bonusAnimator.SetBool("destroy", true);
    }
}
