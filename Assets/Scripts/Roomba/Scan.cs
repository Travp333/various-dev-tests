using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
[RequireComponent(typeof(NavMeshAgent))]
public class Scan : MonoBehaviour
{
    public float magnitude;
    public Transform trashSpawnPos;
    public GameObject[] prefabTrash;
    public Transform[] dest;
    public Transform home;
    public List<string> storage = new List<string>();
    bool full = false;
    Transform target;
    NavMeshAgent agent;
    List<GameObject> trash = new List<GameObject>();
    public Vector3 destination;
    GameObject shootTrash;
    public int storageCapacity = 10;
    int whichTrash;

    GameObject trashMatch;
    GameObject trashMatch2;

    int counter;
    int closestIndex;
    public bool dumping;

    public bool occupied = false;

    public void resetDest(){
        target = home;
    }

    public void sortStorage(){
        //Debug.Log("Sorting List Contents...");
        storage.Sort();
    }

    public void resetTrash(){
        shootTrash.GetComponent<objectSize>().isTrash = true;
    }

    public void dumpElectronics(){
        if(storage.Count > 0){
            dumping = true;
            foreach (GameObject g in prefabTrash){
                if(storage.Count > 0 && g.GetComponent<objectSize>().objectName == storage[0] && g.GetComponent<objectSize>().trashType == objectSize.trash.electronic ){
                    Debug.Log("dumping " + storage[0] + " into " + g.GetComponent<objectSize>().trashType);
                    trashMatch2 = g;
                }
            } 
            if(trashMatch2 != null){
                shootTrash = Instantiate(trashMatch2, trashSpawnPos.position, this.transform.rotation);
                shootTrash.GetComponent<Rigidbody>().velocity = ((trashSpawnPos.position - this.transform.position) * magnitude );
                shootTrash.GetComponent<objectSize>().isTrash = false;
                Invoke("resetTrash", 5f);
                storage.Remove(storage[0]);
                buildTrashList();

                if(storage.Count != 0){
                    Debug.Log("still more to dump!");
                    Debug.Log("finding out which trash  is next in queue");
                    foreach (GameObject g in prefabTrash){
                        if(g.GetComponent<objectSize>().objectName == storage[0]){
                            trashMatch2 = g;
                        }
                    } 
                    Debug.Log("is the next trash the same type as the last?");
                    if(trashMatch2.GetComponent<objectSize>().trashType == objectSize.trash.electronic){
                        Debug.Log(trashMatch2.GetComponent<objectSize>().trashType);
                        Debug.Log("yes it is, doing recursive call");
                        Invoke("dumpElectronics", .5f);
                        dumping = false;
                    }
                    else{
                        Debug.Log("no it isnt, moving to next dump spot");
                        dumping = false;
                        findDest();
                        trashMatch2 = null;
                    } 
                }
                    else if (storage.Count == 0){
                        dumping = false;
                        Debug.Log("Internal Storage Empty! searching for more trash...");
                        full = false;
                        findTrash();
                    }
            }
        }
    }
//----------------------------------------------------------------------------------------------------------------------
    public void dumpMisc(){
        if(storage.Count > 0){
            dumping = true;
            foreach (GameObject g in prefabTrash){
                if(storage.Count > 0 && g.GetComponent<objectSize>().objectName == storage[0] && g.GetComponent<objectSize>().trashType == objectSize.trash.misc ){
                    Debug.Log("dumping " + storage[0] + " into " + g.GetComponent<objectSize>().trashType);
                    trashMatch2 = g;
                }
            } 
            if(trashMatch2 != null){
                shootTrash = Instantiate(trashMatch2, trashSpawnPos.position, this.transform.rotation);
                shootTrash.GetComponent<Rigidbody>().velocity = ((trashSpawnPos.position - this.transform.position) * magnitude );
                shootTrash.GetComponent<objectSize>().isTrash = false;
                Invoke("resetTrash", 5f);
                storage.Remove(storage[0]);
                buildTrashList();

                if(storage.Count != 0){
                    Debug.Log("still more to dump!");
                    Debug.Log("finding out which trash  is next in queue");
                    foreach (GameObject g in prefabTrash){
                        if(g.GetComponent<objectSize>().objectName == storage[0]){
                            trashMatch2 = g;
                        }
                    } 
                    Debug.Log("is the next trash the same type as the last?");
                    if(trashMatch2.GetComponent<objectSize>().trashType == objectSize.trash.misc){
                        Debug.Log("yes it is, doing recursive call");
                        Invoke("dumpMisc", .5f);
                        dumping = false;
                    }
                    else{
                        Debug.Log("no it isnt, moving to next dump spot");
                        dumping = false;
                        findDest();
                        trashMatch2 = null;
                    } 
                }
                else if (storage.Count == 0){
                    dumping = false;
                    Debug.Log("Internal Storage Empty! searching for more trash...");
                    full = false;
                    findTrash();
                }
            }
        }
    }

//----------------------------------------------------------------------------------------------------------------------
    public void dumpRecyclable(){
        if(storage.Count > 0){
            dumping = true;
            foreach (GameObject g in prefabTrash){
                if(storage.Count > 0 &&g.GetComponent<objectSize>().objectName == storage[0] && g.GetComponent<objectSize>().trashType == objectSize.trash.recycleable ){
                    Debug.Log("dumping " + storage[0] + " into " + g.GetComponent<objectSize>().trashType);
                    trashMatch2 = g;
                }
            } 
            if(trashMatch2 != null){
                shootTrash = Instantiate(trashMatch2, trashSpawnPos.position, this.transform.rotation);
                shootTrash.GetComponent<Rigidbody>().velocity = ((trashSpawnPos.position - this.transform.position) * magnitude );
                shootTrash.GetComponent<objectSize>().isTrash = false;
                Invoke("resetTrash", 5f);
                storage.Remove(storage[0]);
                buildTrashList();

                if(storage.Count != 0){
                    Debug.Log("still more to dump!");
                    Debug.Log("finding out which trash  is next in queue");
                    foreach (GameObject g in prefabTrash){
                        if(g.GetComponent<objectSize>().objectName == storage[0]){
                            trashMatch2 = g;
                        }
                    } 
                    Debug.Log("is the next trash the same type as the last?");
                    if(trashMatch2.GetComponent<objectSize>().trashType == objectSize.trash.recycleable){
                        Debug.Log("yes it is, doing recursive call");
                        Invoke("dumpRecyclable", .5f);
                        dumping = false;
                    }
                    else{
                        Debug.Log("no it isnt, moving to next dump spot");
                        dumping = false;
                        findDest();
                        trashMatch2 = null;
                    } 
                }
                else if (storage.Count == 0){
                    dumping = false;
                    Debug.Log("Internal Storage Empty! searching for more trash...");
                    full = false;
                    findTrash();
                }
            }
        }
    }

//----------------------------------------------------------------------------------------------------------------------
    public void dumpFood(){
        if(storage.Count > 0){
            dumping = true;
            foreach (GameObject g in prefabTrash){
                if(storage.Count > 0 &&g.GetComponent<objectSize>().objectName == storage[0] && g.GetComponent<objectSize>().trashType == objectSize.trash.food ){
                    Debug.Log("dumping " + storage[0] + " into " + g.GetComponent<objectSize>().trashType);
                    trashMatch2 = g;
                }
            } 
            if(trashMatch2 != null){
                shootTrash = Instantiate(trashMatch2, trashSpawnPos.position, this.transform.rotation);
                shootTrash.GetComponent<Rigidbody>().velocity = ((trashSpawnPos.position - this.transform.position) * magnitude );
                shootTrash.GetComponent<objectSize>().isTrash = false;
                Invoke("resetTrash", 5f);
                storage.Remove(storage[0]);
                buildTrashList();

                if(storage.Count != 0){
                    Debug.Log("still more to dump!");
                    Debug.Log("finding out which trash  is next in queue");
                    foreach (GameObject g in prefabTrash){
                        if(g.GetComponent<objectSize>().objectName == storage[0]){
                            trashMatch2 = g;
                        }
                    } 
                    Debug.Log("is the next trash the same type as the last?");
                    if(trashMatch2.GetComponent<objectSize>().trashType == objectSize.trash.food){
                        Debug.Log("yes it is, doing recursive call");
                        Invoke("dumpFood", .5f);
                        dumping = false;
                    }
                    else{
                        Debug.Log("no it isnt, moving to next dump spot");
                        dumping = false;
                        findDest();
                        trashMatch2 = null;
                    } 
                }
                else if (storage.Count == 0){
                    dumping = false;
                    Debug.Log("Internal Storage Empty! searching for more trash...");
                    full = false;
                    findTrash();
                }
            }
        }
    }

//----------------------------------------------------------------------------------------------------------------------
    public void dumpInventory(){
        Debug.Log("finding which item to dump...");
        foreach (GameObject g in prefabTrash){
            if(g.GetComponent<objectSize>().objectName == storage[0]){
                Debug.Log("dumping " + storage[0] + " into " + g.GetComponent<objectSize>().trashType);
                trashMatch2 = g;
            }
        } 
        objectSize.trash temp;
        shootTrash = Instantiate(trashMatch2, trashSpawnPos.position, this.transform.rotation);
        shootTrash.GetComponent<Rigidbody>().velocity = ((trashSpawnPos.position - this.transform.position) * magnitude );
        shootTrash.GetComponent<objectSize>().isTrash = false;
        storage.Remove(storage[0]);
        buildTrashList();
        if(trashMatch2.GetComponent<objectSize>().trashType == objectSize.trash.electronic){
            temp = objectSize.trash.electronic;
        }
        else if (trashMatch2.GetComponent<objectSize>().trashType == objectSize.trash.recycleable){
            temp = objectSize.trash.recycleable;
        }
        else if (trashMatch2.GetComponent<objectSize>().trashType == objectSize.trash.food){
            temp = objectSize.trash.food;
        }
        else{
            temp = objectSize.trash.misc;
        } 
        
        if(storage.Count != 0){
            Debug.Log("still more to dump!");
            Debug.Log("finding out which trash  is next in queue");
            foreach (GameObject g in prefabTrash){
                if(g.GetComponent<objectSize>().objectName == storage[0]){
                    trashMatch2 = g;
                }
            } 
            Debug.Log("is the next trash the same type as the last?");
            if(trashMatch2.GetComponent<objectSize>().trashType == temp){
                Debug.Log("yes it is, doing recursive call");
                dumpInventory();
            }
            else{
                Debug.Log("no it isnt, moving to next dump spot");
                findDest();
            } 
        }
        else if (storage.Count == 0){
            dumping = false;
            Debug.Log("Internal Storage Empty! searching for more trash...");
            full = false;
            findTrash();
        }
    }

    Transform GetClosestTrash(List<GameObject> trash){

        //Debug.Log("Finding closest Trash...");
        counter = 0;
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        buildTrashList();
        foreach (GameObject t in trash){
            float dist = Vector3.Distance(t.gameObject.transform.position, currentPos);
            if (dist < minDist && ! t.GetComponent<objectSize>().isTargeted){
                tMin = t.transform;
                minDist = dist;
                closestIndex = counter;
            }
            counter += 1;
        }
        trash[closestIndex].GetComponent<objectSize>().isTargeted = true;
        counter = 0;
        closestIndex = 0;
        return tMin;
    }

    void findTrash(){
        
        //Debug.Log("Deciding where to go next...");
        buildTrashList();
        if(trash.Count > 0){
            //Debug.Log("There is still trash in the world!");
            //Debug.Log("assigning target to closest trash");
            target = GetClosestTrash(trash);
            Debug.DrawRay(transform.position, target.position - transform.position, Color.red, .5f);
        }
        else if (trash.Count == 0 && storage.Count == 0){
            //Debug.Log("no more trash left, going home...");
            target = home;
        }

    }
    public void addTrash(GameObject b){
        trash.Add(b);
    }

    public void removeTrash(GameObject b){
        trash.Remove(b);
    }


    public void buildTrashList(){
        //Debug.Log("Updating List of Trash...");
        trash.Clear();
        foreach (GameObject g in GameObject.FindObjectsOfType<GameObject>()){
            if (g.gameObject.GetComponent<objectSize>() != null){
                objectSize item = g.gameObject.GetComponent<objectSize>();
                if(item.isTrash){
                    trash.Add(g);
                }
                
            }
            
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        buildTrashList();
        destination = agent.destination;
        findTrash();
    }

    void findDest(){
        //Debug.Log("Searching for which prefab this object is...");
        foreach (GameObject g in prefabTrash){
            if(g.GetComponent<objectSize>().objectName == storage[0]){
                trashMatch = g;
                //Debug.Log("found " + trashMatch);
            }
        }    
            if(trashMatch.GetComponent<objectSize>().trashType == objectSize.trash.electronic){
                target = dest[2];
            }
            else if (trashMatch.GetComponent<objectSize>().trashType == objectSize.trash.recycleable){
                target = dest[0];
            }
            else if (trashMatch.GetComponent<objectSize>().trashType == objectSize.trash.food){
                target = dest[1];
            }
            else{
                target = dest[3];
            }
    }

    void OnTriggerEnter(Collider collider){
        //Debug.Log("Collided with a thing");
        if(storage.Count >= storageCapacity){
            full = true;
            //Debug.Log("Dont have room for the thing");
        }
        if (collider.gameObject.GetComponent<objectSize>() != null && collider.gameObject.GetComponent<objectSize>().isTrash && !full){
            //Debug.Log("removing isTrash tag from object");
            collider.gameObject.GetComponent<objectSize>().isTrash = false;
            //Debug.Log("Adding " + collision.gameObject.GetComponent<objectSize>().objectName + " to internal storage");
            storage.Add(collider.gameObject.GetComponent<objectSize>().objectName);         
            if(storage.Count >= storageCapacity){
                full = true;
                //Debug.Log("now at max capacity");
            }
            //Debug.Log("Destroying world Prop");
            Destroy(collider.gameObject);
            buildTrashList();
            sortStorage();
            
            if(trash.Count > 0 && !full){
                //Debug.Log("There is still more trash to pick up and you have room to do it!");
                target = GetClosestTrash(trash);
                Debug.DrawRay(transform.position, target.position - transform.position, Color.red, .5f);
            }
            else if (trash.Count == 0 && storage.Count == 0){
                //Debug.Log("No trash in world or inventory");
                buildTrashList();
                if (trash.Count == 0 && storage.Count == 0){
                    //Debug.Log("still no trash in world or inventory, going home");
                    target = home;
                }
                
            }
            else if (full || trash.Count == 0 && storage.Count != 0){
                //Debug.Log("cant hold any more trash, or there is not trash left in the world to pick up. heading to dump");
                buildTrashList();
                findDest();
            }
        }
        else if(collider.gameObject.GetComponent<objectSize>() != null && collider.gameObject.GetComponent<objectSize>().isTrash && (full || trash.Count == 0 && storage.Count != 0)){
            //Debug.Log("Picked up the last bit of trash in the world, heading to dump...");
            buildTrashList();
            findDest();
        }
    }

    // Update is called once per frame
    void Update()   
    {
        if(target != null && !dumping){
            if(Vector3.Distance(destination, target.position) > .1f){
                destination = target.position;
                agent.destination = destination;
            }
        }
        else if (target == null && !dumping){
            buildTrashList();
            findTrash();
        }
        if (occupied && storage.Count >= storageCapacity){
            destination = home.position;
            agent.destination = destination;
        }
        //else if (occupied && storage.Count < storag)
    }
}

