using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    private Rigidbody2D rb;
    private Renderer m_Renderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_Renderer = GetComponent<Renderer>();
    }

    void Move()
    {
        Vector3 movement = new Vector3(0.2f, 0, 0);
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
