using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis Parks
//This script calls sound objects tied to a certain object
public class SoundCaller : MonoBehaviour
{
    [SerializeField]
    GameObject[] punches;
    int index;
    private void Start() {
        if(tag == "ragdoll"){
            index = Random.Range(0, punches.Length);
            punches[index].GetComponent<AudioSource>().Play();
        }
    }
    public void playPunch(){   
        index = Random.Range(0, punches.Length);
        punches[index].GetComponent<AudioSource>().Play();
    }
    
}
