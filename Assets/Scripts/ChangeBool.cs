using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script used for setting booleans in the animator states for the duration of the animation.
public class ChangeBool : StateMachineBehaviour
{
 public string boolName;
 public bool status;
 public bool resetOnExit;

 override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    animator.SetBool(boolName, status);
 }

 override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){

    if(resetOnExit){
        animator.SetBool(boolName, !status);
    }
 }
}
