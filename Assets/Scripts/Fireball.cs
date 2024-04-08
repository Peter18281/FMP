using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Fireball : NetworkBehaviour
{

    private Rigidbody2D rb;
    private Renderer m_Renderer;
    private NetworkAnimator nAnim;
    private Animator anim;
    [SyncVar] public bool right = false;
    [SerializeField]
    private float moveSpeed = 0.2f;
    private Vector3 scale;
    private bool _collided = false;
    public Player player;
    private bool collided;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_Renderer = GetComponent<Renderer>();
        nAnim = GetComponent<NetworkAnimator>();
        anim = GetComponent<Animator>();
        scale = transform.localScale;
    }

    void Move()
    {
        if (right && moveSpeed > 0 && scale.x > 0)
        {
            moveSpeed *= -1;
            scale.x *= -1;
        }
        transform.localScale = scale;
        Vector3 movement = new Vector3(moveSpeed, 0, 0);
        transform.position = transform.position + movement;
    }

    [Server]
    void DestroyFireBall()
    {
        NetworkServer.Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        foreach (var x in player.myHurtboxes)
        {
            if (!player.myHurtboxes.Contains(x) && !player.invincible)
            {
                if (collided) return;
                collided = true;
                nAnim.SetTrigger("Hit");
                _collided = true;
            }
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_collided)
        {
            Move();
        }

        if (!m_Renderer.isVisible || !anim.GetBool("isExploding"))
        {
            DestroyFireBall();
        }
    }
}
