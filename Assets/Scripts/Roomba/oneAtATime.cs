using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oneAtATime : MonoBehaviour
{

    void OnTriggerEnter(Collider other){
        foreach(GameObject g in FindObjectsOfType<GameObject>()){
            if(g.GetComponent<Scan>()!= null && g != other.transform.root.gameObject){
                g.GetComponent<Scan>().occupied = true;
            }
            else if (g == other.transform.root.gameObject){
                g.GetComponent<Scan>().occupied = false;
            }
        }
    }

    void OnTriggerStay(Collider other){
        if (other.transform.root.gameObject.GetComponent<Scan>().occupied == true){
            other.transform.root.gameObject.GetComponent<Scan>().occupied = false;
        }
    }
    void OnTriggerExit(Collider other){
        foreach(GameObject g in FindObjectsOfType<GameObject>()){
            if(g.GetComponent<Scan>()!= null){
                g.GetComponent<Scan>().occupied = false;
            }
        }
    }

}
