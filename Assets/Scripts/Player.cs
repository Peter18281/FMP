using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
   void Movement(){
    if(isLocalPlayer){
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal * 0.1f, 0, 0);
        transform.position = transform.position + movement;
    }
   }

   void Update(){
    Movement();
   }
}