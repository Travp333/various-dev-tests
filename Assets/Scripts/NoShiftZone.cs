using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoShiftZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            other.gameObject.transform.root.gameObject.GetComponent<ImprovedZoneWarp>().setShiftBlocked(true);
        }
    }
    void OnTriggerExit(Collider other) {
            if(other.gameObject.tag == "Player"){
        other.gameObject.transform.root.gameObject.GetComponent<ImprovedZoneWarp>().setShiftBlocked(false);
    }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
