using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jerryAnimScript : MonoBehaviour
{
    [SerializeField]
    float sprintThreshold;
    [SerializeField]
    float walkThreshold;
    [SerializeField]
    float slowThreshold;
    NPCMove move;
    Animator anim;
    Rigidbody body;
    [SerializeField]
    float speedometer;
    bool isRunning;
    bool isWalking;
    // Start is called before the first frame update
    void Start()
    {
        body = this.gameObject.GetComponent<Rigidbody>();
        move = this.gameObject.GetComponent<NPCMove>();
        anim = this.gameObject.GetComponent<Animator>();
    }

    public void setIsRunning(bool swap){
        isRunning = swap;
    }
    public void setIsWalking(bool swap){
        isWalking = swap;
    }

    // Update is called once per frame
    void Update()
    {
        speedometer = move.agent.velocity.magnitude;
        //Debug.Log(isWalking);
        //Debug.Log(isRunning);
        if(move.infected){
            anim.SetBool("isInfected", true);
        }
        else{
            anim.SetBool("isInfected", false);
        }
        if(move.agent.velocity.magnitude < .001f){
            //Debug.Log("Not moving!");
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
        }
        else if (move.infected && move.agent.velocity.magnitude < walkThreshold && move.agent.velocity.magnitude > slowThreshold){
                anim.SetBool("isWalking", true);
                anim.SetBool("isRunning", false);
        }
        else if(move.agent.velocity.magnitude > walkThreshold && move.agent.velocity.magnitude < sprintThreshold && move.agent.velocity.magnitude > 0){
            if(move.infected){
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", true);
            }
            else{
                //Debug.Log("walking!");
                anim.SetBool("isWalking", true);
                anim.SetBool("isRunning", false);
            }

        }
        else if(move.agent.velocity.magnitude > sprintThreshold && move.agent.velocity.magnitude > 0){
            //Debug.Log("running!");
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", true);
        }
    }
}
