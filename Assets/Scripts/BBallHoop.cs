using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBallHoop : MonoBehaviour
{
    GameObject[] balls;
    int index;
    int ballLength;

    void Update() {
        if (GameObject.FindGameObjectsWithTag("bball").Length != ballLength){
            balls = GameObject.FindGameObjectsWithTag("bball");
            ballLength = balls.Length;
        }

    }
    void Start() {
        balls = GameObject.FindGameObjectsWithTag("bball");
    }

    void OnTriggerEnter(Collider other) {
        index = 0;
        foreach(GameObject b in balls){
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
        balls[index - 1].gameObject.GetComponent<BBall>().setThruHoop(false);
    }
}

