using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : MonoBehaviour
{
    [SerializeField]
    private GameObject Agent;
/// <summary>
/// OnTriggerEnter is called when the Collider other enters the trigger.
/// </summary>
/// <param name="other">The other Collider involved in this collision.</param>
void OnTriggerEnter(Collider other)
{
    //Agent.GetComponent<NPCMove>().
}
}
