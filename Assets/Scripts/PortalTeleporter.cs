//Adapted and modified from video "Smooth PORTALS in Unity" by Brackeys
//https://www.youtube.com/watch?v=cuQao3hEKfs
//Author: Sandeep
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    [SerializeField]
    AudioSource shiftNoise;
    //reference to player
    public Transform player;
    //reference to receiving portal
    public Transform receiver;

    private bool playerIsOverlapping = false;
    bool justWarped;

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
            player.Rotate(Vector3.up, rotationDiff);

            Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
            player.position = receiver.position + positionOffset;
            player.GetComponent<PlayerStats>().portalWarp = true;
            playerIsOverlapping = false;
            Invoke("resetJustWarped", .5f);
          }
        }
    }

    void resetJustWarped(){
      player.GetComponent<PlayerStats>().portalWarp = false;
    }

    //This method checks whether player is colliding with the portal
    void OnTriggerEnter(Collider other)
    {
      if (other.tag == "Player" && !player.GetComponent<PlayerStats>().portalWarp)
      {
        shiftNoise.Play(); 
        playerIsOverlapping = true;
      }
    }

    //This method checks when the player is not colliding with the portal
    void OnTriggerExit (Collider other)
    {
      if (other.tag == "Player" && !player.GetComponent<PlayerStats>().portalWarp)
      {
        playerIsOverlapping = false;
      }
    }
}
