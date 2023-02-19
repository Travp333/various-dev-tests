using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWP : MonoBehaviour
{
    [SerializeField]
    GameObject[] waypoints;
    int currentWP;
    [SerializeField]
    float speed = 10.0f;
    [SerializeField]
    float rotSpeed = 10.0f; 
    [SerializeField]
    float lookAhead = 10f;
    GameObject tracker;

    private void Start() {
        tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = this.transform.position;
        tracker.transform.rotation = this.transform.rotation;
    }
    private void ProgressTracker(){
        if (Vector3.Distance(tracker.transform.position, this.transform.position) > lookAhead) return;

        if(Vector3.Distance(tracker.transform.position, waypoints[currentWP].transform.position)<3)
            currentWP++;
        if (currentWP >= waypoints.Length)
            currentWP = 0;

        tracker.transform.LookAt(waypoints[currentWP].transform);
        tracker.transform.Translate(0, 0, (speed+20) * Time.deltaTime);
    }
    void Update()
    {
        ProgressTracker();
        Quaternion lookatWP =  Quaternion.LookRotation(tracker.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookatWP, rotSpeed*Time.deltaTime);
        this.transform.Translate(0, 0, (speed) * Time.deltaTime);
    }
}
