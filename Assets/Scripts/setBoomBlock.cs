using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis Parks
//This script blocks the bomb's ability to blow up. It essentailyl allows us to make safe zones that the bomb will never explode in
public class setBoomBlock : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Explosive"){
            other.gameObject.GetComponent<Shatter>().setBoomBlocked(true);
        }
        
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Explosive"){
            other.gameObject.GetComponent<Shatter>().setBoomBlocked(true);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Explosive"){
            other.gameObject.GetComponent<Shatter>().setBoomBlocked(false);
        }
        
    }
}
