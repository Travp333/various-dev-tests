using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jerryAnimScript : MonoBehaviour
{
    NPCMove move;
    Animator anim;
    [SerializeField]
    [Tooltip("How fast this npc is currently moving, dont edit")]
    float speedometer;
    // Start is called before the first frame update
    void Start()
    {
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
        else if(!move.scared && !move.chasing){
            //Walking Speed
            anim.SetBool("isWalking", true);
            anim.SetBool("isRunning", false);

        }
        else if(move.scared || move.chasing){
            //Running Speed
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", true);
        }

    }
}
