using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineHitDetection : MonoBehaviour
{
    [SerializeField]
    private OfflinePlayer player;
    [SerializeField]
    private List<GameObject> myHitboxes;
    private OfflineAttackData attackData;
    private OfflineFireball fireball;
    private bool isColliding;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("HitBox") && !myHitboxes.Contains(col.gameObject) && !player.invincible)
        {
            if (isColliding) return;
            isColliding = true;
            attackData = col.gameObject.GetComponent<OfflineAttackData>();
            if (attackData.isFireball)
            {
                fireball = attackData.fireball;
                Debug.Log(fireball.player.id);
                if (fireball.player.id != player.id && !player.anim.GetBool("fireballInvincible"))
                {
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
            else
            {
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
            col.GetComponent<GameObject>().SetActive(false);
        }
    }

    void Update()
    {
        isColliding = false;
    }
}
