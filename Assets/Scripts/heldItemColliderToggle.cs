using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heldItemColliderToggle : MonoBehaviour
{
    [SerializeField]
    Grab grab;
    GameObject smallCollider;
    GameObject medCollider;
    GameObject largeCollider;
    // Start is called before the first frame update
    void Start()
    {
        smallCollider = transform.GetChild(0).gameObject;
        medCollider = transform.GetChild(1).gameObject;
        largeCollider = transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if( grab.isHolding ){
            if( grab.SmallMediumLarge == "LARGE"){
                largeCollider.SetActive(true);
            }
            if( grab.SmallMediumLarge == "MEDIUM"){
                medCollider.SetActive(true);
            }
            if( grab.SmallMediumLarge == "SMALL"){
                smallCollider.SetActive(true);
            }
        }
        else{
            smallCollider.gameObject.SetActive(false);
            medCollider.SetActive(false);
            largeCollider.SetActive(false);
        }
    }
}
