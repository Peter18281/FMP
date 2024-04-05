using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCollision : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PushBox"))
        {
            if (rb.transform.position.x > col.gameObject.transform.position.x)
            {
                Vector3 movement = new Vector3(0.5f, 0, 0);
                rb.transform.position = rb.transform.position + movement;
            }
            else{
                Vector3 movement = new Vector3(-0.5f, 0, 0);
                rb.transform.position = rb.transform.position + movement;
            }
        }
    }
}
