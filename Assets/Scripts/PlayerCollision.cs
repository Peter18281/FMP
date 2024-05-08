using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script used to handle player collision
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

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
