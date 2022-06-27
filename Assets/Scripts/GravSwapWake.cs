using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The whole point of this script is to prevent "sleeping" props from getting stuck in place when gravity suddenly changes. can be removed if we never plan to change gravity

public class GravSwapWake : MonoBehaviour
{
    Vector3 dummy;
    void Update()
    {
        if (Physics.gravity != dummy ) {
            GetComponent<Rigidbody>().WakeUp();
            dummy = Physics.gravity;
        }
    }
}
