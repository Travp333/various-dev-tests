using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis Parks
//This script will force a bomb to explode if it overlaps with this volume
public class bombTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Explosive"){
            other.gameObject.GetComponent<Shatter>().oneShot(0);
            if(this.gameObject.GetComponent<dieOnEvent>() != null){
                this.gameObject.GetComponent<dieOnEvent>().DIE();
            }
        }
    }
}
