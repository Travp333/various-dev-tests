using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Animator animator;
    public GameObject shellPrefab; // Add Shell game object in the Inspector.
    public GameObject shellSpawnPos; // Add Cube game object in the Inspector.
    public GameObject target; // Add Enemy game object in the Inspector.
    public GameObject parent; // Add Tank game object in the Inspector.

    [SerializeField]
    float speed = 15;
    float turnSpeed = 2;
    bool canShoot = true;
    [SerializeField]
    bool highorLow;
    int ShootHash;

    [SerializeField]
    [Tooltip("its like accuracy i think?")]
    float sensitivityRange;
    [SerializeField]
    [Tooltip("angle between cannon and target determines when it will shoot, this decides sensitivity ")]
    float detectionAngle = 10;

    [SerializeField]
    [Tooltip("will it hitscan or use projectile")]
    bool hitscan = false;
    [SerializeField]
    [Tooltip("what kind of stuff will not collide with ammo")]
    LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        ShootHash = Animator.StringToHash("shoot");
    }

    void CanShootAgain()
    {
        canShoot = true;
        
    }

    void animatorReset(){
        animator.SetBool(ShootHash, false);
    }
    void Fire()
    {
        if (canShoot)
        {
            if(!hitscan){
                Debug.DrawRay(this.transform.position, shellSpawnPos.transform.forward, Color.blue, 5f );
                Debug.DrawRay(this.transform.position, this.transform.forward, Color.red, 5f );
                GameObject shell = Instantiate(shellPrefab, shellSpawnPos.transform.position, shellSpawnPos.transform.rotation);
                shell.GetComponent<Rigidbody>().velocity = speed * this.transform.forward; // Use 'forward' because it's the Z axis you want to shoot along.
                shell.GetComponent<Rigidbody>().angularVelocity = speed*5 * Random.insideUnitSphere;
                canShoot = false;
                animator.SetBool(ShootHash, true);
                Invoke("CanShootAgain", .5f); // Increase value to slow down rate of fire, decrease value to speed up rate of fire.
                Invoke("animatorReset", .01f);
            }
            else{
                RaycastHit hit;
                if (Physics.Raycast(shellSpawnPos.transform.position, shellSpawnPos.transform.forward, out hit, Mathf.Infinity, mask )){
                    Instantiate(shellPrefab, hit.point, shellSpawnPos.transform.rotation);
                }
                Debug.DrawRay(shellSpawnPos.transform.position, shellSpawnPos.transform.forward * 100 , Color.green, 1f );
                canShoot = false;
                animator.SetBool(ShootHash, true);
                Invoke("CanShootAgain", .5f);
                Invoke("animatorReset", .01f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (target.transform.position - parent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        parent.transform.rotation = Quaternion.Slerp(parent.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        if(!hitscan){
            float? angle = RotateTurret();
            if(angle != null && Vector3.Angle(direction, parent.transform.forward) < detectionAngle) // When the angle is less than 10 degrees...
            Fire(); // ...start firing.
        }
        else{
            Vector3 Hitdirection = (target.transform.position - shellSpawnPos.transform.position);
            Quaternion HitscanRotation = Quaternion.LookRotation(Hitdirection);
           // Quaternion ForwardRotation = Quaternion.LookRotation(this.transform.forward);
            float angle = Vector3.Angle((target.transform.position - this.transform.position), this.transform.forward);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, HitscanRotation, Time.deltaTime * turnSpeed);
            Fire();
            
            //this.transform.localEulerAngles = new Vector3(this.transform.rotation.x, 0, 0); 
            //this.transform.localEulerAngles = new Vector3(360f - angle, 0f, 0f);
        }

        
    }

    float? RotateTurret()
    {
        float? angle;
        angle = CalculateAngle(highorLow); // Set to false for upper range of angles, true for lower range.
        if (angle != null) // If we did actually get an angle...
        {
            if((target.transform.position - shellSpawnPos.transform.position).magnitude > sensitivityRange ){
                    this.transform.localEulerAngles = new Vector3(360f - (float)angle, 0f, 0f); // ... rotate the turret using EulerAngles because they allow you to set angles around x, y & z.
            }
        }

        return angle;
    }
    float? CalculateAngle(bool low)
    {
        
        Vector3 targetDir = target.transform.position - shellSpawnPos.transform.position;
        Debug.DrawRay(shellSpawnPos.transform.position, targetDir, Color.green, 5f );
        float y = targetDir.y;
        targetDir.y = 0f;
        float x = targetDir.magnitude;
        float gravity = 9.81f;
        float sSqr = speed * speed;
        float underTheSqrRoot = (sSqr * sSqr) - gravity * (gravity * x * x + 2 * y * sSqr);

        if (underTheSqrRoot >= 0f)
        {
            float root = Mathf.Sqrt(underTheSqrRoot);
            float highAngle = sSqr + root;
            float lowAngle = sSqr - root;

            if (low){   
                return (Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg);
            }
            else{
                return (Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg);
            }

        }
        else
            return null;
    }
}

