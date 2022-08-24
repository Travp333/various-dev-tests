using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityGuardAnimController : MonoBehaviour
{
    [SerializeField]
    public TextureHolder texHol;
    bool isAngry, isScared, isSad, isHappy, isTrance;
    bool flipFlop;
    bool isBlinking;
    int angryID = 7, angryBlink1ID = 8, angryBlink2ID = 9, angryLeftID = 10, angryLeftBlinkID = 11, angryRightID = 12, angryRightBlinkID = 13;
    int sadID = 14, sadBlink1ID = 15, sadBlink2ID = 16, sadLeftID = 17, sadLeftBlinkID = 18, sadRightID = 19, sadRightBlinkID = 20;
    int baseID = 0, baseBlink1ID = 1, baseBlink2ID = 2, baseLeftID = 3, baseLeftBlinkID = 4, baseRightID = 5, baseRightBlinkID = 6;
    int TranceID = 28, TranceFlipID = 29, TranceBlink1ID = 30,  TranceFlipBlink1ID = 31;
    int scaredID = 21, scaredBlink1ID = 22, scaredBlink2ID = 23, scaredLeftID = 26, scaredLeftBlinkID = 27, scaredRightID = 24, scaredRightBlinkID = 25;
    int happyID = 32, happyBlink1ID = 33, happyBlink2ID = 34, happyLeftID = 35, happyLeftBlinkID = 36, happyRightID = 37, happyRightBlinkID = 38;
    bool isLookingLeft, isLookingRight;
    [SerializeField]
    bool forceLeft, forceRight, forceStraight;
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
        this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryID));
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
        this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseID));
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
        this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredID));
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
        this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(TranceID));
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
        this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyID));
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
        this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadID));
        Base();
    }

    void tranceFlip(){
        if(isTrance && !isBlinking){
            if(flipFlop){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(TranceID));
                flipFlop = !flipFlop;
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(TranceFlipID));
                flipFlop = !flipFlop;
            }
            Invoke("tranceFlip", .1f);
        }

    }
    void Base(){
        if(isAngry){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryLeftID));
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryRightID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryID));
            }
        }
        else if(isScared){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredLeftID));
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredRightID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredID));
            }
        }
        else if(isSad){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadLeftID));
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadRightID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadID));
            }
        }
        else if(isHappy){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyLeftID));
            }
            else if (isLookingRight){

                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyRightID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyID));
            }
        }
        else if(isTrance){
            tranceFlip();
        }
        else{
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseLeftID));
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseRightID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseID));
            }
        }
        StartCoroutine(callBlinkDown());
    }


    void BlinkDown(){
        if(isAngry){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryLeftBlinkID));
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryRightBlinkID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryBlink1ID));
            }
        }
        else if(isScared){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredLeftBlinkID));
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredRightBlinkID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredBlink1ID));
            }
        }
        else if(isSad){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadLeftBlinkID));
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadRightBlinkID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadBlink1ID));
            }
        }
        else if(isHappy){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyLeftBlinkID));
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyRightBlinkID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyBlink1ID));
            }
        }
        else if(isTrance){
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(TranceBlink1ID));
            Invoke("flipTrance", .025f);
        }
        else{
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseLeftBlinkID));
            }
            else if(isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseRightBlinkID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseBlink1ID));
            }
        }
        StartCoroutine(callBlink());
    }
    void Blink(){
        if(isAngry){
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryBlink2ID));
        }
        else if(isScared){
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredBlink2ID));
        }
        else if(isSad){
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadBlink2ID));
        }
        else if(isHappy){
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyBlink2ID));
        }
        else if(isTrance){
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredBlink2ID));
        }
        else{
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseBlink2ID));
        }
        StartCoroutine(callBlinkUp());
    }
    void BlinkUp(){
        if(isAngry){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryLeftBlinkID));
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryRightBlinkID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryBlink1ID));
            }
        }
        else if(isScared){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredLeftBlinkID));
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredRightBlinkID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredBlink1ID));
            }
        }
        else if(isSad){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadLeftBlinkID));
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadRightBlinkID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadBlink1ID));
            }
        }
        else if(isHappy){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyLeftBlinkID));
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyRightBlinkID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyBlink1ID));
            }
        }
        else if(isTrance){
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(TranceBlink1ID));
            Invoke("flipTrance", .025f);
        }
        else{
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseLeftBlinkID));
            }
            else if(isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseRightBlinkID));
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseBlink1ID));
            }
        }
        StartCoroutine(callBase());
    }
    void flipTrance(){
        this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(TranceFlipBlink1ID));
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
        yield return new WaitForSeconds(Random.Range(5,10));
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
            int random = Random.Range(0, 6);
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
