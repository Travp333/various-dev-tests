using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis Parks
//This script switches the volume levels of the two soundtracks to make them toggle when you world shift
public class soundTrackToggler : MonoBehaviour
{
    [SerializeField]
    float defaultVolume = .1f;
    public bool whichOne;

    public void swap(bool plug){
        if(!plug){
            transform.GetChild(0).gameObject.GetComponent<AudioSource>().volume = defaultVolume;
            transform.GetChild(1).gameObject.GetComponent<AudioSource>().volume = 0;
            whichOne = plug;
        }
        else{
            transform.GetChild(0).gameObject.GetComponent<AudioSource>().volume = 0;
            transform.GetChild(1).gameObject.GetComponent<AudioSource>().volume = defaultVolume;
            whichOne = plug;
        }
    }
    public void swapOff(){
        transform.GetChild(0).gameObject.GetComponent<AudioSource>().volume = defaultVolume;
        transform.GetChild(1).gameObject.GetComponent<AudioSource>().volume = 0;
    }
    public void swapOn(){
        transform.GetChild(0).gameObject.GetComponent<AudioSource>().volume = 0;
        transform.GetChild(1).gameObject.GetComponent<AudioSource>().volume = defaultVolume;
    }
}
