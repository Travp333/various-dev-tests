using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    public GameObject shatterPrefab;
    [Tooltip("What shattered mesh spawns")]
    public GameObject shatterSpawnPos;
    [Tooltip("Where the shattered mesh spawns")]
    Renderer color;
    float darken = 1f;
    [SerializeField]
    [Tooltip("How many hits a prop will take before breaking")]
    float hitPoints = 2;
    int Damagestate = 0;

    float breakSpeed = 40f;

    void OnCollisionEnter(Collision other) {
        // does this object have a ridigbody? is the object colliding with another object past the breaking speed? if so, break it. dont let that object be a player. that colliding objects mass must be greater than or equal to the current obejcts mass
        //consider doing better calculations here, ie dot product of collision normal and collision velocity(relative velocity of both bodies) times the mass of the other collider
        if (((other.gameObject.GetComponent<Rigidbody>() != null) && other.gameObject.GetComponent<Rigidbody>().velocity.magnitude > breakSpeed) || this.gameObject.GetComponent<Rigidbody>().velocity.magnitude > breakSpeed && other.gameObject.tag != "Player" && other.gameObject.GetComponent<Rigidbody>().mass >= this.gameObject.GetComponent<Rigidbody>().mass){
            oneShot(0);
        }
    }
    void Start() {
        color = GetComponent<Renderer>();
    }
    public void oneShot(float time){
        Invoke("spawnShatter", time);
    }
    public void takeDamage(){
        if (Damagestate < hitPoints){
            foreach(Material m in color.materials ){
                m.SetColor("_EmissionColor", Color.grey * darken);
                m.SetColor("_Color", Color.grey * darken);
                Damagestate++;
                darken -= .2f;
            }
        }
        else if ( Damagestate >= hitPoints){
            Invoke("spawnShatter", 0);
        }
    }
    void spawnShatter(){
        Instantiate(shatterPrefab, shatterSpawnPos.transform.position, shatterSpawnPos.transform.rotation);
        Destroy(this.gameObject);
    }
   // public void Break(Collision other){
       // if(other.gameObject.tag == "Player" ){
        //    takeDamage();
            
       // }

   // }
    //void OnCollisionEnter(Collision other) {
      //  Break(other);
    //}
}
