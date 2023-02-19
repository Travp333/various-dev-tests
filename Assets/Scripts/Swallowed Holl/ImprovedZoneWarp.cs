using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedZoneWarp : MonoBehaviour
{
    Collider other = null;
    [SerializeField]
    GameObject dummy;
    [SerializeField]
    GameObject[] dummies;
    [SerializeField]
    float warpOffset;
    bool flipflop;
    bool desiresShift;
    bool subDummy = false;
    bool possibleShift;

    bool shiftBlocked = false;
    public void setShiftBlocked(bool plug){
        shiftBlocked = plug;
    }
    public void setOther(Collider oth){
        other = oth;
    }
    public bool getDesiresShift(){
        return desiresShift;
    }
    public void setDesiresShift(bool plug){
        desiresShift = plug;
    }
    void Update()
    {
        Debug.Log(possibleShift);
        if(flipflop){
                dummy.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - warpOffset);
        }
        else{
                dummy.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + warpOffset);
        }
        if (Input.GetKeyDown(KeyCode.R)){
            if (flipflop){ 
                desiresShift = true;
                if(!shiftBlocked){   
                    if(dummy.gameObject.GetComponent<canShift>().getShiftable()){
                        
                        if(dummy.gameObject.GetComponent<canShift>().getCollider() != null){
                            Debug.Log("Shifting to Breakable Object");
                            dummy.gameObject.GetComponent<canShift>().getCollider().GetComponent<Shatter>().oneShot(0);
                            transform.position = dummy.transform.position;
                            flipflop = false;
                        }
                        else{
                            Debug.Log("Shifting to Main dummy");
                            transform.position = dummy.transform.position;
                            flipflop = false;
                        }

                    }    
                    else {
                        foreach(GameObject d in dummies){
                            if(d.gameObject.GetComponent<canShift>().getShiftable()){
                                Debug.Log("Shifting to subdummy "+ d);
                                transform.position = d.transform.position;
                                flipflop = false;
                                subDummy = true;
                                return;
                            }
                            else{
                                subDummy = false;
                            }
                        }

                    }
                    if(subDummy == false){
                        Debug.Log("Cant Shift!");
                    }
                }
                else{
                    Debug.Log("Shift Blocked!");
                }
            }          
            else if (!flipflop){ 
                desiresShift = true; 
                if(!shiftBlocked){   
                if(dummy.gameObject.GetComponent<canShift>().getShiftable()){
                    
                    if(dummy.gameObject.GetComponent<canShift>().getCollider() != null){
                        Debug.Log("Shifting to Breakable Object");
                        dummy.gameObject.GetComponent<canShift>().getCollider().GetComponent<Shatter>().oneShot(0);
                        transform.position = dummy.transform.position;
                        flipflop = true;
                    }
                    else{
                        Debug.Log("Shifting to Main dummy");
                        transform.position = dummy.transform.position;
                        flipflop = true;
                    }

                }    
                else {
                    foreach(GameObject d in dummies){
                        if(d.gameObject.GetComponent<canShift>().getShiftable()){
                            Debug.Log("Shifting to subdummy "+ d);
                            transform.position = d.transform.position;
                            flipflop = true;
                            subDummy = true;
                            return;
                        }
                        else{
                            subDummy = false;
                        }
                    }

                }
                if(subDummy == false){
                    Debug.Log("Cant Shift!");
                        }
                    }   
                    else{
                        Debug.Log("Shift Blocked");
                    }
                }       
            }
        else{
            desiresShift = false;
        }
        if (dummies[0].gameObject.GetComponent<canShift>().getShiftable() || dummies[1].gameObject.GetComponent<canShift>().getShiftable() || dummies[2].gameObject.GetComponent<canShift>().getShiftable() || dummies[3].gameObject.GetComponent<canShift>().getShiftable() || dummies[4].gameObject.GetComponent<canShift>().getShiftable() || dummy.gameObject.GetComponent<canShift>().getShiftable()){
            possibleShift = true;
        }
        else{
            possibleShift = false;
        }
    }
}

