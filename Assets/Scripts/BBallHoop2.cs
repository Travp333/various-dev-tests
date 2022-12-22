using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
//Travis Parks
// This script finalizes the basketball going through the hoop
public class BBallHoop2 : MonoBehaviour
{
    public BBallTracker tracker;
    int ballLength;
    Animator anim;
    void Start() {
        anim = transform.parent.transform.parent.GetComponent<Animator>();
    }
    //when something collides with the second volume on the hoop
    void OnTriggerEnter(Collider other) {
        // go through every current ball and see if that was the collision
        foreach(GameObject b in tracker.BBallList){
            if (b.gameObject == other.gameObject){
                // if it was, check if it has triggered its "getthruhoop" state
                if(b.gameObject.GetComponent<BBall>().getThruHoop()){
                    b.gameObject.GetComponent<BBall>().setThruHoop(false);
                    anim.SetBool("isSwish", true);
                    Invoke("reset", .1f);
                }
            }
        }
        
    }
    void reset(){
        anim.SetBool("isSwish", false);
    }
}

