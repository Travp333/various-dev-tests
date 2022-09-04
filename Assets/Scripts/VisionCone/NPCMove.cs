using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    [SerializeField]
    GameObject carrot;
    [SerializeField]
    LayerMask mask;
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
    GameObject Min;
    [SerializeField]
    [Tooltip("Radius where npcs get scared by infected")]
    private float fearRadius;
    [SerializeField]
    [Tooltip("Radius where npcs infected npcs will find uninfected targets")]
    private float detectRadius = 60f;

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
    RaycastHit hit, hit2, hit3;
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
    bool moveBlocked;
    Vector3 reflect, reflect2;
    [SerializeField]
    float reflectRange = 5f;
    bool raycastBlock;
    bool uninfectedBlock = false;
    bool infectedBlock = false;
    [SerializeField]
    [Tooltip("How often this agent searches for uninfected agents in level")]
    float uninfectedSearchCap = 1f;
    float uninfectedSearchCount;
    [SerializeField]
    [Tooltip("How often this agent searches for infected agents in level")]
    float infectedSearchCap = 1f;
    float infectedSearchCount;
    float roamTimer;
    [SerializeField]
    FaceTexController tex;


    void Start()
    {
        counter2 = roamTimer;
        infectedSearchCount = infectedSearchCap;
        uninfectedSearchCount = uninfectedSearchCap;
        raycastCap = Random.Range(raycastCapLower, raycastCapUpper);
        raycastCount = raycastCap;
        //finds the game object holding the list script
        foreach(GameObject g in GameObject.FindObjectsOfType<GameObject>()){
            if(g.GetComponent<uninfectedList>()!=null){
                uninfectedList = g;
            }
        }
        //plugging references
        //meshy = RandomNavmeshLocation(Random.Range(50f, 300f));
        path = new NavMeshPath();
        list = uninfectedList.GetComponent<uninfectedList>();
        agent = GetComponent<NavMeshAgent>();
        Roam();

    }

    void uninfectedUpdate(){
        if(!uninfectedBlock){
            tex.setBase();
            list.removeFromInfected(this.gameObject);
            list.updateUninfectedList(this.gameObject);
            uninfectedBlock = true;
            infectedBlock = false;
        }
    }
    void infectedUpdate(){
        if(!infectedBlock){
            tex.setAngry();
            list.updateInfectedList(this.gameObject);
            list.removeFromUninfected(this.gameObject);
            infectedBlock = true;
            uninfectedBlock = false;
        }

    }


    void Update()
    {
        if(!infected){
            //You are uninfected
            uninfectedUpdate();
            if(list.infected.Count > 0){
                infectedSearchCount += Time.deltaTime;
                if(infectedSearchCount > infectedSearchCap){
                    GetClosestInfected();
                    infectedSearchCount = 0;
                }
                NavMesh.CalculatePath(Min.transform.position, this.transform.position , NavMesh.AllAreas, path);
                float dist = Vector3.Distance(this.transform.position, Min.transform.position);
                if(dist > fearRadius && scared){
                    updateCount = updateCount + Time.deltaTime;
                    if(updateCount > updateCap){
                        //Debug.Log("Reset Scared!");
                        resetScared();
                        updateCount = 0;
                        Roam();
                    }
                }
                else if(dist < fearRadius && path.status == NavMeshPathStatus.PathComplete){ 
                    //Debug.Log("VALID ROUTE");
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
                findEscape();
            }
            else{
                Roam();
            }
        }
        else{
            //you are infected!
            infectedUpdate();
            uninfectedSearchCount += Time.deltaTime;
            if(uninfectedSearchCount > uninfectedSearchCap){
                GetClosestUninfected();
                uninfectedSearchCount = 0;
            }
            
            if(Min != null){
                NavMesh.CalculatePath(this.transform.position, Min.transform.position, NavMesh.AllAreas, path);
            }
            

            if(Min != null && path.status == NavMeshPathStatus.PathComplete && Vector3.Distance(this.transform.position, Min.transform.position) <= detectRadius){
                //there are uninfected in the level, finding path to closest one
                //NavMesh.CalculatePath(this.transform.position, Min.transform.position, NavMesh.AllAreas, path);
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
    void resetRaycastBlock(){
        raycastBlock = false;
        
    }
    void findEscape(){
        GetClosestInfected();
        float dist = Vector3.Distance(this.transform.position, Min.transform.position);
        Vector3 dirToThreat = this.transform.position - Min.transform.position;
        dirToThreat.Normalize();
        Vector3 newPos = (transform.position + dirToThreat);
        raycastCount = raycastCount + Time.deltaTime;
        //check if a wall is in front of you
        if(dist < criticalDist){
            //Debug.Log("Critical Distance");
            if(NavMesh.CalculatePath(transform.position, carrot.transform.position, NavMesh.AllAreas, path)){
                agent.ResetPath();
                agent.SetPath(path);
                Quaternion toRotation = Quaternion.LookRotation(dirToThreat, Vector3.up);
                this.transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, (turnRate) * Time.deltaTime);
            }
        }
        else if(Physics.Raycast(transform.position, this.transform.forward, out hit, reflectRange, mask) && ! raycastBlock){
            //There is a wall! clear current path, stop movement
            agent.ResetPath();
            moveBlocked = true; 
            //Debug.Log("Hit wall");
            //Debug.DrawRay(transform.position, hit.point - transform.position, Color.red, 1f);
            //reflect off of that wall to find alternate escape route
            reflect = (Vector3.Reflect(hit.point - transform.position, hit.normal));
            spreadAngle = Quaternion.AngleAxis(Random.Range(-runningAngle, runningAngle), new Vector3(0, 1, 0));
            reflect = spreadAngle * reflect;
            //check if wall is in front of reflected ray
            if(Physics.Raycast(hit.point, reflect, out hit2, reflectRange, mask)){
                //yes something is in the way of the reflected ray, reflect again!
                reflect2 = (Vector3.Reflect(((hit2.point - hit.point).normalized * (hit.point - transform.position).magnitude), hit2.normal));
                spreadAngle = Quaternion.AngleAxis(Random.Range(-runningAngle, runningAngle), new Vector3(0, 1, 0));
                reflect2 = spreadAngle * reflect2;
                //Debug.DrawRay(hit.point, reflect, Color.magenta, 1f);
                //is something in the way of that reflection?
                if(Physics.Raycast(hit2.point, reflect2, out hit3, reflectRange, mask)){
                    //yes, then just run somewhere random??
                    PanicRoam();
                    //Debug.DrawLine(hit2.point, hit3.point, Color.green, 1f);
                }
                else{
                    //nothing in the way, navigate to that point!
                    Ray ble2 = new Ray(hit2.point, reflect2);
                    if(NavMesh.CalculatePath(transform.position, ble2.GetPoint(reflectRange * 5), NavMesh.AllAreas, path)){
                        raycastBlock = true;
                        Invoke("resetRaycastBlock", raycastCap);
                        // yes, navigate there
                        //Debug.DrawRay(hit2.point, reflect2 * 5, Color.green,1f);
                        agent.ResetPath();
                        agent.SetPath(path);
                        
                        //agent.SetDestination(reflect2);
                        //rotate towards that point
                        Quaternion toRotation = Quaternion.LookRotation(reflect2, Vector3.up);
                        this.transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, (turnRate) * Time.deltaTime);
                        //Debug.Log("navigating to second reflected point");
                    }

                }
            }
            // theres nothing in the way of the reflected ray, navigate there!
            else{
                //is the path to the reflected point valid? 
                Ray ble = new Ray(hit.point, reflect);
                if(NavMesh.CalculatePath(transform.position, ble.GetPoint(reflectRange), NavMesh.AllAreas, path)){
                    raycastBlock = true;
                    Invoke("resetRaycastBlock", raycastCap);
                    // yes, navigate there
                    //Debug.DrawRay(hit.point, reflect, Color.magenta,1f);
                    agent.ResetPath();
                    agent.SetPath(path);
                    //agent.SetDestination(reflect);
                    //rotate towards that point
                    Quaternion toRotation = Quaternion.LookRotation(reflect, Vector3.up);
                    this.transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, (turnRate) * Time.deltaTime);
                    //Debug.Log("navigating to reflected point");
                }
            }

        }
        if(moveBlocked){
            if(Vector3.Distance(transform.position, agent.pathEndPosition) < 3){
                moveBlocked = false;
            }
        }

        if(NavMesh.CalculatePath(transform.position, carrot.transform.position, NavMesh.AllAreas, path) && !moveBlocked){
            //Debug.Log("Running Straight Away");
            agent.ResetPath();
            agent.SetPath(path);
            Quaternion toRotation = Quaternion.LookRotation(dirToThreat, Vector3.up);
            this.transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, (turnRate) * Time.deltaTime);
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
            //Debug.Log("Roaming");
            agent.speed = defaultSpeed;
        }
        if(counter2 < roamTimer){
            counter2 += Time.deltaTime;
        }
        else{
            roamTimer = Random.Range(roamTimerLow, roamTimerUp);
            meshy = RandomNavmeshLocation(Random.Range(50f, 300f));
            counter2 = 0;
            agent.ResetPath();
            NavMesh.CalculatePath(this.transform.position, meshy, NavMesh.AllAreas, path);
            agent.SetPath(path);
        }
    }
    void PanicRoam(){
        if(!infected){
            //Debug.Log("Panic Roaming");
            agent.speed = runSpeed;
        }
        if(counter2 < Random.Range(roamTimerLow, roamTimerUp)){
            counter2 += Time.deltaTime;
        }
        else{
            meshy = RandomNavmeshLocation(Random.Range(50f, 300f));
            counter2 = 0;
        }
        //agent.ResetPath();
        NavMesh.CalculatePath(this.transform.position, meshy, NavMesh.AllAreas, path);
        agent.SetPath(path);
    }
    public void setScared(){
        agent.speed = runSpeed;
        scared = true;
        tex.setScared();
    }
    public void resetScared(){
        agent.speed = defaultSpeed;
        scared = false;
        tex.setBase();
    }
    public void Infect(Collider other)
    {
        list.removeFromUninfected(other.gameObject);
        other.gameObject.GetComponent<NPCMove>().infected = true;
        other.gameObject.GetComponent<NPCMove>().tex.setAngry();
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
