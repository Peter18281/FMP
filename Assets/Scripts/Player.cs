using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    private float moveSpeed = 70f;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private Vector3 jump;
    private float jumpForce = 2.0f;
    private Animator anim;
    [SerializeField]
    private GameObject fireballObject;
    private int fireballs;
    private Vector3 spawnPoint;
    private bool airAttack;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Movement()
    {
        if (isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");

            if (isGrounded && !anim.GetBool("isCrouching") && anim.GetBool("canAct"))
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    anim.SetBool("isWalkingRight", true);
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    anim.SetBool("isWalkingLeft", true);
                }
                else
                {
                    anim.SetBool("isWalkingLeft", false);
                    anim.SetBool("isWalkingRight", false);
                }
                Vector3 movement = new Vector3(moveHorizontal * moveSpeed * 0.0001f, 0, 0);
                transform.position = transform.position + movement;
            }
            if (Input.GetAxisRaw("Vertical") == -1)
            {
                anim.SetBool("isCrouching", true);
            }
            else
            {
                anim.SetBool("isCrouching", false);
            }

        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
            airAttack = false;
            anim.SetBool("isFalling", false);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

    void Fireball()
    {
        if (Input.GetButtonDown("Special") && Input.GetAxisRaw("Horizontal") == 0 && anim.GetBool("canAct") && isGrounded)
        {
            fireballs = GameObject.FindGameObjectsWithTag("Projectile").Length;
            if (fireballs == 0)
            {
                anim.SetTrigger("Fireball");
                StartCoroutine(SpawnFireball());
            }
        }
    }

    IEnumerator SpawnFireball()
    {
        yield return new WaitForSeconds(0.15f);
        spawnPoint = new Vector3(transform.position.x + 1.1f, transform.position.y + 0.35f, transform.position.z);
        Instantiate(fireballObject, spawnPoint, Quaternion.identity);
    }

    void Uppercut()
    {
        float horizontalSpeed = 5.0f;
        float jumpHeight = 65.0f;

        jump = new Vector3(1 * horizontalSpeed, jumpHeight, 0.0f);

        if (Input.GetButtonDown("Special") && Input.GetAxisRaw("Horizontal") == 1 && anim.GetBool("canAct") && isGrounded)
        {
            anim.SetTrigger("Uppercut");
            rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
        }
    }

    void Jump()
    {
        if (isLocalPlayer)
        {
            float horizontalSpeed = 25.0f;
            float jumpHeight = 65.0f;
            float jumpHorizontal = Input.GetAxisRaw("Horizontal");

            jump = new Vector3(jumpHorizontal * horizontalSpeed, jumpHeight, 0.0f);

            if (Input.GetButtonDown("Jump") && isGrounded && anim.GetBool("canAct"))
            {
                anim.SetBool("isJumping", true);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0.0f;
                rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    void CrouchAttack()
    {
        if (anim.GetBool("isCrouching") && Input.GetButtonDown("Attack") && anim.GetBool("canAct") && isGrounded)
        {
            anim.SetTrigger("Crouch Attack");
        }
    }

    void StandAttack()
    {
        if (!anim.GetBool("isCrouching") && Input.GetButtonDown("Attack") && anim.GetBool("canAct") && isGrounded)
        {
            anim.SetTrigger("Stand Attack");
        }
    }

    void JumpAttack()
    {
        if (Input.GetButtonDown("Attack") && !isGrounded && !airAttack)
        {
            airAttack = true;
            anim.SetTrigger("Jump Attack");
        }
    }

    void Update()
    {
        Movement();
        Jump();
        CrouchAttack();
        StandAttack();
        JumpAttack();
        Fireball();
        Uppercut();

        if (rb.velocity.y < 0 && !isGrounded)
        {
            anim.SetBool("isFalling", true);
            anim.SetBool("isJumping", false);
        }
    }

    void FixedUpdate()
    {
        // Debug.Log(isGrounded);
    }
}
