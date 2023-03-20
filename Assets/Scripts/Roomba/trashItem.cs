using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashItem : MonoBehaviour
{
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
        if(isTrash){
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
}
