using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removeBallFromCount : MonoBehaviour
{
    public GameObject[] player;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<objectSize>() != null && other.gameObject.GetComponent<objectSize>().isTrash){

            foreach (GameObject g in player){
                g.GetComponent<Scan>().removeTrash(other.gameObject);
            }
            other.gameObject.GetComponent<objectSize>().isTrash = false;
        }
        
    }
    void OnTriggerEnter(Collider other){
        if (other.gameObject.GetComponent<objectSize>() != null && other.gameObject.GetComponent<objectSize>().isTrash){

            foreach (GameObject g in player){
                g.GetComponent<Scan>().removeTrash(other.gameObject);
            }
            other.gameObject.GetComponent<objectSize>().isTrash = false;
        }
    }
    void OnTriggerExit(Collider other){
        if(other.gameObject.GetComponent<objectSize>() != null && other.gameObject.GetComponent<objectSize>().isTrash){
            foreach (GameObject g in player){
                g.GetComponent<Scan>().addTrash(other.gameObject);
            }
            other.gameObject.GetComponent<objectSize>().isTrash = true;
        }
    }
}
