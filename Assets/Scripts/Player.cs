using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
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
    private NetworkAnimator nAnim;
    [SerializeField]
    private GameObject fireballObject;
    private GameObject[] fireballs;
    private Vector3 spawnPoint;
    private bool airAttack;
    private Player otherPlayer;
    private Rigidbody2D otherPlayerRB;
    private GameObject[] players;
    private GameObject halfway;
    private Vector3 scale;
    [SyncVar] public int health = 100;
    [SerializeField]
    private GameObject pushBox;
    public int id;
    private bool fireballUsed = false;
    public bool invincible = false;
    [SerializeField]
    public List<GameObject> myHurtboxes;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        nAnim = GetComponent<NetworkAnimator>();
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 1)
        {
            id = 1;
        }
        else
        {
            id = 2;
        }
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

    public void GetHit(int damage, float pushback, bool isKnockdown, bool isBlocking, bool isSpecial, bool isCrouching, int attackHeight, float hitStun, float blockStun)
    {
        Vector3 pushbackForce = new Vector3(pushback * back, 0, 0);
        if(invincible) return;
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
                nAnim.SetTrigger("Low Block");
            }
            else
            {
                health -= damage;
                if (isKnockdown)
                {
                    nAnim.SetTrigger("Knockdown");
                }
                else
                {
                    StartCoroutine(Inactionable(hitStun));
                    rb.AddForce(pushbackForce, ForceMode2D.Impulse);
                    if (!isCrouching)
                    {
                        nAnim.SetTrigger("Stand Hit");
                    }
                    else
                    {
                        nAnim.SetTrigger("Low Hit");
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
                    nAnim.SetTrigger("Stand Block");
                }
                else
                {
                    nAnim.SetTrigger("Low Block");
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
                    nAnim.SetTrigger("Knockdown");
                }
                else
                {
                    StartCoroutine(Inactionable(hitStun));
                    rb.AddForce(pushbackForce, ForceMode2D.Impulse);
                    if (!isBlocking && !isCrouching)
                    {
                        nAnim.SetTrigger("Stand Hit");
                    }
                    else
                    {
                        nAnim.SetTrigger("Low Hit");
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
                    nAnim.SetTrigger("Stand Block");

            }
            else
            {
                health -= damage;
                if (isKnockdown)
                {
                    isGrounded = false;
                    Vector3 knockdownForce = new Vector3(60f * back, 45f, 0f);
                    rb.AddForce(knockdownForce, ForceMode2D.Impulse);
                    nAnim.SetTrigger("Knockdown");
                }
                else
                {
                    StartCoroutine(Inactionable(hitStun));
                    rb.AddForce(pushbackForce, ForceMode2D.Impulse);
                    if (!isBlocking && !isCrouching)
                    {
                        nAnim.SetTrigger("Stand Hit");
                    }
                    else
                    {
                        nAnim.SetTrigger("Low Hit");
                    }
                }
            }
        }
    }

    void OpenCancellable(){
        anim.SetBool("canCancel", true);
    }

    void CloseCancellable(){
        anim.SetBool("canCancel", false);
    }

    IEnumerator Inactionable(float stun)
    {
        anim.SetBool("canAct", false);
        yield return new WaitForSeconds(stun);
        anim.SetBool("canAct", true);
    }

    [Command]
    void FireballCmd()
    {
        if ((anim.GetBool("canAct") || anim.GetBool("canCancel")) && isGrounded && !anim.GetBool("isCrouching"))
        {
            fireballs = GameObject.FindGameObjectsWithTag("Projectile");
            fireballUsed = false;
            foreach (var fb in fireballs)
            {
                var script = fb.GetComponent<Fireball>();
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
                GameObject fireball = Instantiate(NetworkManager.singleton.spawnPrefabs[0], spawnPoint, Quaternion.identity);
                NetworkServer.Spawn(fireball);
                fireball.GetComponent<Fireball>().player = GetComponent<Player>();
                if (facingRight)
                {
                    fireball.GetComponent<Fireball>().right = false;
                }
                else
                {
                    fireball.GetComponent<Fireball>().right = true;
                }
            }
        }
    }

    [ClientRpc]
    void FireballRpc()
    {
        nAnim.SetTrigger("Fireball");
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
            nAnim.SetTrigger("Uppercut");
            rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
        }
    }

    void Jump()
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

    void CrouchAttack()
    {
        if (anim.GetBool("isCrouching") && Input.GetButtonDown("Attack") && anim.GetBool("canAct") && isGrounded)
        {
            nAnim.SetTrigger("Crouch Attack");
        }
    }

    void StandAttack()
    {
        if (!anim.GetBool("isCrouching") && Input.GetButtonDown("Attack") && anim.GetBool("canAct") && isGrounded)
        {
            nAnim.SetTrigger("Stand Attack");
        }
    }

    void JumpAttack()
    {
        if (Input.GetButtonDown("Attack") && !isGrounded && !airAttack)
        {
            airAttack = true;
            nAnim.SetTrigger("Jump Attack");
        }
    }

    void AirKicks()
    {
        if (Input.GetButtonDown("Special") && Input.GetAxisRaw("Horizontal") == back && isGrounded && !anim.GetBool("isCrouching"))
        {
            nAnim.SetTrigger("Air Kicks");
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
        FacingDirection();
        
        if (!Application.isFocused) return;
        if (isLocalPlayer)
        {
            Movement();
            Jump();
            CrouchAttack();
            StandAttack();
            JumpAttack();
            AirKicks();
            if (Input.GetButtonDown("Special") && Input.GetAxisRaw("Horizontal") == 0)
            {
                FireballCmd();
            }
            Uppercut();

            pushBox.SetActive(isGrounded);

            if(anim.GetBool("isInvincible")){
                invincible = true;
            }
            else{
                invincible = false;
            }

            if (anim.GetBool("isKnockedDown") && isGrounded)
            {
                nAnim.SetTrigger("Get Up");
            }

            if (anim.GetBool("isAirKicking"))
            {
                Vector3 movement = new Vector3(forward * moveSpeed * 0.00015f, 0, 0);
                transform.position = transform.position + movement;
            }

            if (rb.velocity.y < 0 && !isGrounded)
            {
                anim.SetBool("isFalling", true);
                anim.SetBool("isJumping", false);
            }

            if (players.Length < 2)
            {
                players = GameObject.FindGameObjectsWithTag("Player");
            }
            else if (otherPlayer == null)
            {
                foreach (var player in players)
                {
                    if (player != gameObject)
                    {
                        otherPlayer = player.GetComponent<Player>();
                        otherPlayerRB = player.GetComponent<Rigidbody2D>();
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        // Debug.Log(invincible);
    }
}
