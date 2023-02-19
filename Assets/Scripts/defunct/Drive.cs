using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    //public float speed = 10f;
    //public float rotationSpeed = 100f;
    public Transform goal;
    [SerializeField]
    float autoSpeed = .1f;
    [SerializeField]
    float autoRotateSpeed = .02f;
    bool autoPilot;
    [SerializeField]
    float range = 35;

    
    void AutoPilot(){
            transform.Translate(this.transform.forward * autoSpeed, Space.World);
            CalculateAngle();
    }
	private void Update()
	{
        if(CalculateDistance() > range){
            AutoPilot();
        }
	}
    //manual controls with arrow keys
//	void FixedUpdate()
 //   {
        //float translation = Input.GetAxis("VerticalDpad") * speed;
        //float rotation = Input.GetAxis("HorizontalDpad") * rotationSpeed;
        //translation *= Time.deltaTime;
       // rotation *= Time.deltaTime;
       // transform.Translate(0, 0, translation);
       // transform.Rotate(0, rotation, 0);
  //  }
    
    float CalculateDistance() {
        Vector3 tP = this.transform.position;
        Vector3 fP = goal.position;

        float distance = Vector3.Distance(tP, fP);

        return distance;
        //float distance = Mathf.Sqrt(
        //    Mathf.Pow(tP.x - fP.x, 2) + Mathf.Pow(tP.y - fP.y, 2) + Mathf.Pow(tP.z - fP.z, 2));
    }

    void CalculateAngle() {
        Vector3 tF = this.transform.forward;
        Vector3 fD = goal.position - this.transform.position;

        float dot = Vector3.Dot(tF, fD);
        float angle = Mathf.Acos(dot / (tF.magnitude * fD.magnitude));

        //Debug.Log(angle * Mathf.Rad2Deg);

        //Debug.DrawRay(this.transform.position, tF*20, Color.blue, 3);
        //Debug.DrawRay(this.transform.position, fD, Color.white, 3);

        int clockwise = 1;
        if (Vector3.Cross(tF, fD).y < 0) {
            clockwise = -1;
        }


        this.transform.Rotate(0, (angle * Mathf.Rad2Deg * clockwise) * autoRotateSpeed, 0);


    }
   
}
