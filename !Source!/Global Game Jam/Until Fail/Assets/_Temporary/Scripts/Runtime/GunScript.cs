using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [Header("Editor")]
    public string Name = "Unknown";
    public int MaxAmmo = 500;
    public float Damage = 10f;
    public float Range = 100f;
    public float Delay; //Задержка между выстрелами по зажатию кнопки выстрела
    public AudioSource ShootSound;
    public AudioSource PeekSound; //Звук при подборе
    public ParticleSystem ShootEffect;
    public GameObject BulletPrefab; //Какими пулями стреляем

    [Space]
    [Header("Runtime")]
    public int currentAmmo;
    public float timeAfterLastShoot;
    private Camera currentCamera;

    void OnEnable()
    {
        PeekSound.Play();
    }

    void Start()
    {
        timeAfterLastShoot = Delay;
        currentAmmo = 20;
        currentCamera = Camera.main;
    }

    void Update()
    {
        timeAfterLastShoot += Time.deltaTime;
    }

    public void Shoot()
    {

        if (timeAfterLastShoot > Delay)
        {
            //Обновляем задержку перед выстрелом
            timeAfterLastShoot = 0;

            //Проверяем магазин
            if (currentAmmo > 0)
            {
                currentAmmo -= 1;
                ShootSound.Play();
                ShootEffect.Play();
                // Лучом из глаз проверяем попадание
                if (Physics.Raycast(currentCamera.transform.position, currentCamera.transform.forward, out RaycastHit hit, Range))
                {
                    // Todo проверка попадания по врагу
                    // Instantiate(ImpactPrefab, hit.point, Quaternion.identity);
                }
            }
            else
            {
                //Todo Звук холостого нажатия на спусковой крючок
                Debug.Log($"No ammo on {Name} gun");
            }
        }
    }

    public void Function() // Уникальная функция оружия
    {
        //Временно===
        currentAmmo += 10;
        Debug.Log("Cheater!");
        //===


        // Sniper - Zoom
        // Machine gun - Slightly zoom
        // Rifle - Grenade
        // None
    }
}
