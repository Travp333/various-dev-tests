using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneWarp : MonoBehaviour
{
    [SerializeField]
    float warpOffset;
    bool flipflop = true;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)){
            if (flipflop){
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + warpOffset );
                flipflop = false;
            }
            else if (!flipflop){
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - warpOffset );
                flipflop = true;
            }
        }
    }
}
