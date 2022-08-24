using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KaylaFaceAnimController : MonoBehaviour
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
    int happyID = 32;
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
                //Debug.Log("angry Left");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryLeftID));
            }
            else if (isLookingRight){
                //Debug.Log("angry Right");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryRightID));
            }
            else{
                //Debug.Log("angry");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryID));
            }
        }
        else if(isScared){
            if(isLookingLeft){
                //Debug.Log("scared left");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredLeftID));
            }
            else if (isLookingRight){
                //Debug.Log("scared left");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredRightID));
            }
            else{
                //Debug.Log("scared");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredID));
            }
        }
        else if(isSad){
            if(isLookingLeft){
                //Debug.Log("sad left");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadLeftID));
            }
            else if (isLookingRight){
                //Debug.Log("sad right");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadRightID));
            }
            else{
                //Debug.Log("sad");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadID));
            }
        }
        else if(isHappy){
            //Debug.Log("happy");
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyID));
        }
        else if(isTrance){
            //Debug.Log("trance");
            tranceFlip();
            //this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(13));
        }
        else{
            if(isLookingLeft){
                //Debug.Log("Base left");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseLeftID));
            }
            else if (isLookingRight){
                //Debug.Log("Base right");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseRightID));
            }
            else{
                //Debug.Log("Base");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseID));
            }
        }
        StartCoroutine(callBlinkDown());
        //Invoke("BlinkDown", Random.Range(5,10));
    }


    void BlinkDown(){
        if(isAngry){
            if(isLookingLeft){
                //Debug.Log("angry blink down left");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryLeftBlinkID));
            }
            else if (isLookingRight){
                //Debug.Log("angry blink down right");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryRightBlinkID));
            }
            else{
                //Debug.Log("angry blink down");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryBlink1ID));
            }
        }
        else if(isScared){
            if(isLookingLeft){
                //Debug.Log("scared blink down left");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredLeftBlinkID));
            }
            else if (isLookingRight){
                //Debug.Log("scared blink down right");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredRightBlinkID));
            }
            else{
                //Debug.Log("scared blink down");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredBlink1ID));
            }
        }
        else if(isSad){
            if(isLookingLeft){
                //Debug.Log("sad blink down left");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadLeftBlinkID));
            }
            else if (isLookingRight){
                //Debug.Log("sad blink down right");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadRightBlinkID));
            }
            else{
                //Debug.Log("sad blink down");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadBlink1ID));
            }
        }
        else if(isHappy){
            //Debug.Log("happy blink down");
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyID));
        }
        else if(isTrance){
            //Debug.Log("trance blink down");
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(TranceBlink1ID));
            Invoke("flipTrance", .025f);
        }
        else{
            if(isLookingLeft){
                //Debug.Log("Base blink down left");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseLeftBlinkID));
            }
            else if(isLookingRight){
                //Debug.Log("Base blink down right");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseRightBlinkID));
            }
            else{
                //Debug.Log("Base blink down");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseBlink1ID));
            }
        }
        StartCoroutine(callBlink());
        //Invoke("Blink", .05f);
    }
    void Blink(){

        if(isAngry){
            //Debug.Log("angry blink");
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryBlink2ID));
        }
        else if(isScared){
            //Debug.Log("scared blink");
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredBlink2ID));
        }
        else if(isSad){
            //Debug.Log("sad blink");
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadBlink2ID));
        }
        else if(isHappy){
            //Debug.Log("happy blink");
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyID));
        }
        else if(isTrance){
            //Debug.Log("trance blink");
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredBlink2ID));
        }
        else{
            //Debug.Log("Base blink");
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseBlink2ID));
        }
        StartCoroutine(callBlinkUp());
        //Invoke("BlinkUp", .1f);
    }
    void BlinkUp(){
        if(isAngry){
            if(isLookingLeft){
                //Debug.Log("angry blink down left");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryLeftBlinkID));
            }
            else if (isLookingRight){
                //Debug.Log("angry blink down right");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryRightBlinkID));
            }
            else{
                //Debug.Log("angry blink down");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(angryBlink1ID));
            }
        }
        else if(isScared){
            if(isLookingLeft){
                //Debug.Log("scared blink down left");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredLeftBlinkID));
            }
            else if (isLookingRight){
                //Debug.Log("scared blink down right");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredRightBlinkID));
            }
            else{
                //Debug.Log("scared blink down");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(scaredBlink1ID));
            }
        }
        else if(isSad){
            if(isLookingLeft){
                //Debug.Log("sad blink down left");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadLeftBlinkID));
            }
            else if (isLookingRight){
                //Debug.Log("sad blink down right");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadRightBlinkID));
            }
            else{
                //Debug.Log("sad blink down");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(sadBlink1ID));
            }
        }
        else if(isHappy){
            //Debug.Log("happy blink down");
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(happyID));
        }
        else if(isTrance){
            //Debug.Log("trance blink down");
            this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(TranceBlink1ID));
            Invoke("flipTrance", .025f);
        }
        else{
            if(isLookingLeft){
                //Debug.Log("Base blink down left");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseLeftBlinkID));
            }
            else if(isLookingRight){
                //Debug.Log("Base blink down right");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseRightBlinkID));
            }
            else{
                //Debug.Log("Base blink down");
                this.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texHol.getTex(baseBlink1ID));
            }
        }
        StartCoroutine(callBase());
        //Invoke("Base", .05f);
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
            Debug.Log("deciding which way to look using the number " + random);
            if(random == 4){
                Debug.Log("Looking Left");
                isLookingLeft = true;
            }
            else if (random == 5){
                Debug.Log("Looking Right");
                isLookingRight = true;
            }
            else{
                Debug.Log("Looking Straight");
                isLookingRight = false;
                isLookingLeft = false;
            }
        }
    }


}
