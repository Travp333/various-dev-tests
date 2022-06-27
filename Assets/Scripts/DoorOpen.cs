using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{   
    public GameObject player = default;
    float stopwatch;
    float countdown = 1;
    bool isTimer;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    public void Open() {
        animator.SetBool("OPEN", true);
        isTimer = true;
        
    }

    public void Close() {
        animator.SetBool("OPEN", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimer){
             stopwatch += Time.deltaTime;
             if (stopwatch == countdown){
                animator.SetBool("OPEN", false);
                isTimer = false;
                stopwatch = 0;
             }
        }
        
    }
}
