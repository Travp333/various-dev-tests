using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{  
    public float hunger = 100;
    public float hp = 100;
    public float coins = 0;
    float stopwatch;
    float countdown = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void getHungry(){
        hunger -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hunger);
        if (stopwatch < countdown){
            stopwatch += Time.deltaTime;
        }
        else if (stopwatch >= countdown){
            getHungry();
            stopwatch = 0;
        }
        

    }
}
