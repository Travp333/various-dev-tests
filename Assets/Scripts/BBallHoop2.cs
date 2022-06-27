using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 


public class BBallHoop2 : MonoBehaviour
{
    GameObject[] balls;
    int ballLength;
    Animator anim;
    void Update() {
        //checks if the amount of balls in level has changed. if not, do nothing
        if (GameObject.FindGameObjectsWithTag("bball").Length != ballLength){
            balls = GameObject.FindGameObjectsWithTag("bball");
            ballLength = balls.Length;
        }

    }

    void Start() {
        balls = GameObject.FindGameObjectsWithTag("bball");
        anim = transform.parent.transform.parent.GetComponent<Animator>();
    }
    //when something collides with the second volume on the hoop
    void OnTriggerEnter(Collider other) {
        // go through every current ball and see if that was the collision
        foreach(GameObject b in balls){
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

