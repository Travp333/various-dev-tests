using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : MonoBehaviour
{
    [SerializeField]
    float food = default;
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player"){

            other.gameObject.transform.root.gameObject.GetComponent<FPSMovingSphere>().hungerDive();
            other.gameObject.transform.root.gameObject.GetComponent<playerHunger>().increaseHunger(food);    

            //hunger.GetComponent<FPSMovingSphere>().hungerDive();
           // hunger.GetComponent<playerHunger>().increaseHunger(food);
            Destroy(gameObject);

        }
        
    }

}
