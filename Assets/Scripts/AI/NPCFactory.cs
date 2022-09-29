using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCFactory : MonoBehaviour
{
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
    }


    void DespawnNPC(){

    }
    void SpawnNPC(GameObject g){
        if(disabledNPCList.Contains(g)){
            //Debug.Log("Spawning existing NPC "+ g.name + "  on random spawnpoint");
            int rand = Random.Range(0, NPCSpawners.Count - 1);
            Vector3 randPos = GetRandomPoint(NPCSpawners[rand]);
            AssignRandomNpcTraits(g);
            g.transform.position = randPos;
            g.SetActive(true);
            disabledNPCList.Remove(g);
            activeNPCList.Add(g);
        }
        else{
            int rand = Random.Range(0, NPCSpawners.Count - 1);
            //Debug.Log(NPCSpawners[rand].name + " being spawned on", NPCSpawners[rand]);
            Vector3 randPos = GetRandomPoint(NPCSpawners[rand]);
            //Debug.Log(randPos + " is the position within " + NPCSpawners[rand].name, NPCSpawners[rand]);
            //Debug.DrawLine(this.transform.position, randPos, Color.red, 5f);
            GameObject g2 = Instantiate(g, randPos, Quaternion.identity);
            AssignRandomNpcTraits(g2);
            activeNPCList.Add(g);
        }
    }
    Vector3 GetValidSpawnPoint(){
        Vector3 randPos = RandomNavmeshLocation(Random.Range(10f, 500f));
        NavMesh.CalculatePath(homeBeacon.position, randPos, NavMesh.AllAreas, path);
        if(path.status == NavMeshPathStatus.PathComplete){
            return randPos;
        }
        else{
            return GetValidSpawnPoint();
        }
    }
    void SpawnRandomLocationNpc(GameObject g){
        if(disabledNPCList.Contains(g)){
            //Debug.Log("Spawning existing NPC " + g.name + " on random point on navmesh");
            Vector3 randPos = GetValidSpawnPoint();
            //Debug.Log(randPos);
            AssignRandomNpcTraits(g);
            g.SetActive(true);
            disabledNPCList.Remove(g);
            activeNPCList.Add(g);
        }
        else{
            //Debug.Log("Spawning new NPC "+ g.name + " on a random point on the navmesh");
            Vector3 randPos = GetValidSpawnPoint();
            //Debug.Log(randPos);
            GameObject g2 = Instantiate(g, randPos, Quaternion.identity);
            AssignRandomNpcTraits(g2);
            activeNPCList.Add(g);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        NPCStartingAmount = Random.Range(NPCStartingAmountUpper, NPCStartingAmountLower);
        for (int i = 0; i < NPCStartingAmount; i++)
        {
            SpawnRandomLocationNpc(NPCList[Random.Range(0, NPCList.Count)]);
        }
    }

    public Vector3 GetRandomPoint(Transform center){
        Vector3 finalPos;
        Vector3 randomDirection = GetRandomDirectionVector3();
        RaycastHit hit;
        if(Physics.Raycast(center.position, randomDirection, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Collide)){
            //Debug.Log("using random spot");
            //Debug.DrawLine(center.position, hit.point, Color.green, 5f);
            Vector3 difference = hit.point - center.position;
            Vector3 newDifference = difference * Random.Range(0f, 1f);
            finalPos = center.position + newDifference;
            return finalPos;
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

    public Vector3 GetRandomPointInBounds(Bounds bounds, GameObject g) {
        float minX = bounds.size.x * -0.5f;
        float minY = bounds.size.y * -0.5f;
        float minZ = bounds.size.z * -0.5f;

        return (Vector3)g.gameObject.transform.TransformPoint(
            new Vector3(Random.Range (minX, -minX),
                Random.Range (minY, -minY),
                Random.Range (minZ, -minZ))
        );
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
