using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectSize : MonoBehaviour
{
    [SerializeField]
    public string objectName;
    [SerializeField]
    [Tooltip ("Just pick one of these to determine the position of grab point, the size of the hit box, and more")]
    public bool isSmall;
    [SerializeField]
    [Tooltip ("Just pick one of these to determine the position of grab point, the size of the hit box, and more")]
    public bool isMedium;
    [SerializeField]
    [Tooltip ("Just pick one of these to determine the position of grab point, the size of the hit box, and more")]
    public bool isLarge;
    [SerializeField]
    public bool isTrash;
    [SerializeField]
    public enum trash{electronic, food, recycleable, misc};
    public trash trashType;
    public bool isTargeted;
    [SerializeField]
    [Tooltip ("amount of time before the trash resets its targeted status")]
    public float timeRemaining = 10f;
    

    void Update()
    {
        if(isTargeted == true){
            if(timeRemaining > 0){
                timeRemaining -= Time.deltaTime;
            }
            else{
                isTargeted = false;
                timeRemaining = 10f;
            }
        }
    }
}
