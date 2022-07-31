using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    [SerializeField]
    LayerMask mask;
    jerryAnimScript anim;
    [SerializeField]
    [Tooltip("Speeds of default roaming, boosted scared speed, and a currently unused slow speed(injured?)")]
    public float runSpeed, defaultSpeed, slowSpeed;
    [SerializeField]
    [Tooltip("Speeds of Zombie roaming, chasing speed, and post attack cooldown speed")]
    public float ZrunSpeed, ZdefaultSpeed, ZslowSpeed;
    float updateCount = 0;
    [SerializeField]
    [Tooltip("How long an NPC stays scared")]
    public float updateCap = 60;
    GameObject uninfectedList;
    private uninfectedList list;
    [HideInInspector]
    public NavMeshAgent agent;
    [SerializeField]
    [Tooltip("the rate at which the npc turns")]
    private float turnRate;

    MaterialSelector select;
    GameObject Min;
    [SerializeField]
    [Tooltip("Radius where npcs get scared by infected")]
    private float fearRadius;

    [SerializeField]
    [Tooltip("distance at which the npc will use panicked movement, should be less than fearRadius")]
    float criticalDist;

    [SerializeField]
    [Tooltip("Amount of time infected stay slow after infecting someone")]
    public float slowCoolDown;
    NavMeshPath path;
    float counter2 = 0f;
    Vector3 meshy;
    [SerializeField]
    [Tooltip("Upper cap of how long an npc waits until roaming to new position")]
    float roamTimerUp = 15;
    [SerializeField]
    [Tooltip("Lower cap of how long an npc waits until roaming to new position")]
    float roamTimerLow = 5;
    public bool chasing;
    public bool scared;
    [SerializeField]
    public bool infected;
    public bool gate;
    RaycastHit hit;
    Quaternion spreadAngle;
    Vector3 newVector;
    float raycastCount;
    [SerializeField]
    [Tooltip("How often an npc will recalculate its running away path(Upper Bound)")]
    float raycastCapUpper;
    [SerializeField]
    [Tooltip("How often an npc will recalculate its running away path (Lower Bound)")]
    float raycastCapLower;
    float raycastCap;
    [SerializeField]
    [Tooltip("Angle at which the ai will choose its running away route from")]
    float runningAngle;
    Vector3 raycast;
    float magnitude;
    [SerializeField]
    [Tooltip("Space between player and the hit point that cannot be selected by the npc when choosing an escape route")]
    float raycastBuffer;

    void Start()
    {
        raycastCap = Random.Range(raycastCapLower, raycastCapUpper);
        raycastCount = raycastCap;
        //finds the game object holding the list script
        foreach(GameObject g in GameObject.FindObjectsOfType<GameObject>()){
            if(g.GetComponent<uninfectedList>()!=null){
                uninfectedList = g;
            }
        }
        //plugging references
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
            list.updateUninfectedList(this.gameObject);
            //You are uninfected
            select.Select(0);
            if(list.infected.Count > 0){
                GetClosestInfected();
                float dist = Vector3.Distance(this.transform.position, Min.transform.position);
                if(dist > fearRadius){
                    updateCount = updateCount + Time.deltaTime;
                    if(updateCount > updateCap){
                        Debug.Log("Reset Scared!");
                        resetScared();
                        updateCount = 0;
                    }
                }
                else if(dist < fearRadius){
                    //There are infected in the level, and one is near you
                    setScared();
                }
            }
            else{
                //There are no infected in the level
                resetScared();
                Roam();

            }
            if(scared){
                GetClosestInfected();
                float dist = Vector3.Distance(this.transform.position, Min.transform.position);
                Vector3 dirToThreat = this.transform.position - Min.transform.position;
                dirToThreat.Normalize();
                Vector3 newPos = (transform.position + dirToThreat);
                spreadAngle = Quaternion.AngleAxis(Random.Range(-runningAngle, runningAngle), new Vector3(0, 1, 0));
                newVector = spreadAngle * dirToThreat;
                Quaternion toRotation = Quaternion.LookRotation(dirToThreat, Vector3.up);
                this.transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, (turnRate) * Time.deltaTime);
                raycastCount = raycastCount + Time.deltaTime;
                if(dist < criticalDist){
                    //infected VERY CLOSE 
                    Debug.Log("CRITICAL");
                    agent.ResetPath();
                    agent.SetDestination(newPos);
                }
                else if(raycastCount > raycastCap){
                    Debug.Log("finding path away from enemy");
                    raycastCap = Random.Range(raycastCapLower, raycastCapUpper);
                    if(Physics.Raycast(this.transform.position, newVector, out hit, mask)){
                        raycast = hit.point - this.transform.position;
                        magnitude = raycast.magnitude;
                        raycast.Normalize();
                        raycast = raycast * Random.Range(raycastBuffer, magnitude - raycastBuffer);
                        Debug.DrawRay(this.transform.position, raycast, Color.red, 1f);
                        agent.ResetPath();
                        //agent.SetDestination(raycast);
                        NavMesh.CalculatePath(transform.position, raycast, NavMesh.AllAreas, path);
                        Debug.Log(path.status);
                        //agent.SetDestination(path.status);
                        //agent.SetPath(path);
                    }
                    raycastCount = 0;

                }
                   
            }
        }
        else{
            //you are infected!
            list.updateInfectedList(this.gameObject);
            list.removeFromUninfected(this.gameObject);
            select.Select(1);
            GetClosestUninfected();
            if(Min != null){
                //there are uninfected in the level, finding path to closest one
                agent.ResetPath();
                NavMesh.CalculatePath(this.transform.position, Min.transform.position, NavMesh.AllAreas, path);
                agent.SetPath(path);
                agent.speed = ZrunSpeed;
                chasing = true;
            }
            else{
                //there are no uninfected left in the level
                Roam();
                chasing = false;
            }
            //movement cooldown after having just infected someone
            if(gate){
                agent.speed = ZslowSpeed;
                chasing = false;
            }

        }
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
        //else if (list.infected.Count <= 0){
        //    Min = home;
        //}
    }
    void Roam(){
        if(infected){
            agent.speed = ZdefaultSpeed;
        }
        else if(!infected){
            Debug.Log("Roaming");
            agent.speed = defaultSpeed;
        }
        if(counter2 < Random.Range(roamTimerLow, roamTimerUp)){
            counter2 += Time.deltaTime;
        }
        else{
            meshy = RandomNavmeshLocation(400f);
            counter2 = 0;
        }
        agent.ResetPath();
        NavMesh.CalculatePath(this.transform.position, meshy, NavMesh.AllAreas, path);
        agent.SetPath(path);
    }
    public void setScared(){
        agent.speed = runSpeed;
        scared = true;
    }
    public void resetScared(){
        agent.speed = defaultSpeed;
        scared = false;
    }
    public void Infect(Collider other)
    {
        list.removeFromUninfected(other.gameObject);
        other.gameObject.GetComponent<NPCMove>().infected = true;
        flipGate();
        list.updateInfectedList(other.gameObject);
    }

    void GetClosestUninfected(){
        Min = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        if(list.uninfected.Count > 0 ){
            foreach (GameObject g in list.uninfected){
                float dist = Vector3.Distance(g.gameObject.transform.position, currentPos);
                if (dist < minDist){
                    Min = g;
                    minDist = dist;
                }
            }
        }
        //else if (list.uninfected.Count <= 0){
        //    Min = home;
        //}

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



    void flipGate(){
        gate = true;
        Invoke("resetGate", slowCoolDown);
    }
    void resetGate(){
        gate = false;
    }
}
