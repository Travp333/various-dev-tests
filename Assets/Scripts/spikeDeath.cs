using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Lizbeth Solis
public class spikeDeath : MonoBehaviour
{
    
    [SerializeField]
    public float damageSpikes = 1.0f;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player"){
             //health -= damageSpikes;
            other.transform.root.GetComponent<PlayerStats>().takeDamage(damageSpikes);
         }
         if(other.gameObject.GetComponent<Shatter>() != null ){
             other.gameObject.GetComponent<Shatter>().oneShot(0);
         }
    }
}

