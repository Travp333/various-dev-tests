using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviorChangersList : MonoBehaviour
{
    public List<GameObject> npcs = new List<GameObject>();
    public List<GameObject> scary = new List<GameObject>();
    public List<GameObject> attractive = new List<GameObject>();

    public void removeFromScary(GameObject agent){
        if(scary.Contains(agent)){
            scary.Remove(agent);
        }
        
    }
    public void removeFromAttractive(GameObject agent){
        if(attractive.Contains(agent)){
            attractive.Remove(agent);
        }
        
    }
    public void removeFromNPCS(GameObject agent){
        if(npcs.Contains(agent)){
            npcs.Remove(agent);
        }
        
    }
    public void updateNPCList(GameObject g){
        if(!npcs.Contains(g)){
            npcs.Add(g);
        }
    }
    public void updateScaryList(GameObject g){
        if(!scary.Contains(g)){
            scary.Add(g);
        }
    }

    public void updateAttractiveList(GameObject g){
        if(!attractive.Contains(g)){
            attractive.Add(g);
        }

    }


}
