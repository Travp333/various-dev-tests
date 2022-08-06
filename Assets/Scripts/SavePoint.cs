using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Forces the game to save the players progress
//Travis Parks and Andrew
public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player"){
            other.gameObject.transform.root.GetComponent<PlayerStats>().SavePlayer();
        }
    }
}
