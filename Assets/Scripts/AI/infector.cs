using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infector : MonoBehaviour
{
    ZombieMove move;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        move = this.transform.parent.GetComponent<ZombieMove>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ZombieMove>() != null){
            if(move.infected && !other.gameObject.GetComponent<ZombieMove>().infected && !move.gate){
                move.Infect(other);
                //move.resetScared();
            }

        }
    }
}
