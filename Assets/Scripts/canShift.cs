using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canShift : MonoBehaviour
{
    Collider dummyCollider = null;
    [SerializeField]
    GameObject player;
    ImprovedZoneWarp warp;
    bool shiftable = true;


    void Start()
    {
        //gameObject.GetComponent<MaterialSelector>().Select(1);
        warp = player.GetComponent<ImprovedZoneWarp>();

    }
    void OnTriggerExit(Collider other) {
        shiftable = true;
        //gameObject.GetComponent<MaterialSelector>().Select(1);
        dummyCollider = null;
    }
    public Collider getCollider(){
        return dummyCollider;
    }
    public bool getShiftable(){
        return shiftable;
    }
    void OnTriggerStay(Collider other) {
        //this dummy is colling with something
        if(other.gameObject.tag == "Breakable" || other.gameObject.tag == "Explosive" ){
            //it is breakable!
            shiftable = true;
            //we can shift!
            //gameObject.GetComponent<MaterialSelector>().Select(1);
            dummyCollider = other;
            }  
        
        // the thing the dummy is colliding with is not breakable
        else{
            //we cannot shift!
            shiftable = false;
            //update material to show it is not shiftable
            //gameObject.GetComponent<MaterialSelector>().Select(0);
            
        }  
    }
}  