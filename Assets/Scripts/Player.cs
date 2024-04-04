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
    private bool facingRight;
    private Rigidbody2D otherPlayer;
    private int forward = 1;
    private int back = -1;
    private GameObject[] players;
    private GameObject halfway;
    private Vector3 scale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        players = GameObject.FindGameObjectsWithTag("Player");
        halfway = GameObject.Find("Halfway");
        scale = transform.localScale;
        if (halfway.transform.position.x < transform.position.x)
        {
            facingRight = false;
            forward = -1;
            back = 1;
            if (scale.x > 0)
            {
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
    }

    void Movement()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        Debug.Log(moveHorizontal);

        if (isGrounded && !anim.GetBool("isCrouching") && anim.GetBool("canAct"))
        {
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                anim.SetBool("isWalkingRight", true);
            }
            else if (Input.GetAxisRaw("Horizontal") == -1)
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

    [Command]
    void ShootFireball()
    {
        if (Input.GetButtonDown("Special") && Input.GetAxisRaw("Horizontal") == 0 && anim.GetBool("canAct") && isGrounded && isLocalPlayer)
        {
            fireballs = GameObject.FindGameObjectsWithTag("Projectile").Length;
            if (fireballs == 0)
            {
                anim.SetTrigger("Fireball");
                StartCoroutine(Delay(0.15f));
                float xDisplacement;
                if (facingRight)
                {
                    xDisplacement = 1.1f;
                }
                else
                {
                    xDisplacement = -1.1f;
                }
                spawnPoint = new Vector3(transform.position.x + xDisplacement, transform.position.y + 0.35f, transform.position.z);
                GameObject fireball = Instantiate(NetworkManager.singleton.spawnPrefabs[0], spawnPoint, Quaternion.identity);
                if (facingRight)
                {
                    fireball.GetComponent<Fireball>().right = false;
                }
                else
                {
                    fireball.GetComponent<Fireball>().right = true;
                }
                NetworkServer.Spawn(fireball);
            }
        }
    }

    IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    void Uppercut()
    {
        float horizontalSpeed = 5.0f;
        float jumpHeight = 65.0f;

        jump = new Vector3(1 * horizontalSpeed, jumpHeight, 0.0f);

        if (Input.GetButtonDown("Special") && Input.GetAxisRaw("Horizontal") == forward && anim.GetBool("canAct") && isGrounded)
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

    void FacingDirection()
    {
        scale = transform.localScale;

        if (otherPlayer != null)
        {
            if (otherPlayer.transform.position.x > transform.position.x && !facingRight)
            {
                facingRight = true;
                forward = 1;
                back = -1;
                if (scale.x < 0)
                {
                    scale.x *= -1;
                    transform.localScale = scale;
                }
            }
            else if (otherPlayer.transform.position.x < transform.position.x && facingRight)
            {
                facingRight = false;
                forward = -1;
                back = 1;
                if (scale.x > 0)
                {
                    scale.x *= -1;
                    transform.localScale = scale;
                }
            }
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            Movement();
            Jump();
            CrouchAttack();
            StandAttack();
            JumpAttack();
            ShootFireball();
            Uppercut();
            FacingDirection();

            if (rb.velocity.y < 0 && !isGrounded)
            {
                anim.SetBool("isFalling", true);
                anim.SetBool("isJumping", false);
            }

            if (players.Length < 2)
            {
                players = GameObject.FindGameObjectsWithTag("Player");
                foreach (var x in players)
                {
                    Debug.Log(x.ToString());
                }
            }
            else if (otherPlayer == null)
            {
                foreach (var player in players)
                {
                    if (player != gameObject)
                    {
                        otherPlayer = player.GetComponent<Rigidbody2D>();
                        Debug.Log(otherPlayer);
                    }
                }
            }
        }

    }

    void FixedUpdate()
    {
        // Debug.Log(anim.GetBool("isFalling"));
    }
}
