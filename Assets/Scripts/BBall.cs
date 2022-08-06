using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis Parks
//This script holds the thruHoop bool that is checked un bballhoop
public class BBall : MonoBehaviour
{
    bool thruHoop = false;

    public bool getThruHoop(){
        return thruHoop;
    }
    public void setThruHoop(bool plug){
       thruHoop = plug;
    }

    // debug lines to observe the ball's thruHoop state

   // void Update() {
        //if(thruHoop == true){
       //     GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
       // }
       // else{
       //     GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
      //  }
   // }
}
