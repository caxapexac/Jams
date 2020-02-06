using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 positionOffset;

    private GameObject plate;

    private Renderer rend;
    private Color startColor;

    void Start () 
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    void OnMouseDown ()
    {
        if (plate != null)
        {
            Debug.Log("Can't build there!");
            return;
        }

        GameObject plateToBuild = BuildManager.instance.GetPlateToBuild();
        plate = (GameObject)Instantiate(plateToBuild, transform.position, transform.rotation);
    }
}
