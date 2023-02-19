using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis Parks
//This script controls the speed that the barrage aniamtion plays
public class BarrageSpeed : MonoBehaviour
{
    float speed = .7f;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnDisable() {
        animator.SetFloat("Speed", 1f );
        speed = .7f;

    }
    void Update(){
        if ( speed < 1f){
            speed += .0005f;
        }
        if (speed >= 1f){
            speed = 1f;
        }
        animator.SetFloat("Speed", speed );
            
    }
}
