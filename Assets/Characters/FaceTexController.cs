using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTexController : MonoBehaviour
{
    bool isAngry, isScared, isSad, isHappy, isTrance;
    bool flipFlop;
    bool isBlinking;
    Vector2 angryID = new Vector2(-0.625f, 0.125f), angryBlink1ID = new Vector2(-.125f*4f, 0.125f), angryBlink2ID = new Vector2(-.125f*3f, 0.125f), angryLeftID = new Vector2(-.125f, .125f), angryLeftBlinkID = new Vector2(-.125f -.125f, .125f), angryRightID = new Vector2(.125f, .125f), angryRightBlinkID = new Vector2(0f, .125f);
    Vector2 sadID = new Vector2(0.25f, -0.25f), sadBlink1ID = new Vector2(-.125f * 4f, -.125f -.125f), sadBlink2ID = new Vector2(-.125f - .125f * 2f, -.125f -.125f), sadLeftID = new Vector2(-.125f, -.125f - .125f), sadLeftBlinkID = new Vector2(-.125f -.125f, -.125f - .125f), sadRightID = new Vector2(.125f, -.125f - .125f), sadRightBlinkID = new Vector2(0f, -.125f -125f -.125f);
    Vector2 baseID = new Vector2(0f, 0f), baseBlink1ID = new Vector2(0f +.125f*2f, 0f + .125f), baseBlink2ID = new Vector2(-.125f * 5f, 0f), baseLeftID = new Vector2(-.125f * 3f, 0f), baseLeftBlinkID = new Vector2(-.125f * 4f, 0f), baseRightID = new Vector2(-.125f * 1f, 0f), baseRightBlinkID = new Vector2(-.125f * 2f, 0f);
    Vector2 TranceID = new Vector2(-0.375f, -0.5f), TranceFlipID = new Vector2(-0.375f - .125f, -0.5f), TranceBlink1ID = new Vector2(-0.375f - .125f*2f, -0.5f),  TranceFlipBlink1ID = new Vector2(-0.375f + .125f*5f, -0.5f + .125f);
    Vector2 scaredID = new Vector2(0.125f, -0.375f), scaredBlink1ID = new Vector2(-.125f*5f, -.125f * 3f), scaredBlink2ID = new Vector2(-.125f*4f, -.125f * 3f), scaredRightID = new Vector2(0f, -.125f*3f), scaredRightBlinkID = new Vector2(-.125f, -.125f*3f), scaredLeftID = new Vector2(-.125f -.125f, -.125f*3f), scaredLeftBlinkID = new Vector2(- .125f*3f, -.125f*3f);
    Vector2 happyID = new Vector2(-0.125f, -0.125f), happyBlink1ID = new Vector2(-0.125f + .125f*2f, -0.125f + .125f), happyBlink2ID = new Vector2(.125f + .125f, 0f), happyLeftID = new Vector2(.125f * -4f, -.125f), happyLeftBlinkID = new Vector2(-0.125f + .125f*4f, -0.125f), happyRightID = new Vector2(-.125f -.125f, -.125f), happyRightBlinkID = new Vector2(.125f * -3f, -.125f);
    bool isLookingLeft, isLookingRight;
    [SerializeField]
    bool forceLeft, forceRight, forceStraight;
    [SerializeField]
    float blinkTimerLow = 5f, blinkTimerHigh = 10f;
    //Start is called before the first frame update
    void Start(){
        Base();
    }

    public void setAngry(){
        StopAllCoroutines();
        isAngry = true;
        isScared = false;
        isSad = false;
        isHappy = false;
        isTrance = false;
        //Debug.Log("angry");
        this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", angryID);
        Base();

    }
    public void setBase(){
        StopAllCoroutines();
        isAngry = false;
        isScared = false;
        isSad = false;
        isHappy = false;
        isTrance = false;
        //Debug.Log("base");
        this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", baseID);

        Base();
    }
    public void setScared(){
        StopAllCoroutines();
        isAngry = false;
        isScared = true;
        isSad = false;
        isHappy = false;
        isTrance = false;
        //Debug.Log("scared");
        this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", scaredID);
        Base();
    }
    public void setTrance(){
        StopAllCoroutines();
        isAngry = false;
        isScared = false;
        isSad = false;
        isHappy = false;
        isTrance = true;
        //Debug.Log("trance");
        this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", TranceID);
        Base();
    }
    public void setHappy(){
        StopAllCoroutines();
        isAngry = false;
        isScared = false;
        isSad = false;
        isHappy = true;
        isTrance = false;
        //Debug.Log("happy");
        this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", happyID);
        Base();
    }
    public void setSad(){
        StopAllCoroutines();
        isAngry = false;
        isScared = false;
        isSad = true;
        isHappy = false;
        isTrance = false;
        //Debug.Log("sad");
        this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", sadID);
        Base();
    }

    void tranceFlip(){
        if(isTrance && !isBlinking){
            if(flipFlop){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", TranceID);
                flipFlop = !flipFlop;
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", TranceFlipID);
                flipFlop = !flipFlop;
            }
            Invoke("tranceFlip", .1f);
        }

    }
    void Base(){
        if(isAngry){
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", angryLeftID);
            }
            else if (isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", angryRightID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", angryID);
            }
        }
        else if(isScared){
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", scaredLeftID);
            }
            else if (isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", scaredRightID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", scaredID);
            }
        }
        else if(isSad){
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", sadLeftID);
            }
            else if (isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", sadRightID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", sadID);
            }
        }
        else if(isHappy){
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", happyLeftID);
            }
            else if (isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", happyRightID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", happyID);
            }
        }
        else if(isTrance){
            tranceFlip();
        }
        else{
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", baseLeftID);
            }
            else if (isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", baseRightID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", baseID);
            }
        }
        StartCoroutine(callBlinkDown());
    }


    void BlinkDown(){
        if(isAngry){
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", angryLeftBlinkID);
            }
            else if (isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", angryRightBlinkID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", angryBlink1ID);
            }
        }
        else if(isScared){
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", scaredLeftBlinkID);
            }
            else if (isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", scaredRightBlinkID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", scaredBlink1ID);
            }
        }
        else if(isSad){
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", sadLeftBlinkID);
            }
            else if (isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", sadRightBlinkID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", sadBlink1ID);
            }
        }
        else if(isHappy){
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", happyLeftBlinkID);
            }
            else if (isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", happyRightBlinkID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", happyBlink1ID);                
            }
        }
        else if(isTrance){
            this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", TranceBlink1ID);
            Invoke("flipTrance", .025f);
        }
        else{
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", baseLeftBlinkID);
            }
            else if(isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", baseRightBlinkID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", baseBlink1ID);
            }
        }
        StartCoroutine(callBlink());
    }
    void Blink(){
        if(isAngry){
            this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", angryBlink2ID);
        }
        else if(isScared || isTrance){
            this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", scaredBlink2ID);
        }
        else if(isSad){
            this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", sadBlink2ID);
        }
        else if(isHappy){
            this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", happyBlink2ID);
        }
        else{
            this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", baseBlink2ID);
        }
        StartCoroutine(callBlinkUp());
    }
    void BlinkUp(){
        if(isAngry){
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", angryLeftBlinkID);
            }
            else if (isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", angryRightBlinkID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", angryBlink1ID);
            }
        }
        else if(isScared){
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", scaredLeftBlinkID);
            }
            else if (isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", scaredRightBlinkID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", scaredBlink1ID);
            }
        }
        else if(isSad){
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", sadLeftBlinkID);
            }
            else if (isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", sadRightBlinkID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", sadBlink1ID);
            }
        }
        else if(isHappy){
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", happyLeftBlinkID);
            }
            else if (isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", happyRightBlinkID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", happyBlink1ID);
            }
        }
        else if(isTrance){
            this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", TranceBlink1ID);
            Invoke("flipTrance", .025f);
        }
        else{
            if(isLookingLeft){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", baseLeftBlinkID);
            }
            else if(isLookingRight){
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", baseRightBlinkID);
            }
            else{
                this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", baseBlink1ID);
            }
        }
        StartCoroutine(callBase());
    }
    void flipTrance(){
        this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", TranceFlipBlink1ID);
    }

    IEnumerator callBlink(){
        yield return new WaitForSeconds(.05f);
        Blink();
    }
    IEnumerator callBlinkUp(){
        yield return new WaitForSeconds(.1f);
        BlinkUp();
    }
    IEnumerator callBlinkDown(){
        yield return new WaitForSeconds(Random.Range(blinkTimerLow,blinkTimerHigh));
        BlinkDown();
        isBlinking = true;
    }
    IEnumerator callBase(){
        yield return new WaitForSeconds(.05f);
        isBlinking = false;
        chooseViewDirection();
        Base();
        
    }

    void chooseViewDirection(){
        if(forceLeft){
            isLookingLeft = true;
            isLookingRight = false;
        }
        else if(forceRight){
            isLookingRight = true;
            isLookingLeft = false;
        }
        else if(forceStraight){
            isLookingRight = false;
            isLookingLeft = false;
        }
        else{
            int random = Random.Range(0, 10);
            if(random == 4){
                isLookingLeft = true;
            }
            else if (random == 5){
                isLookingRight = true;
            }
            else{
                isLookingRight = false;
                isLookingLeft = false;
            }
        }
    }

}
