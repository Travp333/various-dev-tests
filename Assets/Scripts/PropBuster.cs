using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBuster : MonoBehaviour
{
    [SerializeField]
    float power;
    [SerializeField]
    float radius;
    [SerializeField]
    bool oneShot = false;
    Shatter otherExplosive;
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.GetComponent<Rigidbody>() != null){
            if (other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.tag != "Breakable" || other.gameObject.tag != "Explosive"){
                other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(power, transform.root.position, radius);
            }
            if (other.gameObject.tag == "Breakable" || other.gameObject.tag == "Explosive"){
                if(!oneShot){
                    other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(power, transform.root.position, radius);
                    otherExplosive = other.gameObject.GetComponent<Shatter>();
                    otherExplosive.takeDamage();
                }
            else{
                otherExplosive = other.gameObject.GetComponent<Shatter>();
                otherExplosive.oneShot(0);
            }
            }

        }

    }
}

