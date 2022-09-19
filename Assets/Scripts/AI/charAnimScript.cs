using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charAnimScript : MonoBehaviour
{
    NPCMove move;
    Animator anim;
    [SerializeField]
    [Tooltip("How fast this npc is currently moving, dont edit")]
    float speedometer;
    [SerializeField]
    float sprintCutoff;
    [SerializeField]
    float idleCutoff = .01f;
    // Start is called before the first frame update
    void Start()
    {
        sprintCutoff = this.gameObject.GetComponent<NPCMove>().runSpeed - 1f;
        move = this.gameObject.GetComponent<NPCMove>();
        anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        speedometer = move.agent.velocity.magnitude;
        if(speedometer < .0001f){
            //Not Moving
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
        }
        else if(speedometer < sprintCutoff && speedometer > idleCutoff){
            //Walking Speed
            anim.SetBool("isWalking", true);
            anim.SetBool("isRunning", false);

        }
        else if(speedometer > sprintCutoff){
            //Running Speed
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", true);
        }

    }
}
