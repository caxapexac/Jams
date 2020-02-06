using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManger in scene!");
            return;
        }
        instance = this;
    }
    
    public GameObject standartPlatePrefab;

    private GameObject plateToBuild;

    public GameObject GetPlateToBuild() 
    {
        return plateToBuild;
    }

    public void SetPlateToBuild (GameObject plate)
    {
        plateToBuild = plate;
    }
}
