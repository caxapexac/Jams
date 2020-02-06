using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    
    BuildManager buildManager;

    void Start ()
    {
        buildManager = BuildManager.instance;
    }
    public void PurchaseStandartPlate() {

        Debug.Log("Standart Plate purchaset!");
        buildManager.SetPlateToBuild(buildManager.standartPlatePrefab);
    }
}
