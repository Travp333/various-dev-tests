using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis Parks
//this script controls the basketball hoop checking whether or not a basketball has gone through it
public class BBallHoop : MonoBehaviour
{
    public BBallTracker tracker;
    int index;
    int ballLength;

    void OnTriggerEnter(Collider other) {
        index = 0;
        foreach(GameObject b in tracker.BBallList){
            index++;
            if (b.gameObject == other.gameObject){
                if(b.gameObject.GetComponent<BBall>().getThruHoop() == false){
                    b.gameObject.GetComponent<BBall>().setThruHoop(true);
                    Invoke("reset", 2f);
                    break;
                }

            }
        }
        
        
    }
    void reset(){
        tracker.BBallList[index - 1].gameObject.GetComponent<BBall>().setThruHoop(false);
    }
}

