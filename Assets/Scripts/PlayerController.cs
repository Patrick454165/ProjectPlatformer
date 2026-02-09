using System;
using System.Runtime.CompilerServices;
using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] myAudio;
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerInput pi;
    private Vector2 moveInput;
    public float movespeed = 8f;
    private bool facingRight=true;
    private bool jumpPressed=false;

    private float health = 100;
    public BombController bomb;
    [Tooltip("This is how much we slow them down by in the air")]
    public float airMultiplier; 
    public float groundMultiplier = 1f;

    public float jumpForce=10f;
    [Header("Ground Check")]
    public Transform groundCheck;
    public Transform shadowDot;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.25f;
    public bool isGrounded = true;
    public float fallGravityScale = 2f; //update gravity to 200%
    public bool wasGrounded;

    [Header("Shooting")]
    public Transform firePoint;
    public GameObject fire; 
    public float attackRate;
    public float nextAttackTime;

    [Header("HealthDisplay")]
    public UnityEngine.UI.Image heart;
    public UnityEngine.UI.Image heart1;
    public UnityEngine.UI.Image heart2;
    public UnityEngine.UI.Image heart3;
    public Sprite fullHeart;
    public Sprite noHeart;
    public UnityEngine.UI.Image[] hearts;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pi = GetComponent<PlayerInput>();
        hearts[0]=heart;
        hearts[1]=heart1;
        hearts[2]=heart2;
        hearts[3]=heart3;
    }

    void OnMove(InputValue movementValue)
    {
        moveInput = movementValue.Get<Vector2>();
        
    }

    void OnJump(InputValue movementValue)
    {
        
        jumpPressed = true;
    }

    // void OnAttack(InputValue attackValue)
    // {
    //     anim.SetTrigger("isShooting");
    // }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wasGrounded = isGrounded;
        checkGrounded();
        if(!wasGrounded && isGrounded)
        {
            groundMultiplier=2f;
        }else if (wasGrounded && groundMultiplier > 1f)
        {
            groundMultiplier-=.05f;
        }
        //one way to move: translate
        //transform.Translate(moveInput*Vector2.right * Time.deltaTime);
        if(moveInput.x < 0 && facingRight)
        {
            facingRight=false;
            Flip();
        }
        else if(moveInput.x > 0 && !facingRight)
        {
            facingRight=true;
            Flip();
        }
        //event pulling
        float isAttackHeld = pi.actions["Attack"].ReadValue<float>();
        if (isAttackHeld>0 && Time.time >= nextAttackTime)
        {
            anim.SetTrigger("isShooting");
            Instantiate(fire, firePoint.position, facingRight ? firePoint.rotation : Quaternion.Euler(0, 180, 0));
            nextAttackTime=Time.time + attackRate;
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput.x * movespeed; //max speed
        float speedDiff = targetSpeed - rb.linearVelocity.x; //how far from speed max.
        //float accelRate = movespeed;
        float accelRate = isGrounded ? movespeed * groundMultiplier : movespeed * airMultiplier;
        float movement = speedDiff*accelRate; //how hard to push player
        rb.AddForce(Vector2.right*movement);
        anim.SetFloat("speed", Math.Abs(rb.linearVelocity.x));

        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
            jumpPressed=false;
            isGrounded = false;
        }
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = fallGravityScale;
        }
        else
        {
            rb.gravityScale = 1;
        }

        //check if we're hitting the ground
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 50f, groundLayer);
        if (hit)
        {
            shadowDot.position = hit.point;
            shadowDot.gameObject.SetActive(true);
        }

        Debug.DrawRay(rb.position, Vector2.down, Color.red, 1f);
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; //inverts x value
        transform.localScale = theScale; //applies to system.
    }

    void checkGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,groundLayer);
        shadowDot.gameObject.SetActive(!isGrounded); //only show if not grounded
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }

    public Vector2 getDirection()
    {
        if (facingRight)
        {
            return Vector2.right;
        }
        else
        {
            return Vector2.left;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("boundary"))
        {
            Restart();
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            health -= 25;
            if(health <= 0)
            {
                Death();
            }
            else
            {
                audioSource.clip = myAudio[1];
                audioSource.Play();
                foreach(UnityEngine.UI.Image cHeart in hearts)
                {
                    if(cHeart.sprite == fullHeart)
                    {
                        cHeart.sprite = noHeart;
                        break;
                    }
                }
            }
        }
        if (collision.gameObject.CompareTag("console"))
        {
            collision.gameObject.GetComponent<AudioSource>().Play();
            bomb.Disarm();
        }
    }

    public void Restart()
    {
        
        
        SceneManager.LoadScene(0);
    }

    public void Death()
    {
        foreach(UnityEngine.UI.Image cHeart in hearts)
        {
            cHeart.sprite = noHeart;
        }
        pi.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetBool("isDead", true);
        GameManager.instance.DecreaseLives();
        Debug.Log("Lives: " + GameManager.instance.GetLives());
        Invoke("Restart", 2f);
        audioSource.clip = myAudio[0];
        audioSource.Play();
    }

    public void Reset()
    {
        Invoke("Restart", 2f);
    }

    
}
