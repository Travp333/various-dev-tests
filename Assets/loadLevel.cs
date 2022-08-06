using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadLevel : MonoBehaviour
{
    public int iLevelToLoad;
    public string sLevelToLoad;

    public bool useIntegerToLoadLevel = false;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionGameObject = other.gameObject;

        if(collisionGameObject.tag == "Player")
        {
            LoadScene();
        }

    }

   
    /*void onCollisionEnter(Collision gameObjectInformation)
    {
        if(gameObjectInformation.gameObject.tag == "Player")
        {
            Debug.Log("Collision Detected");
            SceneManager.LoadScene("Level02");
        }
    }*/

    void LoadScene()
    {
        if (useIntegerToLoadLevel)
        {
            SceneManager.LoadScene(iLevelToLoad);
        } else
        {
            SceneManager.LoadScene(sLevelToLoad);
        }
    }
} 
