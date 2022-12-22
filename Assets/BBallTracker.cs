using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBallTracker : MonoBehaviour
{
    public List<GameObject> BBallList = new List<GameObject>();
    void Start()
    {
        foreach (GameObject b in GameObject.FindGameObjectsWithTag("bball")){
            BBallList.Add(b);
        }
        //create list of balls

    }
    public void addBall(GameObject ball){
        //allow for creating and destroying balls on the fly
    }

    public void removeBall(GameObject ball){
        //allow for creating and destroying balls on the fly
    }
}
