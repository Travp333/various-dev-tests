using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCFactory : MonoBehaviour
{
    [SerializeField]
    int LifespanUpper, LifespanLower;
    NavMeshPath path;
    [SerializeField]
    Transform homeBeacon;
    [SerializeField]
    public List<Transform> NPCSpawners = new List<Transform>();
    [SerializeField]
    public List<GameObject> NPCList = new List<GameObject>();
    public List<GameObject> activeNPCList = new List<GameObject>();
    public List<GameObject> disabledNPCList = new List<GameObject>();
    [SerializeField]
    int maxNPC;
    [SerializeField]
    LayerMask mask;
    [SerializeField]
    int NPCStartingAmountUpper, NPCStartingAmountLower;
    int NPCStartingAmount;
    float spawnTimerCount, searchCap = 1;

    void AssignRandomNpcTraits(GameObject g2){
        int rand2 = Random.Range(0,150);
        if(rand2 == 5){
            g2.GetComponent<NPCMove>().runner = true;
        }
        else if (rand2 == 6){
            g2.GetComponent<NPCMove>().attractive = true;
        }
        else if (rand2 == 34){
            g2.GetComponent<NPCMove>().runner = true;
            g2.GetComponent<NPCMove>().attractive = true;
        }
        else if (rand2 == 12){
            g2.GetComponent<NPCMove>().scary = true;
        }
        else if (rand2 == 22){
            g2.GetComponent<NPCMove>().scary = true;
            g2.GetComponent<NPCMove>().chaser = true;
        }
        else if (rand2 == 41){
            g2.GetComponent<NPCMove>().chaser = true;
        }
        else if (rand2 == 22){
            g2.GetComponent<NPCMove>().brave = true;
        }
        else if (rand2 == 39){
            g2.GetComponent<NPCMove>().brave = true;
            g2.GetComponent<NPCMove>().chaser = true;
        }
        else if (rand2 == 69){
            g2.GetComponent<NPCMove>().brave = true;
            g2.GetComponent<NPCMove>().attractive = true;
        }
        g2.GetComponent<NPCMove>().lifespan = Random.Range(LifespanLower, LifespanUpper);
    }

    public void DespawnNPC(GameObject g){
        
        if(activeNPCList.Contains(g)){
            Debug.Log(g.gameObject.name + " is in the list");
            int lastIndex = activeNPCList.Count - 1;
            activeNPCList[activeNPCList.IndexOf(g)] = activeNPCList[lastIndex];  
            activeNPCList.RemoveAt(lastIndex);
            //activeNPCList.Remove(g);
            disabledNPCList.Add(g);
            g.SetActive(false);
        }
    }
    void SpawnNPC(GameObject g){
        if(disabledNPCList.Contains(g)){
            AssignRandomNpcTraits(g);
            Vector3 randPos = GetRandomPoint();
            g.transform.position = randPos;
            g.SetActive(true);
            //fancy list removal from Catlike, may be implemented wrong
            int lastIndex = disabledNPCList.Count - 1;
            disabledNPCList[disabledNPCList.IndexOf(g)] = disabledNPCList[lastIndex];  
            disabledNPCList.RemoveAt(lastIndex);
            activeNPCList.Add(g);
        }
        else{
            AssignRandomNpcTraits(g);
            activeNPCList.Add(g);
        }
    }
    void SpawnRandomLocationNpc(GameObject g){
        if(disabledNPCList.Contains(g)){
            Vector3 randPos = GetValidSpawnPoint();
            g.transform.position = randPos;
            AssignRandomNpcTraits(g);
            g.SetActive(true);
            //fancy list removal from Catlike, may be implemented wrong
            int lastIndex = disabledNPCList.Count - 1;
            disabledNPCList[disabledNPCList.IndexOf(g)] = disabledNPCList[lastIndex];  
            disabledNPCList.RemoveAt(lastIndex);
            activeNPCList.Add(g);
        }
        else{
            Debug.Log("FUCKLER");
            AssignRandomNpcTraits(g);
            activeNPCList.Add(g);
        }
    }

    public Vector3 GetValidSpawnPoint(){
        Vector3 randPos = RandomNavmeshLocation(Random.Range(10f, 500f));
        NavMesh.CalculatePath(homeBeacon.position, randPos, NavMesh.AllAreas, path);
        if(path.status == NavMeshPathStatus.PathComplete){
            return randPos;
        }
        else{
            return GetValidSpawnPoint();
        }
    }

    void Update()
    {
        if(activeNPCList.Count < maxNPC){
            spawnTimerCount += Time.deltaTime;
            if(spawnTimerCount >= searchCap){
                Vector3 randPos = GetRandomPoint();
                GameObject g2 = Instantiate(NPCList[Random.Range(0, NPCList.Count)], randPos, Quaternion.identity);
                SpawnNPC(g2);
                spawnTimerCount = spawnTimerCount - searchCap;
                searchCap = Random.Range(5f, 10f);
            }
        }
    }
    void Start()
    {
        searchCap = Random.Range(5f, 10f);
        path = new NavMeshPath();
        NPCStartingAmount = Random.Range(NPCStartingAmountUpper, NPCStartingAmountLower);
        for (int i = 0; i < NPCStartingAmount; i++)
        {
            Vector3 randPos = GetValidSpawnPoint();
            GameObject g2 = Instantiate(NPCList[Random.Range(0, NPCList.Count)], randPos, Quaternion.identity);
            SpawnRandomLocationNpc(g2);
        }
    }

    public Vector3 GetRandomPoint(){
        int rand = Random.Range(0, NPCSpawners.Count - 1);
        Transform center = NPCSpawners[rand].transform;

        Vector3 finalPos;
        Vector3 randomDirection = GetRandomDirectionVector3();
        RaycastHit hit;
        if(Physics.Raycast(center.position, randomDirection, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Collide)){
            //Debug.Log("using random spot");
            //Debug.DrawLine(center.position, hit.point, Color.green, 5f);
            Vector3 difference = hit.point - center.position;
            Vector3 newDifference = difference * Random.Range(0f, 1f);
            finalPos = center.position + newDifference;
            NavMesh.CalculatePath(homeBeacon.position, finalPos, NavMesh.AllAreas, path);
            if(path.status == NavMeshPathStatus.PathComplete){
                return finalPos;
            }
            else{
                return GetRandomPoint();
            }

        }
        else{
            //Debug.Log("using center");
            return center.position;
        }

    }

    private Vector3 GetRandomDirectionVector3() {
        return ProjectDirectionOnPlane(Random.insideUnitSphere.normalized, this.transform.up);
    }
    Vector3 ProjectDirectionOnPlane (Vector3 direction, Vector3 normal) {
		return (direction - normal * Vector3.Dot(direction, normal)).normalized;
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
}
