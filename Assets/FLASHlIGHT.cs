using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLASHlIGHT : MonoBehaviour
{
    [SerializeField]
    GameObject lite;
    bool flipflop = true;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)){
            if(flipflop){
                lite.SetActive(false);
                flipflop = false;
            }
            else if(!flipflop){
                lite.SetActive(true);
                flipflop = true;
            }

        }
    }
}
