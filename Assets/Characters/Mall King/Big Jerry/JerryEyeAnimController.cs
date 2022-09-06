using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerryEyeAnimController : MonoBehaviour
{
    ZombieMove move;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        move = this.transform.parent.GetComponent<ZombieMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if(move.scared){
            anim.SetBool("Scared", true);
        }
        else{
            anim.SetBool("Scared", false);
        }

        if(move.infected){
            anim.SetBool("Infected", true);
        }
        else{
            anim.SetBool("Infected", false);
        }
    }
}
