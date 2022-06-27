using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHunger : MonoBehaviour
{
    float maxHunger = 100f;
    float hunger = 100f;
    [SerializeField]
    float hungerRate;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void increaseHunger(float food){
        if (hunger < 100){
            if (hunger + food > maxHunger){
                hunger = 100f;
            }
            else{
                hunger = hunger + food;
            }
        }
        else {
           // Debug.Log("Not Hungry!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hunger);
        if (hunger <= 0){
           // Debug.Log("STARVED");
            //script
        }
        else{
            hunger = hunger - hungerRate;
        }
        
    }
}
