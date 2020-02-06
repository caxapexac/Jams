using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public int FineAmount;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        CarComponent car = other.GetComponentInParent<CarComponent>();
        if (car != null)
        {
            car.DamageMade = -FineAmount;
            if (car.Destroyed) return;
            car.Destroyed = true;
            God.Instance.ChangeMoney(-5, true);
            TextMesh tm = Instantiate(God.Instance.DigitsPrefab, transform.position, transform.rotation, null).GetComponent<TextMesh>();
            tm.text = car.DamageMade.ToString();
            tm.color = Color.red;
            car.Explosion.Play();
            CameraShaker.Instance.Shake();
            car.RootMesh.gameObject.SetActive(false);
            Destroy(gameObject, 4f);
            Debug.Log("LFDFKL");
        }
    }
}
