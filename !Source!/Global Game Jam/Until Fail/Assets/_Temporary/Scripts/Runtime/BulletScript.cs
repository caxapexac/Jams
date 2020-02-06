using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public ParticleSystem ImpactPrefab; //Эффект взрыва при столкновении, префаб
    public TrailRenderer Trail; //Ссылка на хвост

    [HideInInspector]
    public GunScript Gun;
    private float range;

    public void Setup(GunScript gun)
    {
        //peed = gun.
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        // Бум
    }
}
