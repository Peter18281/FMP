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
            if(Input.GetAxisRaw("Vertical") == -1){
                anim.SetBool("isCrouching", true);
            }
            else{
                anim.SetBool("isCrouching", false);
            }

        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
            anim.SetBool("isFalling", false);
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

            if (Input.GetButton("Jump") && isGrounded)
            {
                anim.SetBool("isJumping", true);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0.0f;
                rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }
            if (rb.velocity.y < 0 && !isGrounded)
            {
                anim.SetBool("isFalling", true);
                anim.SetBool("isJumping", false);
            }
        }
    }

    void CrouchAttack(){
        if(anim.GetBool("isCrouching") && Input.GetButtonDown("Attack")){
            anim.SetBool("canAct", false);
            Debug.Log(Input.GetButtonDown("Attack"));
            anim.SetTrigger("Crouch Attack");
        }
    }

    void Update()
    {
        Movement();
        Jump();
        CrouchAttack();
    }
}
