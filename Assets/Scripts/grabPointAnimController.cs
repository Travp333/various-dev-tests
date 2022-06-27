using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabPointAnimController : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    Grab grab;
    [SerializeField]
    HandAnim hand;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hand.getisThrowing()){
            anim.SetBool("isThrowing", true);
        }
        else {
            anim.SetBool("isThrowing", false);
        }
        if (grab.isgrabCharging){
            anim.SetBool("isCharging", true);
        }
        else{
            anim.SetBool("isCharging", false);
        }
        if (grab.SmallMediumLarge == "LARGE" && grab.SmallMediumLarge != "MEDIUM" && grab.SmallMediumLarge != "SMALL"){
            anim.SetBool("Large", true);
            anim.SetBool("Medium", false);
            anim.SetBool("Small", false);
        }
        if (grab.SmallMediumLarge == "MEDIUM" && grab.SmallMediumLarge != "LARGE" && grab.SmallMediumLarge != "SMALL"){
            anim.SetBool("Large", false);
            anim.SetBool("Medium", true);
            anim.SetBool("Small", false);
        }
        if (grab.SmallMediumLarge == "SMALL" && grab.SmallMediumLarge != "MEDIUM" && grab.SmallMediumLarge != "LARGE"){
            anim.SetBool("Large", false);
            anim.SetBool("Medium", false);
            anim.SetBool("Small", true);
        }
        else if (grab.SmallMediumLarge == "NULL"){
            anim.SetBool("Large", false);
            anim.SetBool("Medium", false);
            anim.SetBool("Small", false);
        }

    }
}
