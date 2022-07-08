using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uninfectedList : MonoBehaviour
{
    public List<GameObject> unscareduninfected = new List<GameObject>();
    public List<GameObject> scareduninfected = new List<GameObject>();
    public List<GameObject> infected = new List<GameObject>();
    public void Infected(GameObject agent){
        unscareduninfected.Remove(agent);
    }
    public void updateInfectedList(){
        infected.Clear();
        foreach(GameObject g in GameObject.FindObjectsOfType<GameObject>()){
            if(g.GetComponent<NPCMove>()!=null){
                if(g.GetComponent<NPCMove>().infected){
                    infected.Add(g);
                }
            }
        }
    }

    public void updateUninfectedList(){
        unscareduninfected.Clear();
        scareduninfected.Clear();

        foreach(GameObject g in GameObject.FindObjectsOfType<GameObject>()){
            if(g.GetComponent<NPCMove>()!=null){
                if(!g.GetComponent<NPCMove>().infected && !g.GetComponent<NPCMove>().scared){
                    unscareduninfected.Add(g);
                }
                if(!g.GetComponent<NPCMove>().infected && g.GetComponent<NPCMove>().scared){
                    scareduninfected.Add(g);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject g in GameObject.FindObjectsOfType<GameObject>()){
            if(g.GetComponent<NPCMove>()!=null){
                if(!g.GetComponent<NPCMove>().infected && !g.GetComponent<NPCMove>().scared){
                    unscareduninfected.Add(g);
                }
                if(!g.GetComponent<NPCMove>().infected && g.GetComponent<NPCMove>().scared){
                    scareduninfected.Add(g);
                }
                if(g.GetComponent<NPCMove>().infected){
                    infected.Add(g);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
