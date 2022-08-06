using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis Parks
//This controls the behavior of the conveyor belt object, pushing rigidbodies that fall upon it
public class ConveyorBelt : MonoBehaviour
{
    [SerializeField]
    bool isSideways;
    [SerializeField]
    bool isEndPiece = false;
    [SerializeField]
    float speed;
    [SerializeField]
    [Tooltip("amount of spin added to objects on the conveyor belt")]
    float spinAmount;
    [SerializeField]
    GameObject grav;
    List<GameObject> pushingObjects = new List<GameObject>();
    [SerializeField]
    bool tempFlip;
    [SerializeField]
    float reverseTime;
    [SerializeField]
    GameObject Belt;
    BeltSpeedController speedController;
    public bool subGate;
    private void Start() {
        speedController = Belt.GetComponent<BeltSpeedController>();
    }
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.GetComponent<Rigidbody>().isKinematic == false && other.gameObject.layer != 16 && other.gameObject.tag != "Breakable" && other.gameObject.tag != "Explosive"&& other.gameObject.tag != "Player" && pushingObjects.Contains(other.gameObject) == false){
            //Debug.Log("an object with a rigidbody just got added");
            pushingObjects.Add(other.gameObject);
        }
        if( other.transform.parent != null){
            if(other.transform.parent.gameObject.GetComponent<Rigidbody>() != null  && other.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic == false && other.transform.parent.gameObject.tag != "Breakable" && other.transform.parent.gameObject.tag != "Explosive" && other.transform.parent.gameObject.tag != "Player" && pushingObjects.Contains(other.transform.parent.gameObject) == false){
                //Debug.Log("an object with a rigibody in its parent just got added");
                pushingObjects.Add(other.transform.parent.gameObject);
            }
        }
        if(other.gameObject.TryGetComponent<BoxCollider>(out var coll)){
            if( other.gameObject.tag == "Breakable" || other.gameObject.tag == "Explosive"&& other.gameObject.tag != "Player"){
                if(coll.isTrigger == false && pushingObjects.Contains(other.gameObject) == false){
                    //Debug.Log("A breakable object with a Box collider just got added");
                    pushingObjects.Add(other.gameObject);
                }    
            }
        }
        if(other.gameObject.TryGetComponent<MeshCollider>(out var coll2)){
            if (other.gameObject.tag == "Breakable" || other.gameObject.tag == "Explosive" && other.gameObject.tag != "Player"){
                if(coll2.isTrigger == false && pushingObjects.Contains(other.gameObject) == false){
                    //Debug.Log("A breakable object with a Mesh collider just got added");
                    pushingObjects.Add(other.gameObject);
                }
            }
        }
        if(other.gameObject.tag == "Player" && pushingObjects.Contains(other.gameObject.transform.root.gameObject) == false){
            //Debug.Log("A player just got added");
            //other.transform.parent.GetChild(0).GetChild(0).GetChild(2).GetComponent<disableDynamicBone>().toggle(true);
            pushingObjects.Add(other.gameObject.transform.root.gameObject);
        }
    }
    void OnTriggerExit(Collider other) {
        if(other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.GetComponent<Rigidbody>().isKinematic == false && other.gameObject.layer != 16 && other.gameObject.tag != "Player" && pushingObjects.Contains(other.gameObject) == true){
            //Debug.Log("An object just got removed");
            pushingObjects.Remove(other.gameObject);
            if(isEndPiece){
                other.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Rigidbody>().velocity + this.transform.right * (speed);
            }
        }
        if(other.transform.parent != null){
            if(other.transform.parent.gameObject.GetComponent<Rigidbody>() != null && other.transform.parent.gameObject.tag != "Player" && pushingObjects.Contains(other.transform.parent.gameObject) == true){
                //Debug.Log("An object with a parent rigidbody just got removed");
                pushingObjects.Remove(other.transform.parent.gameObject);
                if(isEndPiece){
                    //Debug.Log("Lil Speed Boost");
                    other.transform.parent.gameObject.GetComponent<Rigidbody>().velocity = other.transform.parent.gameObject.GetComponent<Rigidbody>().velocity + this.transform.right * (speed);
                }
            }
        }
        if(other.gameObject.tag == "Player" && pushingObjects.Contains(other.gameObject.transform.root.gameObject) == true){
            //Debug.Log("A player just got removed");
            pushingObjects.Remove(other.gameObject.transform.root.gameObject);
            //other.transform.parent.GetChild(0).GetChild(0).GetChild(2).GetComponent<disableDynamicBone>().toggle(false);
            if(isSideways){
                if(grav!=null){
                    grav.GetComponent<GravityPlane>().gravity = 9.81f;
                }

            }
            if(isEndPiece){
                //Debug.Log("Lil Speed Boost");
                other.gameObject.transform.root.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.transform.root.gameObject.GetComponent<Rigidbody>().velocity + this.transform.right * (speed);
            }
        }
    }
    public void reReverseDirection(){
        speed = -speed;
        speedController.reverseDirection();
        subGate = false;
    }

    public void reverseDirection(){
        if(!subGate){
            speed = -speed;
            speedController.reverseDirection();
            if(tempFlip){
                Invoke("reReverseDirection", reverseTime);
                subGate = true;
            }
        }
    }


    //void Update() {
        //foreach ( GameObject p in pushingObjects){
            //Debug.Log(p.name);
        //}
    //}
      void FixedUpdate()
    {
        for (int i = 0; i < pushingObjects.Count; i++){
            if (pushingObjects[i] == null){
                //Debug.Log("REMOVED VIA DESTRUCTION");
                pushingObjects.Remove(pushingObjects[i].gameObject);
            }
            else if(pushingObjects[i].gameObject.layer == 16){
                //Debug.Log("REMOVED VIA LAYER");
                pushingObjects.Remove(pushingObjects[i].gameObject);
            }
            else{
                if(speed != 0){
                    if(!isSideways){
                        //speed = 4;
                        //for (float y = 0; y <= speed; y++)
                        //{
                            
                        //    pushingObjects[i].transform.position = Vector3.Lerp(pushingObjects[i].transform.position, pushingObjects[i].transform.position + this.transform.right * (speed * Time.deltaTime), (y/speed));
                        //    Debug.Log("Y is "+ y + " and y/speed is " + y/speed);  
                        //}
                        pushingObjects[i].transform.position = pushingObjects[i].transform.position + this.transform.right * (speed * Time.deltaTime);
                       
                    }
                    else if(pushingObjects[i].gameObject.tag == "Player" && isSideways){
                        if(grav!= null){
                            grav.GetComponent<GravityPlane>().gravity = 0f;
                            pushingObjects[i].transform.position = pushingObjects[i].transform.position + this.transform.right * (speed * Time.deltaTime);
                        }
                        else{
                            Debug.Log("Tagged as sideways but no gravity source connected!");
                        }
                    }
                }
                if(spinAmount != 0){
                    pushingObjects[i].transform.Rotate(new Vector3(0f, Time.deltaTime * spinAmount), Space.World);
                }
            }
        }
    }
}


