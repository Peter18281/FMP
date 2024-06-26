using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script used to store data for an attack when the player gets hit.
public class AttackData : MonoBehaviour
{
    public int damage;
    public float pushback;
    public bool isKnockdown;
    public bool isSpecial;
    public int attackHeight;
    public float hitStun;
    public float blockStun;
    public bool isFireball;
    public Fireball fireball;
}
