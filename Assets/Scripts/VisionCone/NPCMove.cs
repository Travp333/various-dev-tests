using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    jerryAnimScript anim;
    [SerializeField]
    public GameObject carrot;
    [SerializeField]
    public float runSpeed, defaultSpeed, slowSpeed;

    float updateCount = 0;
    [SerializeField]
    public float updateCap = 60;
    [SerializeField]
    private GameObject uninfectedList;
    private uninfectedList list;
    [SerializeField]
    float stickLength = 5f;
    [HideInInspector]
    public NavMeshAgent agent;

    [SerializeField]
    private float turnRate;
    public bool scared;
    [SerializeField]
    private GameObject home;

    [SerializeField]
    public bool infected;
    MaterialSelector select;
    private int counter;
    GameObject Min;
    [SerializeField]
    private float fearRadius;
    bool gate;
    [SerializeField]
    public float slowCoolDown;
    NavMeshPath path;
    float counter2 = 0f;
    float counter3 = 0f;
    Vector3 meshy;
    [SerializeField]
    LayerMask layerMask;
    public void infectAgain(Collider other)
    {
        list.Infected(other.gameObject);
        other.gameObject.GetComponent<NPCMove>().infected = true;
        flipGate();
        list.updateInfectedList();

    }
    void flipGate(){
        gate = true;
        Invoke("resetGate", slowCoolDown);
    }
    void resetGate(){
        gate = false;
    }

    void GetClosestInfected(){
        Min = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        if(list.infected.Count > 0){
            foreach (GameObject g in list.infected){
                float dist = Vector3.Distance(g.gameObject.transform.position, currentPos);
                if (dist < minDist){
                    
                    Min = g;
                    minDist = dist;
                }
            }
        }
        else if (list.infected.Count <= 0){
            Min = home;
        }

    }

    void GetClosestUninfected(){
        Min = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        if(list.unscareduninfected.Count > 0 ){
            foreach (GameObject g in list.unscareduninfected){
                float dist = Vector3.Distance(g.gameObject.transform.position, currentPos);
                if (dist < minDist){
                    Min = g;
                    minDist = dist;
                }
            }
        }
        else if(list.unscareduninfected.Count <= 0 && list.scareduninfected.Count > 0){
            foreach (GameObject g in list.scareduninfected){
                float dist = Vector3.Distance(g.gameObject.transform.position, currentPos);
                if (dist < minDist){
                    Min = g;
                    minDist = dist;
                }
            }
        }
        else if (list.unscareduninfected.Count <= 0 && list.scareduninfected.Count <= 0){
            Min = home;
        }

    }
    public void setScared(){
        scared = true;
        list.updateUninfectedList();
    }
    public void resetScared(){
        scared = false;
        list.updateUninfectedList();
    }

    public Vector3 RandomNavmeshLocation(float radius) {
         Vector3 randomDirection = Random.insideUnitSphere * radius;
         randomDirection += transform.position;
         NavMeshHit hit;
         Vector3 finalPosition = Vector3.zero;
         if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
             finalPosition = hit.position;            
         }
         return finalPosition;
     }

    void Start()
    {
        anim = GetComponent<jerryAnimScript>();
        meshy = RandomNavmeshLocation(400f);
        path = new NavMeshPath();
        list = uninfectedList.GetComponent<uninfectedList>();
        select = GetComponent<MaterialSelector>();
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if(!infected){
            select.Select(0);
            GetClosestInfected();
            float dist = Vector3.Distance(this.transform.position, Min.transform.position);
            if(dist < fearRadius && !scared){
                setScared();
            }
            if(scared){
                agent.speed = runSpeed;
                //carrot.transform.position
                //Debug.Log(this.transform.position);
                Vector3 dirToThreat = this.transform.position - Min.transform.position;
                dirToThreat.Normalize();
                Vector3 newPos = (transform.position + dirToThreat);
                NavMesh.CalculatePath(this.transform.position, newPos, NavMesh.AllAreas, path);
                agent.SetPath(path);
                //agent.SetDestination(dirToThreat);
                //agent.destination = path;
                //Debug.DrawRay(this.transform.position, newPos);
                Quaternion toRotation = Quaternion.LookRotation(dirToThreat, Vector3.up);
                this.transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, (turnRate) * Time.deltaTime);
                updateCount = updateCount + Time.deltaTime;
                if(updateCount > updateCap && dist > fearRadius){
                    resetScared();
                    updateCount = 0;
                }
            }
            else{
                agent.speed = defaultSpeed;
                if(counter2 < 5){
                    counter2 += Time.deltaTime;
                }
                else{
                    //Debug.Log("AGGHHH");
                    meshy = RandomNavmeshLocation(400f);
                    counter2 = 0;
                }
                NavMesh.CalculatePath(this.transform.position, meshy, NavMesh.AllAreas, path);
                agent.SetPath(path);
                //agent.destination = home.transform.position;
            }
            
            //this.transform.Rotate(0, this.transform.rotation.y + turnRate, 0);
        }
        else{
            select.Select(1);
            GetClosestUninfected();
            NavMesh.CalculatePath(this.transform.position, Min.transform.position, NavMesh.AllAreas, path);
            agent.SetPath(path);
            //agent.destination = Min.transform.position; 
            if(gate){
                agent.speed = slowSpeed;
            }
            else{
                agent.speed = defaultSpeed;
            }

        }
    }
}
