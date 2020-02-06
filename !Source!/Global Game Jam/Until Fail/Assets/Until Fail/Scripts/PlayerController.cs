using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 point = Vector3.zero;

    void Update()
    {
        //Debug.DrawLine(Vector3.zero, point);
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //point = hit.point;
                //Debug.Log(hit.transform.tag);
                if (hit.transform.tag == "Tile")
                {
                    //Todo
                    var dm = hit.transform.gameObject.GetComponentInParent<DurabilityManager>();
                    dm.Repair();
                    //if (dm.CurrentDurability < dm.MaxDurability)
    
                    //hit.transform.gameObject.GetComponentInChildren<DurabilityManager>().wasPlaced = true;
                    //hit.transform.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    //Debug.Log(hit.transform.parent);
                    //Debug.Log(hit.transform.parent.GetComponent<Collider>());
                    //hit.transform.parent.GetComponent<BoxCollider>().enabled = true;
                    //hit.transform.parent.GetComponentInChildren<Canvas>().enabled = true;
                }
            }

            //onTriggerExit()
            //send info to the car
            //take damage
        }
    }
}
