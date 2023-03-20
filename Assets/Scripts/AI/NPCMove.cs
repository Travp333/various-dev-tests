using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPCMove : MonoBehaviour
{
    public TMP_Text debugText;
    public float destThreshold = 5f;
    bool destGate;
    public float lifespan;
    public bool isLeaving = false;
    Vector3 dest;
    [SerializeField]
    GameObject carrot;
    [SerializeField]
    LayerMask mask;
    [HideInInspector]
    public float runSpeed, defaultSpeed, slowSpeed;

    [SerializeField]
    [Tooltip("Speeds of default roaming, boosted scared speed, and a currently unused slow speed(injured?)")]
    public float runSpeedUpper, runSpeedLower, defaultSpeedUpper, defaultSpeedLower, slowSpeedUpper, slowSpeedLower;
    float updateCount = 0;
    [SerializeField]
    [Tooltip("How long an NPC stays scared")]
    public float updateCap = 60;
    [SerializeField]
    [Tooltip("How often this agent searches for other agents in level")]
    float searchCap = 1f;
    float scarySearchCount, AttractiveSearchCount, scaredSearchCount, NPCSearchCount, destSearchCount;
    [HideInInspector]
    public NavMeshAgent agent;
    [SerializeField]
    [Tooltip("the rate at which the npc turns")]
    private float turnRate;
    GameObject Min;
    [SerializeField]
    [Tooltip("Radius where npcs get scared")]
    private float fearRadius;
    [SerializeField]
    [Tooltip("Radius where chaser npcs will find targets")]
    private float detectRadius = 60f;

    [SerializeField]
    [Tooltip("distance at which the npc will use panicked movement, should be less than fearRadius")]
    float criticalDist;
    NavMeshPath path;
    float counter2 = 0f;
    Vector3 meshy;
    [SerializeField]
    [Tooltip("Upper cap of how long an npc waits until roaming to new position")]
    float roamTimerUp = 15;
    [SerializeField]
    [Tooltip("Lower cap of how long an npc waits until roaming to new position")]
    float roamTimerLow = 5;

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
    public bool moveBlocked;
    Vector3 reflect, reflect2;
    [SerializeField]
    float reflectRange = 5f;
    bool raycastBlock;
    float roamTimer;
    [SerializeField]
    FaceTexController tex;
    NPCBehaviorChangersList list;
    [SerializeField]
    public bool chaser, runner, scary, attractive, chasing, brave, scared, gate;

    bool lifespanGate;
    NPCFactory fact;
    float tempSpeed;

    void Start()
    {
        debugText.text = "";
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("EmptyScriptHolders") ){
            if(g.GetComponent<NPCFactory>()!=null){
                fact = g.GetComponent<NPCFactory>();
            }
        }
        runSpeed = Random.Range(runSpeedLower, runSpeedUpper);
        defaultSpeed = Random.Range(defaultSpeedLower, defaultSpeedUpper);
        slowSpeed = Random.Range(slowSpeedLower, slowSpeedUpper);
        
        foreach(GameObject g in GameObject.FindObjectsOfType<GameObject>()){
            if(g.GetComponent<NPCBehaviorChangersList>()!=null){
                list = g.GetComponent<NPCBehaviorChangersList>();
            }
        }
        AttractiveSearchCount = searchCap;
        NPCSearchCount = searchCap;
        scaredSearchCount = searchCap;
        scarySearchCount = searchCap;
        counter2 = roamTimer;
        raycastCap = Random.Range(raycastCapLower, raycastCapUpper);
        raycastCount = raycastCap;
        //plugging references
        //meshy = RandomNavmeshLocation(Random.Range(50f, 300f));
        path = new NavMeshPath(); 
        agent = GetComponent<NavMeshAgent>();
        agent.speed = defaultSpeed;
        Roam();
        list.updateNPCList(this.gameObject);
        if(scary){
            list.updateScaryList(this.gameObject);
        }
        if(attractive){
            list.updateAttractiveList(this.gameObject);
        }

    }

    void GetClosestScary(){
        //Debug.Log(this.gameObject.name + "is finding the nearest scary NPC");
        Min = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        if(list.scary.Count > 0){
            foreach (GameObject g in list.scary){
                float dist = Vector3.Distance(g.gameObject.transform.position, currentPos);
                if (dist < minDist){
                    Min = g;
                    minDist = dist;
                }
            }
        }
    }
    void GetClosestNPC(){
        //Debug.Log(this.gameObject.name + "is finding the nearest npc");
        Min = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        if(list.npcs.Count > 0){
            foreach (GameObject g in list.npcs){
                float dist = Vector3.Distance(g.gameObject.transform.position, currentPos);
                if (dist < minDist && g != this.gameObject){
                    Min = g;
                    minDist = dist;
                }
            }
        }
    }
    void GetClosestNONSCARYNPC(){
        //Debug.Log(this.gameObject.name + "is finding the nearest non scary npc");
        Min = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        if(list.nonScaryNPCs.Count > 0){
            foreach (GameObject g in list.nonScaryNPCs){
                float dist = Vector3.Distance(g.gameObject.transform.position, currentPos);
                if (dist < minDist && g != this.gameObject){
                    Min = g;
                    minDist = dist;
                }
            }
        }
    }
    void GetClosestAttractive(){
        //Debug.Log(this.gameObject.name + "is finding the nearest attractive npc");
        Min = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        if(list.attractive.Count > 0){
            foreach (GameObject g in list.attractive){
                float dist = Vector3.Distance(g.gameObject.transform.position, currentPos);
                if (dist < minDist){
                    Min = g;
                    minDist = dist;
                }
            }
        }
    }

    public void setIsLeaving(Vector3 dest2){
        
        isLeaving = true;
        dest = dest2;
    }

    float debugCount = 0f;
    float debugCap = 0.5f;
    void Update()
    {
        debugCount += Time.deltaTime;
        if(debugCount >= debugCap){
            debugCount = 0.5f;
            changeDebugText();
        }
        if(lifespan > 0f){  
            lifespan -= Time.deltaTime;
            isLeaving = false;
            lifespanGate = false;
        }
        else if (lifespan <= 0f){
            lifespan = 0f;
            if(!lifespanGate){
                resetScared();
                setIsLeaving(fact.GetRandomPoint());
                lifespanGate = true;

            }
            
        }
        //if(Min != null){
        //    Debug.Log(this.gameObject.name + " " + Min.gameObject.name);
        //}
        if(isLeaving){
            //Debug.Log(this.gameObject.name + " is leaving");
            if(!destGate){
                NavMesh.CalculatePath(this.transform.position, dest, NavMesh.AllAreas, path);
                agent.ResetPath();
                agent.SetPath(path);
                destGate = true;
                changeAgentSpeedToGiven(defaultSpeed);
                //tempSpeed = agent.speed;
                //StopAllCoroutines();
                //StartCoroutine(Fade(defaultSpeed));
                //agent.speed = defaultSpeed;
            }
            if(destGate){
                destSearchCount += Time.deltaTime;
                if(destSearchCount >= searchCap){
                    //Debug.Log(this.gameObject.name + " is checking distance" + Vector3.Distance(this.gameObject.transform.position, dest));
                    if(Vector3.Distance(this.gameObject.transform.position, dest) < destThreshold){
                        //Debug.Log(this.gameObject.name + " is leaving");
                        fact.DespawnNPC(this.gameObject);
                        
                    }
                    destSearchCount = destSearchCount - searchCap;
                }
            }

        }
        else if(!chaser && !scary){
            //Debug.Log(this.gameObject.name + "is not a chaser and is not scary");
            if(list.scary.Count > 0 && !runner){
                //Debug.Log(this.gameObject.name + "knows there is a scary agent somewhere");
                //there are scary npcs on the level somewhere, find the closest one
                scarySearchCount += Time.deltaTime;
                if(scarySearchCount >= searchCap){
                    //find the closest one, once per second
                    GetClosestScary();
                    //Debug.Log(this.gameObject.name + "is calculating a path away from " + Min.gameObject.name);
                    scarySearchCount = scarySearchCount - searchCap;
                    //calculate a path away from that scary NPC
                }
                // if you are a runner, just find the nearest NPC and run away from them
                
                if(Min != null){
                    
                    NavMesh.CalculatePath(Min.transform.position, this.transform.position , NavMesh.AllAreas, path);
                    float dist = Vector3.Distance(this.transform.position, Min.transform.position);

                    if(dist > fearRadius && scared){
                        updateCount = updateCount + Time.deltaTime;
                        if(updateCount >= updateCap){
                            //Debug.Log("Reset Scared!");
                            resetScared();
                            updateCount = updateCount - updateCap;
                            Roam();
                        }
                    }
                    else if(dist < fearRadius && path.status == NavMeshPathStatus.PathComplete){ 
                        //Debug.Log("VALID ROUTE");
                        //There are Scary NPCS in the level, and one is near you
                        //Debug.Log(this.gameObject.name + "is near a scary NPC");
                        setScared();
                    }
                }
            }

            if(runner){
                //Debug.Log("I AM RUNNER", this.gameObject);
                NPCSearchCount += Time.deltaTime;
                if(NPCSearchCount >= searchCap){
                    //find the closest one, once per second
                    GetClosestNPC();
                    //Debug.Log(this.gameObject.name + "is calculating a path away from " + Min.gameObject.name);
                    NPCSearchCount = NPCSearchCount - searchCap;
                    //calculate a path away from that scary NPC
                }
                if(Min != null){
                    NavMesh.CalculatePath(Min.transform.position, this.transform.position , NavMesh.AllAreas, path);
                    float dist = Vector3.Distance(this.transform.position, Min.transform.position);
                    if(dist > fearRadius && scared){
                        updateCount = updateCount + Time.deltaTime;
                        if(updateCount >= updateCap){
                            //Debug.Log("Reset Scared!");
                            resetScared();
                            updateCount = updateCount - updateCap;
                            Roam();
                        }
                    }
                    else if(dist < fearRadius && path.status == NavMeshPathStatus.PathComplete){ 
                        //Debug.Log("VALID ROUTE");
                        //There are Scary NPCS in the level, and one is near you
                        //Debug.Log(this.gameObject.name + "is near a scary NPC");
                        setScared();
                    }
                }

            }
            if(scared){
                findEscape();
            }
            else if(!scared && !chaser && !attractive){
                //Debug.Log(this.gameObject.name + "is not scared or a chaser");
                //check for attractive NPCS
                AttractiveSearchCount += Time.deltaTime;
                if(AttractiveSearchCount >= searchCap){
                    GetClosestAttractive();
                    AttractiveSearchCount = AttractiveSearchCount - searchCap;
                }
                if(Min != null){
                    //Debug.Log(this.gameObject.name + "found a nearby attractive npc, " + Min.gameObject.name);
                    NavMesh.CalculatePath(this.transform.position, Min.transform.position, NavMesh.AllAreas, path);
                }
                if((Min != null && path.status == NavMeshPathStatus.PathComplete && Vector3.Distance(this.transform.position, Min.transform.position) <= detectRadius && !runner) && list.attractive.Count > 0){
                    //Debug.Log("there are attractive NPCS in the level, finding path to closest one");
                    //there are attractive NPCS in the level, finding path to closest one
                    agent.ResetPath();
                    agent.SetPath(path);
                    changeAgentSpeedToGiven(runSpeed);
                    //tempSpeed = agent.speed;
                    //StopAllCoroutines();
                    //StartCoroutine(Fade(runSpeed));
                    //agent.speed = runSpeed;
                    chasing = true;
                }
                else{
                    //there are no attractive NPC's near you
                    //Debug.Log(this.gameObject.name + "is not scared or a chaser or near any attractive npc's, roaming");
                    Roam();
                    chasing = false;
                }
            }
        }
        else if(chaser){
            //Debug.Log(this.gameObject.name + "is a chaser");
            //need to create logic for when a chaser reaches its target
            NPCSearchCount += Time.deltaTime;
            if(NPCSearchCount >= searchCap){
                GetClosestNONSCARYNPC();
                NPCSearchCount = NPCSearchCount - searchCap;
            }
            if(Min != null){
                //Debug.Log(this.gameObject.name + "is calculating a path to the nearest npc");
                NavMesh.CalculatePath(this.transform.position, Min.transform.position, NavMesh.AllAreas, path);
            }

            if(Min != null && path.status == NavMeshPathStatus.PathComplete && Vector3.Distance(this.transform.position, Min.transform.position) <= detectRadius){
                //there are NPCS in the level, finding path to closest one
                //Debug.Log(this.gameObject.name + "is now headed toward nearest npc, " + Min.transform.gameObject.name);
                agent.ResetPath();
                agent.SetPath(path);
                //agent.speed = (Min.gameObject.GetComponent<NPCMove>().agent.speed *.9f);
                changeAgentSpeedToGiven(runSpeed);
                //tempSpeed = agent.speed;
                //StopAllCoroutines();
                //StartCoroutine(Fade(runSpeed));
                //agent.speed = runSpeed;
                chasing = true;
            }
            else{
                //Debug.Log(this.gameObject.name + "is a chaser and cant find any npc's");
                //there are no NPC's near you
                Roam();
                chasing = false;
            }
            if(Min != null){
                if(Vector3.Distance(this.transform.position, Min.transform.position) < criticalDist){
                    //Debug.Log("Caught up to chasee", this.gameObject);
                    //Debug.Log("Caught up to chasee", Min.gameObject);
                    changeAgentSpeedToGiven(Min.gameObject.GetComponent<NPCMove>().agent.speed * .5f);
                    //tempSpeed = agent.speed;
                    //StopAllCoroutines();
                    //StartCoroutine(Fade((Min.gameObject.GetComponent<NPCMove>().agent.speed * .5f)));
                    //agent.speed = (Min.gameObject.GetComponent<NPCMove>().agent.speed * .5f);
                }
            }
        }

    }
    void resetRaycastBlock(){
        raycastBlock = false;
        
    }
    void findEscape(){
        //Debug.Log(this.gameObject.name + "is finding escape");
        if(!runner){
            GetClosestScary();
        }
        else if (runner){
            GetClosestNPC();
        }
        if(Min == null){
            PanicRoam();
        }
        else{
            float dist = Vector3.Distance(this.transform.position, Min.transform.position);
            Vector3 dirToThreat = this.transform.position - Min.transform.position;

            dirToThreat.Normalize();
            Vector3 newPos = (transform.position + dirToThreat);
            raycastCount = raycastCount + Time.deltaTime;
            if(dist < criticalDist){
                //Debug.Log(this.gameObject.name + " is in Critical Distance");
                if(NavMesh.CalculatePath(transform.position, carrot.transform.position, NavMesh.AllAreas, path)){
                    agent.ResetPath();
                    agent.SetPath(path);
                    Quaternion toRotation = Quaternion.LookRotation(dirToThreat, Vector3.up);
                    this.transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, (turnRate) * Time.deltaTime);
                }
            }
            //check if a wall is in front of you
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
                reflect = ProjectDirectionOnPlane(reflect, this.transform.up);
                //check if wall is in front of reflected ray
                if(Physics.Raycast(hit.point, reflect, out hit2, reflectRange, mask)){
                    //yes something is in the way of the reflected ray, reflect again!
                    reflect2 = (Vector3.Reflect(((hit2.point - hit.point).normalized * (hit.point - transform.position).magnitude), hit2.normal));
                    spreadAngle = Quaternion.AngleAxis(Random.Range(-runningAngle, runningAngle), new Vector3(0, 1, 0));
                    reflect2 = spreadAngle * reflect2;
                    reflect2 = ProjectDirectionOnPlane(reflect2, this.transform.up);
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
                //Debug.Log(this.gameObject.name + "is Running Straight Away");
                agent.ResetPath();
                agent.SetPath(path);
                Quaternion toRotation = Quaternion.LookRotation(dirToThreat, Vector3.up);
                this.transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, (turnRate) * Time.deltaTime);
            }
        }
       
    }
    void Roam(){
        //Debug.Log(this.gameObject.name + "is roaming");
        changeAgentSpeedToGiven(defaultSpeed);
        //tempSpeed = agent.speed;
        //StopAllCoroutines();
        //StartCoroutine(Fade(defaultSpeed));
        //agent.speed = defaultSpeed;
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
        changeAgentSpeedToGiven(runSpeed);
        //Debug.Log(this.gameObject.name + "is panic roaming");
        //tempSpeed = agent.speed;
        //StopAllCoroutines();
        //StartCoroutine(Fade(runSpeed));
        //agent.speed = runSpeed;
        if(counter2 < Random.Range(roamTimerLow, roamTimerUp)){
            counter2 += Time.deltaTime;
        }
        else{
            meshy = RandomNavmeshLocation(Random.Range(50f, 300f));
            counter2 = 0;
        }
        //agent.ResetPath();
        NavMesh.CalculatePath(this.transform.position, meshy, NavMesh.AllAreas, path);
        agent.ResetPath();
        agent.SetPath(path);
    }
    public void setScared(){
        if(!brave){
            changeAgentSpeedToGiven(runSpeed);
            //tempSpeed = agent.speed;
            //StopAllCoroutines();
            //StartCoroutine(Fade(runSpeed));
            //agent.speed = runSpeed;
            scared = true;
            tex.setScared();
        }
    }
    public void resetScared(){
        changeAgentSpeedToGiven(defaultSpeed);
        //tempSpeed = agent.speed;
        //StopAllCoroutines();
        //StartCoroutine(Fade(defaultSpeed));
        //agent.speed = defaultSpeed;
        scared = false;
        tex.setBase();
    }

    void changeAgentSpeedToGiven(float speed){
        tempSpeed = agent.speed;
        StopCoroutine("Fade");
        StartCoroutine(Fade(speed));
    }
    IEnumerator Fade(float newSpeed)
    {
        float elapsedTime = 0f;
        float startSpeed = agent.speed;
        float speedDifference = newSpeed - startSpeed;

        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            agent.speed = startSpeed + (speedDifference * (elapsedTime / 2f));
            yield return null;
        }
        
    }
    void changeDebugText(){

        if( lifespan <= 0){
            debugText.text = "Despawning";
        }
        else if(chaser){
            if(scary){
                debugText.text = "Scary Chaser";
            }
            else if(chasing){
                debugText.text = "Chasing Chaser";
            }
            else if(brave){
                debugText.text = "Brave Chaser";
            }
            else if(scared){
                debugText.text = "Scared Chaser";
            }
            else{
                debugText.text = "Chaser";
            }
        }
        else if(runner){
            if(scary){
                debugText.text = "Scary Runner";
            }
            else if(attractive){
                debugText.text = "Attractive Runner";
            }
            else if (scared){
                debugText.text = "Scared Runner";
            }
            else{
                debugText.text = "Runner";
            }
        }
        else if(scary){
            debugText.text = "Scary";
        }
        else if (attractive){
            debugText.text = "Attractive";
        }
        else if (scared){
            debugText.text = "Scared";
        }
        else if (brave){
            debugText.text = "Brave";
        }
        else{
            debugText.text = "";
        }
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
    Vector3 ProjectDirectionOnPlane (Vector3 direction, Vector3 normal) {
		return (direction - normal * Vector3.Dot(direction, normal)).normalized;
	}


}
