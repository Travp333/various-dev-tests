using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    Rigidbody body; 
    [SerializeField]
    float radius;
    [SerializeField]
    float power; 
    [SerializeField]
    float upModifier;
    [SerializeField]
    bool isBomb;
    [SerializeField]
    float otherExplosiveTime = 1f;
    Shatter otherExplosive;
    // Start is called before the first frame update
    void Start()
    {

        body = GetComponent<Rigidbody>();

        Vector3 explosionPos = transform.position;
        	Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        	foreach (Collider hit in colliders)
        	{
            	Rigidbody rb = hit.GetComponent<Rigidbody>();
                if(!isBomb){
                    if (hit.transform.IsChildOf(this.transform)){
                        if (rb != null)
                            rb.AddExplosionForce(power, explosionPos, radius, upModifier);
                        }
                }
                else{
                    if (rb != null){
                        if (rb.gameObject.tag == "Explosive"){
                            otherExplosive = rb.gameObject.GetComponent<Shatter>();
                            otherExplosive.oneShot(otherExplosiveTime);
                        }
                    rb.AddExplosionForce(power, explosionPos, radius, upModifier);
                    }
                }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
