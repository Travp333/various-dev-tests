using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallMove : MonoBehaviour
{
    [SerializeField]
    float force; 
    [SerializeField]
    float radius; 
    [SerializeField]
    float upmodifier;
    [SerializeField]
    float lifeMin = 5;
    [SerializeField]
    float lifeMax = 10;
    Shatter otherExplosive;

    void Start(){
        Destroy(this.gameObject, Random.Range(lifeMin, lifeMax));
    }
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player" ){
            Vector3 distance = this.transform.position - other.transform.position;
            Debug.DrawRay(this.transform.position, distance,  Color.red, 3 );
            Vector3 newDistance = distance * (100f);
            other.rigidbody.AddExplosionForce(force, this.transform.position, radius, upmodifier, ForceMode.Impulse );

            if (other.rigidbody.TryGetComponent(out MovingSphere sphere)) {
				sphere.PreventSnapToGround();
            }
        Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Breakable" || other.gameObject.tag == "Explosive"){
                otherExplosive = other.gameObject.GetComponent<Shatter>();
                otherExplosive.oneShot(0);
                Destroy(this.gameObject);
        }
    }
}
