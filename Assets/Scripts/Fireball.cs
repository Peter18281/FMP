using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Fireball : NetworkBehaviour
{

    private Rigidbody2D rb;
    private Renderer m_Renderer;
    public bool right = false;
    [SerializeField]
    private float moveSpeed = 0.2f;
    private Vector3 scale;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_Renderer = GetComponent<Renderer>();
        scale = transform.localScale;
    }

    void Move()
    {
        if(right && moveSpeed > 0 && scale.x > 0){
          moveSpeed *= -1;
          scale.x *= -1;
        }
        transform.localScale = scale;
        Vector3 movement = new Vector3(moveSpeed, 0, 0);
        transform.position = transform.position + movement;
    }

    void DestroyFireBall()
    {
        if (!m_Renderer.isVisible)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        DestroyFireBall();
    }
}
