using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shittydrive : MonoBehaviour
{
    // Start is called before the first frame update
    bool autoPilot;
    public GameObject Goal;
    float rand;
    void Start()
    {
        rand = Random.Range(-0.4f, 0.4f);
    }

    void CalculateDistance(){
        float distance = Vector3.Distance(this.transform.position, Goal.transform.position);
        Debug.Log(distance);
        Debug.DrawRay(this.transform.position, (Goal.transform.position - this.transform.position), Color.green, 3 );
    }

    void CalculateAngle(){
        Vector3 up = this.transform.forward;
        Vector3 Distance = Goal.transform.position - this.transform.position;
        float angle = Mathf.Acos(Vector3.Dot(up, Distance) / (up.magnitude * Distance.magnitude));
        int clockwise = 1; 
        if (Vector3.Cross(up, Distance).z < 0){
            clockwise = -1;
        }
        Debug.DrawRay(this.transform.position, up * 100, Color.red, 3 );
        this.transform.Rotate(0, 0, angle * clockwise * Mathf.Rad2Deg * rand);
    }
    float autoSpeed = .1f;
    void AutoPilot(){
        Debug.DrawRay(this.transform.position, (Goal.transform.position - this.transform.position), Color.green, 3 );
        CalculateAngle();
        this.transform.Translate(this.transform.up * autoSpeed * rand * 10, Space.World );
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T)){
            autoPilot = !autoPilot;
        }
        if(autoPilot)
            AutoPilot();
    }
}
