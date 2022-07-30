using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    jerryAnimScript anim;
    [SerializeField]
    [Tooltip("Speeds of default roaming, boosted scared speed, and post attack cooldown speed")]
    public float runSpeed, defaultSpeed, slowSpeed;
    [SerializeField]
    [Tooltip("Speeds of Zombie roaming, boosted scared speed, and post attack cooldown speed")]
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
    void Start()
    {
        //finds the game object holding the list script
        foreach(GameObject g in GameObject.FindObjectsOfType<GameObject>()){
            if(g.GetComponent<uninfectedList>()!=null){
                uninfectedList = g;
            }
        }
        //plugging referenences
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
            GetClosestInfected();
            // if min is null, there are not any infected so not scared by default
            if(Min != null){
                float dist = Vector3.Distance(this.transform.position, Min.transform.position);
                if(dist < fearRadius){
                    //There are infected in the level, and one is near you
                    setScared();
                    agent.speed = runSpeed;
                    Vector3 dirToThreat = this.transform.position - Min.transform.position;
                    dirToThreat.Normalize();
                    Vector3 newPos = (transform.position + dirToThreat);
                    NavMesh.CalculatePath(this.transform.position, newPos, NavMesh.AllAreas, path);
                    agent.SetPath(path);
                    Quaternion toRotation = Quaternion.LookRotation(dirToThreat, Vector3.up);
                    this.transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, (turnRate) * Time.deltaTime);
                    updateCount = updateCount + Time.deltaTime;
                    if(updateCount > updateCap && dist > fearRadius){
                        resetScared();
                        updateCount = 0;
                    }
                }
                else{
                    //there are infected in the level, but none are near you
                    Roam();
                }
            }
            else{
                //There are no infected in the level
                Roam();

            }
        }
        else{
            list.updateInfectedList(this.gameObject);
            list.removeFromUninfected(this.gameObject);
            //you are infected!
            select.Select(1);
            GetClosestUninfected();
            if(Min != null){
                //there are uninfected in the level, finding path to closest one
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
        else{
            resetScared();
            agent.speed = defaultSpeed;
        }
        if(counter2 < Random.Range(roamTimerLow, roamTimerUp)){
            counter2 += Time.deltaTime;
        }
        else{
            meshy = RandomNavmeshLocation(400f);
            counter2 = 0;
        }
        NavMesh.CalculatePath(this.transform.position, meshy, NavMesh.AllAreas, path);
        agent.SetPath(path);
    }
    public void setScared(){
        scared = true;
    }
    public void resetScared(){
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
