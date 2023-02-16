using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLASHlIGHT : MonoBehaviour
{
    [SerializeField]
    GameObject lite;
    bool flipflop = false;
    public Controls controls;
    void Awake()
    {
        controls = GameObject.Find("Data").GetComponentInChildren<Controls>();
    }
    void Update()
    {
        if(Input.GetKeyDown(controls.keys["flashlight"])){
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
