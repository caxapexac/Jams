using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class CarComponent : MonoBehaviour
{
    public Transform RootMesh; //Inspector
    public ParticleSystem Explosion;
    public WaypointProgressTracker CarProgressTracker;

    public int Money;

    public float triggerLastTime;
    const float triggerDelay = 0.5f;

    public int DamageMade;

    public bool Destroyed;

    
    void Awake()
    {
        Destroyed = false;
        CarProgressTracker = GetComponent<WaypointProgressTracker>();
        triggerLastTime = -1000;
        DamageMade = Money;
    }

    void OnCollisionEnter(Collision other)
    {
        //if (Vector3.Dot(other.transform.position, transform.position) > 0) return;
        if (other.gameObject.GetComponentInParent<CarComponent>())
        {
            //Boom
            Explosion.Play();
            CameraShaker.Instance.Shake();
            Destroy(gameObject, 2f);
        }
        
    }

    public void ChangeWaypoint(WaypointCircuit circuit)
    {
        if (CarProgressTracker.circuit?.transform.parent == circuit.transform.parent) return;
        if (Time.time - triggerLastTime < triggerDelay) return;
        triggerLastTime = Time.time;
        //Debug.Log("Reset");
        CarProgressTracker.circuit = circuit;
        CarProgressTracker.Reset();
        
    }

    public void SetupMesh(GameObject origin)
    {
        Instantiate(origin, RootMesh.transform.position, RootMesh.transform.rotation * origin.transform.rotation, RootMesh);
    }
    
    public void Finish()
    {
        if (Destroyed) return;
        Destroyed = true;
        God.Instance.ChangeMoney(DamageMade);
        TextMesh tm = Instantiate(God.Instance.DigitsPrefab, transform.position, transform.rotation, null).GetComponent<TextMesh>();
        tm.text = DamageMade.ToString();
        if (DamageMade < 0)
        {
            tm.color = Color.red;
            Explosion.Play();
            CameraShaker.Instance.Shake();
        }
        RootMesh.gameObject.SetActive(false);
        Destroy(gameObject, 4f);
    }
}
