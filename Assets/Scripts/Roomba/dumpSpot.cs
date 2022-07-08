using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dumpSpot : MonoBehaviour
{
    public enum dump{electronic, food, recycleable, misc};
    public dump dumpType;
    bool gate = true;
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
       // Debug.Log("--------------------------------------------------------");
        //Debug.Log(dumpType);
        //Debug.Log(gate);
        //Debug.Log("--------------------------------------------------------");
    }
    void OnTriggerEnter(Collider other){
        if(gate){
            gate = false;
            if (other.gameObject.tag == "Roomba" && !other.transform.root.gameObject.GetComponent<Scan>().dumping){
                if(dumpType == dump.electronic){
                    other.transform.root.gameObject.GetComponent<Scan>().dumpElectronics();
                }
                else if(dumpType == dump.food){
                    other.transform.root.gameObject.GetComponent<Scan>().dumpFood();
                }
                else if(dumpType == dump.recycleable){
                    other.transform.root.gameObject.GetComponent<Scan>().dumpRecyclable();
                }
                else if(dumpType == dump.misc){
                    other.transform.root.gameObject.GetComponent<Scan>().dumpMisc();
                }
            }
                
        }
    }
    void OnTriggerExit(Collider other){
        gate = true;
    }
}
