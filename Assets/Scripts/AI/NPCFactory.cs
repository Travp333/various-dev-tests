using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFactory : MonoBehaviour
{
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

    void DespawnNPC(){

    }
    void SpawnNPC(){
        foreach(GameObject g in NPCList){
            if(disabledNPCList.Contains(g)){
                g.SetActive(true);
                disabledNPCList.Remove(g);
                activeNPCList.Add(g);
            }
            else{
                int rand = Random.Range(0, NPCSpawners.Count - 1);
                //Debug.Log(NPCSpawners[rand].name + " being spawned on", NPCSpawners[rand]);
                Vector3 randPos = GetRandomPoint(NPCSpawners[rand]);
                //Debug.Log(randPos + " is the position within " + NPCSpawners[rand].name, NPCSpawners[rand]);
                Debug.DrawLine(this.transform.position, randPos, Color.red, 5f);
                GameObject g2 = Instantiate(g, randPos, Quaternion.identity);
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
                activeNPCList.Add(g);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnNPC();
        SpawnNPC();
        SpawnNPC();
        SpawnNPC();
        SpawnNPC();
        SpawnNPC();
        SpawnNPC();
        SpawnNPC();
        SpawnNPC();
        SpawnNPC();
    }

    public Vector3 GetRandomPoint(Transform center){
        Vector3 finalPos;
        Vector3 randomDirection = GetRandomDirectionVector3();
        RaycastHit hit;
        if(Physics.Raycast(center.position, randomDirection, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Collide)){
            //Debug.Log("using random spot");
            Debug.DrawLine(center.position, hit.point, Color.green, 5f);
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
}
