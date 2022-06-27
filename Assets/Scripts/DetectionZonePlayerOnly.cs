using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class DetectionZonePlayerOnly : MonoBehaviour {

	MovingSphere sphere = default;
	public GameObject player = default;
	public bool isPlayerOnly;


	[SerializeField]
	UnityEvent onFirstEnter = default, onLastExit = default;
	List<Collider> colliders = new List<Collider>();

	void Start() {
		sphere = player.GetComponent<MovingSphere>();
	}
    void OnTriggerEnter (Collider other) {
		if (!isPlayerOnly){
			if (colliders.Count == 0) {
				onFirstEnter.Invoke();
			}
			colliders.Add(other);
		}
		else {
			if (other.gameObject.tag == "Player"){	
					onFirstEnter.Invoke();
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (!isPlayerOnly){
			if (colliders.Remove(other) && colliders.Count == 0) {
				onLastExit.Invoke();
			}
		}
		else {
			if (other.gameObject.tag == "Player"){
				onLastExit.Invoke();
			}
		}
	}
}
