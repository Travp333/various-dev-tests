using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Written by Travis Parks
//This script controls the behavior of the bomb item, specifically making the bomb explode when you worldshift.

public class Bomb : MonoBehaviour
{
    GameObject player;
    private void Start() {
        foreach (GameObject G in GameObject.FindGameObjectsWithTag("Player")){
            if(G.GetComponent<Movement>() != null){
                player = G;
            }
        }
    }
    private void Update() {
        if(player.GetComponent<WorldShift>().hollOrReal == true && GetComponent<Rigidbody>().isKinematic == true){
            GetComponent<Shatter>().oneShot(0);
        }
    }
}
