using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    [SerializeField]
    float lifeMin = 5;
    [SerializeField]
    float lifeMax = 10;
    void Start()
    {   
        Destroy(this.gameObject, Random.Range(lifeMin, lifeMax));
    }
}
