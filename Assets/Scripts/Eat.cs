//Authors: Brian Meginness, Travis Parks
//Debugging: Brian Meginness
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls food objects being consumed
public class Eat : MonoBehaviour
{

    [SerializeField]
    float food = default;
    [SerializeField]
    bool respawn = true;
    [SerializeField]
    float respawnDur = 5;

    public void respawnFood(){
        this.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        this.GetComponent<BoxCollider>().enabled = true;
    }

    public void hideFood(){
        this.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        this.GetComponent<BoxCollider>().enabled = false;
        if (respawn){ 
            Invoke("respawnFood", respawnDur);

        }
    }

    //Eats the object, destroying it and adding the appropriate value to player's hunger
    public void eatFood(playerHunger ph)
    {
        hideFood();
        ph.increaseHunger(food);

    }

    //For being called in external methods, such as when eating from held (See Grab.cs)
    public void eatFood(){
        
        foreach(GameObject G in GameObject.FindGameObjectsWithTag("Player")){
            if(G.GetComponent<Movement>()){
                G.GetComponent<playerHunger>().increaseHunger(food);
            }
        }

        Destroy(gameObject);
    }
    

    public void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){

            if (other.transform.parent.GetComponentInChildren<Movement>().hungerDive()){
                eatFood(other.transform.parent.GetComponentInChildren<playerHunger>());
            }
            else if (!other.transform.parent.GetComponentInChildren<Grab>().isHolding) {
                GameObject copy = Instantiate(this.gameObject);
                if (this.GetComponent<BoxCollider>() && this.GetComponent<BoxCollider>().isTrigger){ 
                hideFood();
                }
                Interact playerInteract = other.transform.parent.GetComponentInChildren<Interact>();

                copy.GetComponent<Floater>().enabled = false;
                copy.GetComponent<BoxCollider>().isTrigger = false;
                copy.GetComponent<Rigidbody>().useGravity = true;
                playerInteract.pickUp(copy);
            }
        
        } 
        
    }

}
