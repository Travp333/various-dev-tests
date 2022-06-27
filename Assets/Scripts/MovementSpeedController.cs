using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeedController : MonoBehaviour
{
    FPSMovingSphere player;

    [SerializeField, Range(0f, 100f)]
	[Tooltip("speeds of the character, these states represent the speed when your character is jogging, sprinting, walking, swimming, and climbing")]
	public float baseSpeed = 10f, sprintSpeed = 15f, maxClimbSpeed = 2f, maxSwimSpeed = 5f, walkSpeed = 7f;
    // Start is called before the first frame update
    public float currentSpeed;
    [SerializeField]
    float factor = 1f;
    float lastFactor;

    //this will determine how severely speed is impacted

    // kindof a rough system atm, but currently you need to set factor to one of these set values then when you reset it you need to set it to its alternative
    //(.1, 10), (.2, 5), (.4, 2.5), (.5, 2), (.8, 1.25)
    // these are the only clean divisions i could think of so that we end up at the exact number we started with 
    public void setFactor(float plug){
        factor = plug;
    }

    void Update() {
        MovementState(factor);
    }

    void Start() {
        player = GetComponent<FPSMovingSphere>();
    }
    void MovementState(float factor){
		//change movement speeds universally
        bool duckPressed = player.divingPrep;
		bool SprintPressed = Input.GetButton("Sprint");
		bool moving = Input.GetButton("Up") || Input.GetButton("Down") || Input.GetButton("Left") || Input.GetButton("Right");
		// default situation
		if ((!SprintPressed) && moving && !duckPressed ){
			currentSpeed = baseSpeed;
		}
		//sprinting
        if (moving && SprintPressed && player.velocity.magnitude >= 5f && !duckPressed && player.OnGround){
			currentSpeed = sprintSpeed;
        }
		//walking / crouching
		if (moving && duckPressed && !SprintPressed && player.OnGround && !player.ClimbingADJ){
			currentSpeed = walkSpeed;
		}
        else if (duckPressed && !player.OnGround && player.ClimbingADJ){
            currentSpeed  = maxClimbSpeed;
        }
        if(!player.OnGround){
            currentSpeed = baseSpeed;
        }
        if(currentSpeed <= 0){
            currentSpeed = 0;
        }
        if (factor != lastFactor){
            walkSpeed *= factor;
            sprintSpeed *= factor;
            baseSpeed *= factor;
            currentSpeed *= factor;		
            maxClimbSpeed *= factor;
            maxSwimSpeed *= factor;
            lastFactor = factor;
        }
	}
    public Vector3 ProjectDirectionOnPlane (Vector3 direction, Vector3 normal) {
		return (direction - normal * Vector3.Dot(direction, normal)).normalized;
	}
}
