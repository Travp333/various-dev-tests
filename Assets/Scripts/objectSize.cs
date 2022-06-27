using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectSize : MonoBehaviour
{
    [SerializeField]
    [Tooltip ("Just pick one of these to determine the position of grab point, the size of the hit box, and more")]
    public bool isSmall;
    [SerializeField]
    [Tooltip ("Just pick one of these to determine the position of grab point, the size of the hit box, and more")]
    public bool isMedium;
    [SerializeField]
    [Tooltip ("Just pick one of these to determine the position of grab point, the size of the hit box, and more")]
    public bool isLarge;
}
