using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HUDManager : NetworkBehaviour
{
    public GameObject player;
    private Player playerScript;
    public Image healthBar;
    public float healthAmount = 100f;
    private GameObject halfway;
    public GameObject healthBarObject;
    public GameObject rounds;
    public GameObject cooldowns;
    public Image forwardArrow;
    public Image backArrow;
    public Image neutral;
    private bool fireballUsed;
    
    void Awake()
    {
        playerScript = player.GetComponent<Player>();
        halfway = GameObject.Find("Halfway");
        if (player.transform.position.x > halfway.transform.position.x)
        {
            healthBarObject.transform.localScale = new Vector3(healthBarObject.transform.localScale.x * -1, healthBarObject.transform.localScale.y, healthBarObject.transform.localScale.z);
            rounds.transform.localScale = new Vector3(rounds.transform.localScale.x * -1, rounds.transform.localScale.y, rounds.transform.localScale.z);
            cooldowns.transform.localScale = new Vector3(cooldowns.transform.localScale.x * -1, cooldowns.transform.localScale.y, cooldowns.transform.localScale.z);

            healthBar.fillOrigin = (int)Image.OriginHorizontal.Right;
        }
    }

    void Update()
    {
        healthAmount = playerScript.health;
        healthBar.fillAmount = healthAmount / 100f;

        if (!playerScript.anim.GetBool("canAct"))
        {
            GrayOut(forwardArrow, true);
            GrayOut(backArrow, true);
            GrayOut(neutral, true);
        }
        else if(fireballUsed)
        {
            GrayOut(forwardArrow, false);
            GrayOut(backArrow, false);
        }
        else {
            GrayOut(forwardArrow, false);
            GrayOut(backArrow, false);
            GrayOut(neutral, false);
        }
    }

    public void FireballGray(int id, bool used)
    {
        if (id == playerScript.id && used)
        {
            GrayOut(neutral, true);
            fireballUsed = true;
        }
        else if (id == playerScript.id && !used)
        {
            GrayOut(neutral, false);
            fireballUsed = false;
        }
    }

    public void GrayOut(Image img, bool gray)
    {
        if (gray)
        {
            img.color = new Color32(255, 255, 255, 70);
        }
        else
        {
            img.color = new Color32(255, 255, 255, 255);
        }
    }
}
