//Adapted and modified from video "Smooth PORTALS in Unity" by Brackeys
//https://www.youtube.com/watch?v=cuQao3hEKfs
//Author: Sandeep
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    [SerializeField]
    GameObject ragdollSpawn;
    [SerializeField]
    GameObject largeSpawn;
    [SerializeField]
    AudioSource shiftNoise;
    //reference to player
    Transform player;
    Transform Object;
    //reference to receiving portal
    public Transform receiver;

    private bool playerIsOverlapping = false;
    private bool objectIsOverlapping = false;
    bool justWarped;
    private bool ragdollOverlapping = false;

    void Start(){
      //wont work with multiple players
      player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }
    void Update()
    {
        //Moves the player's position
        if (playerIsOverlapping)
        {
          //the distance between the player and portal
          Vector3 portalToPlayer = player.position - transform.position;

          float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

          if (dotProduct < 0f)
          {
            float rotationDiff = Quaternion.Angle(transform.rotation, receiver.rotation);
            rotationDiff += 180;
            //player.Rotate(Vector3.up, rotationDiff);

            Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
            player.position = receiver.position + positionOffset;
            player.GetComponent<PlayerStats>().portalWarp = true;
            playerIsOverlapping = false;
            Invoke("resetJustWarpedP", .5f);
          }
        }
        if (ragdollOverlapping){
          //the distance between the player and portal
          Vector3 portalToPlayer = Object.position - transform.position;
          float rotationDiff = Quaternion.Angle(transform.rotation, receiver.rotation);
          rotationDiff += 180;
          Object.Rotate(Vector3.up, rotationDiff);
          Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
          Object.position = receiver.transform.GetChild(0).gameObject.GetComponent<PortalTeleporter>().ragdollSpawn.transform.position + positionOffset;
          Object.GetComponent<objectSize>().portalWarp = true;
          ragdollOverlapping = false;
          Invoke("resetJustWarpedO", .5f);
        }
        else if(objectIsOverlapping){

          if(Object.GetComponent<objectSize>().sizes == objectSize.objectSizes.large){

            //Debug.Log("Large object overlapping, object is " + Object);
            //the distance between the player and portal
            Vector3 portalToObject = Object.position - largeSpawn.transform.position;
            float rotationDiff = Quaternion.Angle(largeSpawn.transform.rotation, receiver.rotation);
            rotationDiff += 180;
            Object.Rotate(Vector3.up, rotationDiff);
            Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToObject;
            Object.position = receiver.position + positionOffset;
            Object.GetComponent<objectSize>().portalWarp = true;
            objectIsOverlapping = false;
            Invoke("resetJustWarpedO", 1f);
          }
          else{

            //Debug.Log("object overlapping, object is " + Object);
            //the distance between the player and portal
            Vector3 portalToObject = Object.position - transform.position;
            float rotationDiff = Quaternion.Angle(transform.rotation, receiver.rotation);
            rotationDiff += 180;
            Object.Rotate(Vector3.up, rotationDiff);
            Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToObject;
            Object.position = receiver.position + positionOffset;
            Object.GetComponent<objectSize>().portalWarp = true;
            objectIsOverlapping = false;
            Invoke("resetJustWarpedO", 1f);

          }


 
        }
    }

    void resetJustWarpedP(){
      player.GetComponent<PlayerStats>().portalWarp = false;
    }
    void resetJustWarpedO(){
      //Debug.Log("reset just warped status");
      if(Object!= null){
        Object.GetComponent<objectSize>().portalWarp = false;
      }
      


    }

    //This method checks whether player is colliding with the portal
    void OnTriggerEnter(Collider other)
    {
      if(other.tag == "ragdoll" && other.gameObject.layer != 16){
        shiftNoise.Play();
        Object = other.gameObject.transform.root.gameObject.transform;
        //Debug.Log("THIS IS " + Object);
        ragdollOverlapping = true;
      }
      else if (player.GetComponent<PlayerStats>() != null && other.tag == "Player" && !player.GetComponent<PlayerStats>().portalWarp)
      {
        shiftNoise.Play(); 
        playerIsOverlapping = true;
      }
      else if (other.gameObject.GetComponent<objectSize>() != null){
        if(other.gameObject.layer == 13 && !other.gameObject.GetComponent<objectSize>().portalWarp){
          //Debug.Log("Detected object with collider and rigidbody");
          shiftNoise.Play();
          Object = other.gameObject.transform;
          objectIsOverlapping = true;
        }

      }
      else if (other.gameObject.transform.parent != null){
        if (other.gameObject.transform.parent.gameObject.GetComponent<objectSize>() != null){
          if(other.gameObject.transform.parent.gameObject.layer == 13 && !other.gameObject.transform.parent.gameObject.GetComponent<objectSize>().portalWarp){
              //Debug.Log("Detected object with a collider and a rigidbody in parent");
              shiftNoise.Play();
              Object = other.gameObject.transform.parent.transform;
              objectIsOverlapping = true;
          }

        }
      }
      //add an NPC check here too so npcs can get tp'd as well
      
    }

    //This method checks when the player is not colliding with the portal
    void OnTriggerExit (Collider other)
    {
      if (other.tag == "Player" && !player.GetComponent<PlayerStats>().portalWarp)
      {
        playerIsOverlapping = false;
      }
      else if (other.gameObject.layer == 13){
        objectIsOverlapping = false;
      }
    }
}
