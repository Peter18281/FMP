using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineFireball : MonoBehaviour
{

    private Rigidbody2D rb;
    private Renderer m_Renderer;
    private Animator anim;
    public bool right = false;
    [SerializeField]
    private float moveSpeed = 0.2f;
    private Vector3 scale;
    private bool _collided = false;
    public OfflinePlayer player;
    private bool collided;
    public OfflineHUDManager hudManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_Renderer = GetComponent<Renderer>();
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

    void DestroyFireBall()
    {
        Destroy(gameObject);
        hudManager.FireballGray(player.id, false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        foreach (var x in player.myHurtboxes)
        {
            if (!player.myHurtboxes.Contains(x) && !player.invincible)
            {
                if (collided) return;
                collided = true;
                anim.SetTrigger("Hit");
                _collided = true;
            }
        }

        if (col.CompareTag("Projectile"))
        {
            collided = true;
            anim.SetTrigger("Hit");
            _collided = true;
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
