using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uninfectedList : MonoBehaviour
{
    public List<GameObject> uninfected = new List<GameObject>();
    public List<GameObject> infected = new List<GameObject>();
    public void removeFromUninfected(GameObject agent){
        if(uninfected.Contains(agent)){
            uninfected.Remove(agent);
        }
        
    }
    public void removeFromInfected(GameObject agent){
        if(infected.Contains(agent)){
            infected.Remove(agent);
        }
        
    }
    public void updateInfectedList(GameObject g){
        if(!infected.Contains(g)){
            infected.Add(g);
        }
    }

    public void updateUninfectedList(GameObject g){
        if(!uninfected.Contains(g)){
            uninfected.Add(g);
        }

    }


}
