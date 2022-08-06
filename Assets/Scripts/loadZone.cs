using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//forces the player to load their game whtn they overlap
//Travis Parks and Andrew
public class loadZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            other.transform.root.GetComponent<PlayerStats>().LoadPlayer();
        }
    }
}
