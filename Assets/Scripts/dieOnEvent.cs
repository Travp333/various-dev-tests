using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis Parks
//This script will simple destroy the game object it is attached to if it is called.
public class dieOnEvent : MonoBehaviour
{
    // Start is called before the first frame update
    public void DIE(){
        Destroy(this.gameObject);
    }
}
