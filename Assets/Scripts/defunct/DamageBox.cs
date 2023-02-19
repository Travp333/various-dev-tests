//Notes: 
// I want it to knock you upwards more when youre on the ground and maybe it should knock you only sideways if youre in the air so you cant damage boost and super jump
// should the audio be here or in the player? also how do you do "audio arrays"?
// consider finding a way to make it so they obstacles can be stopped if the object has a high enough mass and make that feature toggleable
// THE WEIRD PERPENDICULAR LAUNCH IS CAUSE SNAPPING DUMMY

using UnityEngine;

public class DamageBox : MonoBehaviour
{      
    public Animator animator;
    public AudioSource pain;
    
    //which direction is the object facing in?
    //bool whichTarget;
    // how hard will it get pushed in that direction?
    [SerializeField , Range(1, 50000)]
    float force = 3;
    //first target for the object
    public Transform target;
    //second target for the object 
    public Transform target2;
    //which target is the object moving towards?
    Transform dest;
    //how quickly will it get there?
    public float speed;
    // how much HP will it take from the player

    [SerializeField , Range(1, 100)]
    int damage;

    // which "player" does it interact with?
    PlayerStats stats = default;
    public GameObject player = default;
    // is the cooldown active or not?
    bool coolDown = false;
    // timer
    float stopwatch;
    // how long the cooldown should be active
    [SerializeField , Range(1, 5)]
    float coolDownTimer = 1;

    public float radius = 20.0F;

    [SerializeField , Range(1, 50000)]
    float upForce;

    void cooldown(){
        //Debug.Log("cooldown started");
        coolDown = true;
    }

//(Vector3.up.normalized * 5))
    void OnTriggerStay(Collider other){
        if (other == stats.GetComponent<Collider>() && !coolDown){
            pain.Play();
            cooldown();
            stats.hp -= damage;
            animator.SetBool("TookDamage", true);
            //Vector3 dir = transform.GetChild(0).position - sphere.transform.position;
            //dirUp = Quaternion.Euler(0, 0, 90) * dir;
            //dir = -dir.normalized;

            if (animator.GetBool("isJumping") || animator.GetBool("isFalling") || (animator.GetBool("isOnSteep") && !animator.GetBool("isOnGround"))){
                //Debug.Log("in air hit");
                stats.GetComponent<Rigidbody>().AddExplosionForce(force, transform.GetChild(0).position, radius, -upForce);
   
            }
            else{
               // Debug.Log("on ground hit");
                stats.GetComponent<Rigidbody>().AddExplosionForce(force, transform.GetChild(0).position, radius, -upForce);
            }

            

    }
    }
    void Start() {
        pain = GetComponent<AudioSource>();
        dest = target;
        stats = player.GetComponent<PlayerStats>();
    }

    void Update() {
        Vector3 up = CustomGravity.GetUpAxis(this.transform.position);
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, dest.position, step);
        if ( Vector3.Distance(transform.position, target.position) < 0.001f){
            //whichTarget = true;
            dest = target2;
        }
        if ( Vector3.Distance(transform.position, target2.position) < 0.001f){
            //whichTarget = false;
            dest = target; 
        } 

        if(coolDown){
           // Debug.Log("timer started");
            stopwatch += Time.deltaTime;
            if (stopwatch >= coolDownTimer){
                animator.SetBool("TookDamage", false);
               // Debug.Log("cooldown stopped");
                coolDown = false;
                stopwatch = 0;
            }
        }
        
    }


}