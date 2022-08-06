using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis Parks
//This script Controls the behavior of the bomb dispansor item. It will spawn a new bomb and destroy all existing bombs, unless it is tagged as a spawner
public class bombDIspensor : MonoBehaviour
{
    [SerializeField]
    Transform bombSpawn;
    [SerializeField]
    GameObject bomb;
    [SerializeField]
    bool spawner;
    public float interval;
    GameObject bom;
    public void spawnBomb(){
        foreach(GameObject G in GameObject.FindGameObjectsWithTag("Explosive")){
            if(G.GetComponent<Bomb>()!=null){
                G.GetComponent<Shatter>().oneShot(0);
            }
        }
        Instantiate(bomb, bombSpawn.position, Quaternion.identity);
    }
    private void Update() {
        if(spawner){
            if(interval > 5){
                interval = 0;
                if(bom != null){
                    bom.GetComponent<Shatter>().setBoomBlocked(false);
                    bom.GetComponent<Shatter>().oneShot(0);
                }
                bom = Instantiate(bomb, bombSpawn.position, Quaternion.identity);
            }
            interval += UnityEngine.Time.deltaTime;
        }
    }

}
