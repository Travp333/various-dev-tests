//adapted and modified from video "How to make a HEALTH BAR in Unity!" by Brackeys
//https://www.youtube.com/watch?v=BLfNP4Sc_iA
//Author: Sandeep and Travis
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this script will just keep track of the player's various stats and allow other scripts to access and edit them
public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    AudioSource[] painSounds;
    public float hunger = 100;
    public float hp = 100;

    public HungerBar hungerBar;
    public HealthBar healthBar;
    [HideInInspector]
    public bool portalWarp;
    public PlayerDeath playerDeath;
    bool whichOne;

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
        whichOne = transform.root.gameObject.GetComponent<WorldShift>().whichOne;
    }
    public void LoadPlayer()
    {
        if(GetComponent<Movement>().grab.isHolding){
            GetComponent<Movement>().grab.interact.detach();
        }
        PlayerData data = SaveSystem.LoadPlayerStats();
        hunger = data.hunger;
        hp = data.health;
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
        GetComponent<WorldShift>().hollOrReal = data.hollOrReal;

        if(whichOne){
            transform.root.gameObject.GetComponent<WorldShift>().swap(true);
        }
        else{
            transform.root.gameObject.GetComponent<WorldShift>().swap(false);
        }

    }

    public void restoreHP(float healAmount){
        if (hp + healAmount > 100){
            hp = 100;
        }
        else{
            hp += healAmount;
        }
    }

    public void takeDamage(float damage){
        if (hp - damage < 0){
            //Debug.Log("Went from "+hp+" to 0");

            hp = 0;
            playerDeath.Death();

        }
        else {

            //Debug.Log("Went from "+hp+" to "+ Mathf.Round(hp-damage));
            if(hp != Mathf.Round(hp-damage)){
                int index = Random.Range(0, painSounds.Length - 1);
                painSounds[index].Play();
                hp = Mathf.Round(hp-damage);
            }
        }
    }

    void Start()
    {
        //Test line to see if we can set a default start point
        SaveSystem.SavePlayer(this);
        //when game is started, it sets the slider max value to hunger value
        hungerBar.SetMaxHunger(hunger);
        //when game is started, it sets the slider max value to hp value
        healthBar.SetMaxHealth(hp);
    }

    void Update()
    {
        //updates the slider value to match the current hunger value
        hungerBar.SetHunger(hunger);
        //updates the slider value to match the current hp value
        healthBar.SetHealth(hp);
    }

}
