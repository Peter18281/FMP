using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflinePlayer : MonoBehaviour
{
    private float moveSpeed = 70f;
    private float jumpForce = 2.0f;
    private Vector3 jump;
    private bool facingRight;
    private int forward = 1;
    private int back = -1;
    private Rigidbody2D rb;
    public bool isGrounded = true;
    public Animator anim;
    public GameObject fireballPrefab;
    public GameObject[] fireballs;
    private Vector3 spawnPoint;
    private bool airAttack;
    public OfflinePlayer otherPlayer;
    public Rigidbody2D otherPlayerRB;
    private GameObject halfway;
    private Vector3 scale;
    public int health = 100;
    [SerializeField]
    private GameObject pushBox;
    public int id;
    private bool fireballUsed = false;
    public bool invincible = false;
    [SerializeField]
    public List<GameObject> myHurtboxes;
    [SerializeField]
    private OfflineHUDManager hudManager;
    public bool player1 = false;
    private OfflineRoundManager roundManager;
    public Image round1;
    public Image round2;
    private SpriteRenderer spriteRenderer;
    public bool lost = false;
    private float moveHorizontal;
    private float moveVertical;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        halfway = GameObject.Find("Halfway");
        roundManager = GameObject.Find("GameManager").GetComponent<OfflineRoundManager>();
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
        if (player1)
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveVertical = Input.GetAxisRaw("Vertical");
        }
        else
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal2");
            moveVertical = Input.GetAxisRaw("Vertical2");
        }

        if (isGrounded && !anim.GetBool("isCrouching") && anim.GetBool("canAct"))
        {
            if (moveHorizontal == 1)
            {
                anim.SetBool("isWalkingRight", true);
            }
            else if (moveHorizontal == -1)
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
        if (moveVertical == -1)
        {
            anim.SetBool("isCrouching", true);
        }
        else
        {
            anim.SetBool("isCrouching", false);
        }
    }

    public void WinPose()
    {
        anim.SetTrigger("Won");
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

    public void GetHit(int damage, float pushback, bool isKnockdown, bool isBlocking, bool isSpecial, bool isCrouching, int attackHeight, float hitStun, float blockStun)
    {
        Vector3 pushbackForce = new Vector3(pushback * back, 0, 0);
        if (!invincible)
        {
            if (attackHeight == 0)
            {
                if (isCrouching && isBlocking)
                {
                    StartCoroutine(Inactionable(blockStun));
                    if (isSpecial)
                    {
                        health -= (int)Mathf.Round(damage / 10);
                    }
                    rb.AddForce(pushbackForce, ForceMode2D.Impulse);
                    anim.SetTrigger("Low Block");
                }
                else
                {
                    health -= damage;
                    if (isKnockdown)
                    {
                        anim.SetTrigger("Knockdown");
                    }
                    else
                    {
                        StartCoroutine(Inactionable(hitStun));
                        rb.AddForce(pushbackForce, ForceMode2D.Impulse);
                        if (!isCrouching)
                        {
                            anim.SetTrigger("Stand Hit");
                        }
                        else
                        {
                            anim.SetTrigger("Low Hit");
                        }
                    }
                }
            }

            if (attackHeight == 1)
            {
                if (isBlocking)
                {
                    StartCoroutine(Inactionable(blockStun));
                    if (isSpecial)
                    {
                        health -= (int)Mathf.Round(damage / 10);
                    }
                    rb.AddForce(pushbackForce, ForceMode2D.Impulse);
                    if (!isCrouching)
                    {
                        anim.SetTrigger("Stand Block");
                    }
                    else
                    {
                        anim.SetTrigger("Low Block");
                    }

                }
                else
                {
                    health -= damage;
                    if (isKnockdown)
                    {
                        isGrounded = false;
                        Vector3 knockdownForce = new Vector3(60f * back, 45f, 0f);
                        rb.AddForce(knockdownForce, ForceMode2D.Impulse);
                        anim.SetTrigger("Knockdown");
                    }
                    else
                    {
                        StartCoroutine(Inactionable(hitStun));
                        rb.AddForce(pushbackForce, ForceMode2D.Impulse);
                        if (!isBlocking && !isCrouching)
                        {
                            anim.SetTrigger("Stand Hit");
                        }
                        else
                        {
                            anim.SetTrigger("Low Hit");
                        }
                    }
                }
            }

            if (attackHeight == 2)
            {
                if (isBlocking && !isCrouching)
                {
                    StartCoroutine(Inactionable(blockStun));
                    if (isSpecial)
                    {
                        health -= (int)Mathf.Round(damage / 10);
                    }
                    rb.AddForce(pushbackForce, ForceMode2D.Impulse);
                    anim.SetTrigger("Stand Block");

                }
                else
                {
                    health -= damage;
                    if (isKnockdown)
                    {
                        isGrounded = false;
                        Vector3 knockdownForce = new Vector3(60f * back, 45f, 0f);
                        rb.AddForce(knockdownForce, ForceMode2D.Impulse);
                        anim.SetTrigger("Knockdown");
                    }
                    else
                    {
                        StartCoroutine(Inactionable(hitStun));
                        rb.AddForce(pushbackForce, ForceMode2D.Impulse);
                        if (!isBlocking && !isCrouching)
                        {
                            anim.SetTrigger("Stand Hit");
                        }
                        else
                        {
                            anim.SetTrigger("Low Hit");
                        }
                    }
                }
            }
        }
    }

    void OpenCancellable()
    {
        anim.SetBool("canCancel", true);
    }

    void CloseCancellable()
    {
        anim.SetBool("canCancel", false);
    }

    IEnumerator Inactionable(float stun)
    {
        anim.SetBool("canAct", false);
        yield return new WaitForSeconds(stun);
        anim.SetBool("canAct", true);
    }

    void FireballCmd()
    {
        if ((anim.GetBool("canAct") || anim.GetBool("canCancel")) && isGrounded && !anim.GetBool("isCrouching"))
        {
            fireballs = GameObject.FindGameObjectsWithTag("Projectile");
            fireballUsed = false;
            foreach (var fb in fireballs)
            {
                var script = fb.GetComponent<OfflineFireball>();
                if (script.player.id == id)
                {
                    fireballUsed = true;
                }
            }
            if (!fireballUsed)
            {
                FireballRpc();
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
                GameObject fireball = Instantiate(fireballPrefab, spawnPoint, Quaternion.identity);
                fireball.GetComponent<OfflineFireball>().player = gameObject.GetComponent<OfflinePlayer>();
                fireball.GetComponent<OfflineFireball>().hudManager = hudManager;
                hudManager.FireballGray(id, true);
                if (facingRight)
                {
                    fireball.GetComponent<OfflineFireball>().right = false;
                }
                else
                {
                    fireball.GetComponent<OfflineFireball>().right = true;
                }
            }
        }
    }

    void FireballRpc()
    {
        anim.SetTrigger("Fireball");
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

        if (player1)
        {
            if (Input.GetButtonDown("Special") && Input.GetAxisRaw("Horizontal") == forward && anim.GetBool("canAct") && isGrounded)
            {
                anim.SetTrigger("Uppercut");
                rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            if (Input.GetButtonDown("Special2") && Input.GetAxisRaw("Horizontal2") == forward && anim.GetBool("canAct") && isGrounded)
            {
                anim.SetTrigger("Uppercut");
                rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            }
        }

    }

    void Jump()
    {
        float horizontalSpeed = 25.0f;
        float jumpHeight = 65.0f;
        float jumpHorizontal = Input.GetAxisRaw("Horizontal");

        jump = new Vector3(jumpHorizontal * horizontalSpeed, jumpHeight, 0.0f);

        if (player1)
        {
            if (Input.GetButtonDown("Jump") && isGrounded && anim.GetBool("canAct"))
            {
                anim.SetBool("isJumping", true);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0.0f;
                rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump2") && isGrounded && anim.GetBool("canAct"))
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
        if (player1)
        {
            if (anim.GetBool("isCrouching") && Input.GetButtonDown("Attack") && anim.GetBool("canAct") && isGrounded)
            {
                anim.SetTrigger("Crouch Attack");
            }
        }
        else
        {
            if (anim.GetBool("isCrouching") && Input.GetButtonDown("Attack2") && anim.GetBool("canAct") && isGrounded)
            {
                anim.SetTrigger("Crouch Attack");
            }
        }

    }

    void StandAttack()
    {
        if (player1)
        {
            if (!anim.GetBool("isCrouching") && Input.GetButtonDown("Attack") && anim.GetBool("canAct") && isGrounded)
            {
                anim.SetTrigger("Stand Attack");
            }
        }
        else
        {
            if (!anim.GetBool("isCrouching") && Input.GetButtonDown("Attack2") && anim.GetBool("canAct") && isGrounded)
            {
                anim.SetTrigger("Stand Attack");
            }
        }
    }

    void JumpAttack()
    {
        if (player1)
        {
            if (Input.GetButtonDown("Attack") && !isGrounded && !airAttack)
            {
                airAttack = true;
                anim.SetTrigger("Jump Attack");
            }
        }
        else
        {
            if (Input.GetButtonDown("Attack2") && !isGrounded && !airAttack)
            {
                airAttack = true;
                anim.SetTrigger("Jump Attack");
            }
        }
    }

    void AirKicks()
    {
        if (player1)
        {
            if (Input.GetButtonDown("Special") && Input.GetAxisRaw("Horizontal") == back && isGrounded && !anim.GetBool("isCrouching"))
            {
                anim.SetTrigger("Air Kicks");
            }
        }
        else
        {
            if (Input.GetButtonDown("Special2") && Input.GetAxisRaw("Horizontal2") == back && isGrounded && !anim.GetBool("isCrouching"))
            {
                anim.SetTrigger("Air Kicks");
            }
        }

    }

    void FacingDirection()
    {
        scale = transform.localScale;

        if (otherPlayerRB != null && isGrounded)
        {
            if (otherPlayerRB.transform.position.x > transform.position.x && !facingRight && otherPlayer.isGrounded)
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
            else if (otherPlayerRB.transform.position.x < transform.position.x && facingRight && otherPlayer.isGrounded)
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

        if (anim.GetBool("isInvincible"))
        {
            invincible = true;
        }
        else
        {
            invincible = false;
        }

        if (facingRight)
        {
            if (moveHorizontal == -1)
            {
               anim.SetBool("isBlocking", true);
            }
            else{
                anim.SetBool("isBlocking", false);
            }
        }
        else{
            if (moveHorizontal == 1)
            {
               anim.SetBool("isBlocking", true);
            }
            else{
                anim.SetBool("isBlocking", false);
            }
        }


        if (player1)
        {
            spriteRenderer.color = new Color(110f / 255f, 132f / 255f, 255f / 255f);
            if (roundManager.player1Rounds == 1)
            {
                round1.gameObject.SetActive(true);
            }
            else if (roundManager.player1Rounds == 2)
            {
                round2.gameObject.SetActive(true);
            }
        }
        else
        {
            if (roundManager.player2Rounds == 1)
            {
                round1.gameObject.SetActive(true);
            }
            else if (roundManager.player2Rounds == 2)
            {
                round2.gameObject.SetActive(true);
            }
        }

        if (roundManager.roundEnded)
        {
            if (health <= 0 && isGrounded)
            {
                anim.SetTrigger("Died");
            }
            else if (isGrounded)
            {
                anim.SetTrigger("Won");
            }
        }

        if (rb.velocity.y < 0 && !isGrounded)
        {
            anim.SetBool("isFalling", true);
            anim.SetBool("isJumping", false);
        }

        if (anim.GetBool("isKnockedDown") && isGrounded)
        {
            anim.SetTrigger("Get Up");
        }

        if (!roundManager.roundStarted) return;
        Movement();
        Jump();
        CrouchAttack();
        StandAttack();
        JumpAttack();
        AirKicks();
        FacingDirection();

        if (player1)
        {
            if (Input.GetButtonDown("Special") && Input.GetAxisRaw("Horizontal") == 0)
            {
                FireballCmd();
            }
        }
        else
        {
            if (Input.GetButtonDown("Special2") && Input.GetAxisRaw("Horizontal2") == 0)
            {
                FireballCmd();
            }
        }

        Uppercut();

        pushBox.SetActive(isGrounded);

        if (anim.GetBool("isAirKicking"))
        {
            Vector3 movement = new Vector3(forward * moveSpeed * 0.00015f, 0, 0);
            transform.position = transform.position + movement;
        }

    }

    // void FixedUpdate()
    // {
    //     Debug.Log(otherPlayer);
    // }
}
