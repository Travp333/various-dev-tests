using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis Parks
//This script spawns a ragdoll and destroys the root object when its called, namely when the scientist dies, it becomes a ragdoll
public class ragdollSpawn : MonoBehaviour
{
    [SerializeField]
    bool isProtected;
    [SerializeField]
    GameObject ragdoll;
    [SerializeField]
    Transform spawnPos;
    // Start is called before the first frame update
    public void spawn(){
        if(!isProtected){
            Instantiate(ragdoll, spawnPos.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
