using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis Parks
//This script controls the special kind of switch that appears in the bomb level that requires you to pull two switches to trigger a door
public class doubleSwitch : MonoBehaviour
{
    [SerializeField]
    Light light1;
    [SerializeField]
    Light light2;
    bool switch1Hit;
    bool switch2Hit;
    [SerializeField]
    bool gate;
    [SerializeField]
    float timer;
    void Start() {
        light1.color = Color.red;
        light2.color = Color.red;
    }
    void resetSwitch1(){
        if(!gate){
            //Debug.Log("Reset Switch1");
            switch1Hit = false;
            light1.color = Color.red;
        }
    }
    void resetSwitch2(){
        if(!gate){
            //Debug.Log("Reset Switch2");
            switch2Hit = false;
            light2.color = Color.red;
        }
    }
    public void hitSwitch1(){
        if(!gate){
            //Debug.Log("Hit Switch1");
            switch1Hit = true;
            Invoke("resetSwitch1", timer);
            light1.color = Color.yellow;
        }
    }
    public void hitSwitch2(){
        if(!gate){
            //Debug.Log("Hit Switch2");
            switch2Hit = true;
            Invoke("resetSwitch2", timer);
            light2.color = Color.yellow;
        }
    }
    public void hitSwitch(){
        if(switch1Hit && switch2Hit && !gate){
            //Debug.Log("Hit both Switches!");
            GetComponent<InteractableObject>().Press();
            light1.color = Color.green;
            light2.color = Color.green;
            gate = true;
        }
    }
}
