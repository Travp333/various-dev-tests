using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaylaFaceAnimController : MonoBehaviour
{
    [SerializeField]
    public TextureHolder texHol;
    // Start is called before the first frame update
    void Start(){

        Flip1();
        
    }
    void Flip1(){
        this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(0));
        Invoke("Flip", Random.Range(5,10));  
    }

    void Flip(){
        this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(1));
        Invoke("Flip2", .05f);
    }
    void Flip2(){
        this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(2));
        Invoke("Flip3", .1f);
    }
    void Flip3(){
        this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(1));
        Invoke("Flip1", .05f); 
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
