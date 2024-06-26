using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<GameObject> myHitboxes;
    private AttackData attackData;
    private Fireball fireball;
    private bool isColliding;

    void OnTriggerEnter2D(Collider2D col)
    {
        //Check if collision is with a attack hit box and if not the player's own hitbox, trigger the getHit function on the opposing player.
        if (col.CompareTag("HitBox") && !myHitboxes.Contains(col.gameObject) && !player.invincible)
        {
            if (isColliding) return;
            isColliding = true;
            attackData = col.gameObject.GetComponent<AttackData>();
            //if the attack is a fireball we need to check whose fireball it is.
            if (attackData.isFireball)
            {
                fireball = attackData.fireball;
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
            //Deactivate the hitbox after collision to avoid multiple hits.
            col.GetComponent<GameObject>().SetActive(false);
        }
    }

    void Update()
    {
        isColliding = false;
    }
}
