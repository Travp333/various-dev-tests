using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableDynamicBone : MonoBehaviour
{
    [SerializeField]
    GameObject boneslot1;
    [SerializeField]
    GameObject boneslot2;
    // Start is called before the first frame update

    public void toggle(bool plug){
        if (plug){
            boneslot1.GetComponent<DynamicBone>().enabled = false;
            boneslot2.GetComponent<DynamicBone>().enabled = false;
        }
        else if (!plug){
            boneslot1.GetComponent<DynamicBone>().enabled = true;
            boneslot2.GetComponent<DynamicBone>().enabled = true;
        }
    }
}
