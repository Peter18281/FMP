using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private List<GameObject> myHitboxes;
    private AttackData attackData;
    private bool isColliding;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("HitBox") && !myHitboxes.Contains(col.gameObject))
        {
            if(isColliding) return;
            isColliding = true;
            attackData = col.gameObject.GetComponent<AttackData>();
            player.GetHit(
                attackData.damage,
                attackData.pushback,
                attackData.isKnockdown,
                player.anim.GetBool("isBlocking"),
                attackData.isSpecial,
                player.anim.GetBool("isCrouching"),
                attackData.attackHeight,
                attackData.hitStun,
                attackData.blockStun
                 );
        }
    }

    void Update()
    {
        isColliding = false;
    }
}
