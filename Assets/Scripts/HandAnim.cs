using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnim : MonoBehaviour
{
    MovementSpeedController speedController;
    [SerializeField]
    GameObject sphere;
    FPSMovingSphere player;
    [HideInInspector]
    public bool barragePrep = false;
    bool flipflop = true;
    bool flipflop2 = true;
    bool blocker = true;
    float charge;
    Animator animator;
    float playerSpeed;
    float playerSpeed2;
    float tempSpeed;
    float tempSpeed2;

    bool isOnGround;
    bool isOnSteep;

    [HideInInspector]
    public bool isOnGroundADJ;

    [SerializeField]
    [Tooltip("how long you need to be in the air before the 'onGround' bool triggers")]
    float OnGroundBuffer = .5f;
    [SerializeField]
    [Tooltip("how long isJumping stays true after pressing it ( maybe should be in movingsphere?)")]
    float JumpBuffer = .5f;

    bool JumpSwitch = true;
    float Groundstopwatch = 0;
    float Jumpstopwatch = 0;
    bool JumpPressed;
    Grab grab;
    // Start is called before the first frame update
    public bool getisThrowing(){
        return animator.GetBool("isThrowing");
    }
    public void setisHolding(bool plug){
        animator.SetBool("isHolding", plug);
    }
    public void setisThrowing(bool plug){
        animator.SetBool("grabCharge", !plug);
        animator.SetBool("isThrowing", plug);
    }

    void BoolAdjuster(){
        isOnGround = player.OnGround;
        isOnSteep = player.OnSteep;
        if (!isOnGround && !JumpPressed){
            Groundstopwatch += Time.deltaTime;
            if (Groundstopwatch >= OnGroundBuffer){
                isOnGroundADJ = false;
            }
        }
        if (!isOnGround && JumpPressed){
            isOnGroundADJ = false;
        }
        if(isOnGround){
            Groundstopwatch = 0;
            isOnGroundADJ = true;
        }
    }
    void Start()
    {
        speedController = sphere.GetComponent<MovementSpeedController>();
        animator = GetComponent<Animator>();
        player = sphere.GetComponent<FPSMovingSphere>();
        grab = GetComponent<Grab>();
    }

    void startBarrage(){
        animator.SetBool("barragePrep", false);
        player.isBarraging = true;
        flipflop2 = true;
        speedController.setFactor(.5f);
        //movement change here

    }

    void resetbarragePrep(){
        barragePrep = false;
        speedController.setFactor(2f);
        //movement restoration here
    }

    void openGate(){
        blocker = true;
    }
    void closeGate(){
        blocker = true;
    }

    void waveStartL(){
        blocker = false;
        flipflop = !flipflop;
        animator.SetBool("isPunchingLeft", true);
        Invoke("waveStop", .1f);
    }
    void waveStartR(){
        blocker = false;
        flipflop = !flipflop;
        animator.SetBool("isPunchingRight", true);
        Invoke("waveStop", .1f);

    }
    void waveStop(){
        
        animator.SetBool("isPunchingLeft", false);
        animator.SetBool("isPunchingRight", false);
        blocker = true;
       
    }

    // Update is called once per frame
    void Update()
    {
        if(grab.isgrabCharging){
            animator.SetBool("grabCharge", true);
        }
        else if (!grab.isgrabCharging){
            animator.SetBool("grabCharge", false);
        }
        BoolAdjuster();
        bool JumpPressed = Input.GetKey("space");
        isOnGround = isOnGroundADJ;

        if(isOnGround){
            animator.SetBool("isOnGround", true);
        }
        else if (!isOnGround){
            animator.SetBool("isOnGround", false);
        }

        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d") ){
            animator.SetBool("isMoving", true);
        }
        else{
            animator.SetBool("isMoving", false);
        }
        if (Input.GetKey("left shift")){
            animator.SetBool("isSprinting", true);
        }
        else {
            animator.SetBool("isSprinting", false);
        }
        if (Input.GetKey("c")){
            animator.SetBool("walkPressed", true);
        }
        else {
            animator.SetBool("walkPressed", false);
        }

        playerSpeed = sphere.GetComponent<Rigidbody>().velocity.magnitude;
        if (playerSpeed < .001f){
            animator.SetFloat("Blend", 0f);
        }
        else{
            if(playerSpeed >= 15f){
                animator.SetFloat("Blend", 1f);
            }
            else {
                playerSpeed2 = playerSpeed / 15f;
                animator.SetFloat("Blend", playerSpeed2);

                if(playerSpeed >= 10f){
                    animator.SetFloat("walkBlend", 1f);
                }
                if (playerSpeed < .001f){
                    animator.SetFloat("walkBlend", 0f);
                }
                playerSpeed = playerSpeed / 10f;
                animator.SetFloat("walkBlend", playerSpeed);
            }
        }
        if(Input.GetKeyDown("mouse 1")){
            if(flipflop2 && !player.isBarraging && !grab.isHolding){
                animator.SetBool("barragePrep", true);
                barragePrep = true;
                blocker = false;
                flipflop2 = false;
            }
            else if (!flipflop2){
                player.isBarraging = false;
                animator.SetBool("barragePrep", false);
                barragePrep = false;
                blocker = true;
                flipflop2 = true;
            }
        }
        if ( Input.GetKeyDown("mouse 0") ){
            if(blocker && !grab.isHolding){
                if(flipflop){
                    Invoke("waveStartL", .1f);
                }
                else if (!flipflop){
                    Invoke("waveStartR", .1f);
                }
            }
        }
        if ( Input.GetKey("mouse 0") ){
            if(barragePrep){
                animator.SetBool("charging", true);
                
            }
            else if (!barragePrep){
                animator.SetBool("charging", false);
                player.isBarraging = false;
            }
        }
        else if (!Input.GetKey("mouse 0") ){
            animator.SetBool("charging", false);
            player.isBarraging = false;
        }
    }
}
