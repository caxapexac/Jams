using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Editor")]
    public int MaxHp = 100;
    public List<GunScript> Guns;
    public UiScript Ui;

    [Space]
    [Header("Runtime")]
    public int currentHp;
    public int currentGun;


    private void Start()
    {
        currentHp = MaxHp;
        currentGun = 0;

        if (Guns.Count > 0)
        {
            Guns[currentGun].gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        //ЛКМ выстрел оружия
        if (Input.GetAxis("Fire1") > 0)
        {
            Guns[currentGun].Shoot();
        }

        //ПКМ функция оружия
        if (Input.GetAxis("Fire2") > 0)
        {
            Guns[currentGun].Function();
        }

        //Сменить оружие
        if (Guns.Count > 1 && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            //Убираем предыдущее оружие
            Guns[currentGun].gameObject.SetActive(false);

            //Ищем направление колеса
            if (Input.GetAxis("Mouse ScrollWheel") > 0) //Следующее
            {
                currentGun += 1;
                if (currentGun >= Guns.Count) currentGun = 0; //Переполнение
            }
            else //Предыдущее
            {
                currentGun -= 1;
                if (currentGun < 0) currentGun = Guns.Count - 1; //Переполнение
            }

            //Ставим новое оружие
            Guns[currentGun].gameObject.SetActive(true);
        }

        //Использовать
        if (Input.GetButtonDown("Submit"))
        {
            // TODO
        }

        //Обновляем информацию об игроке
        DrawGui();
    }

    public void Damage(int count)
    {
        currentHp -= count;
        // Play hurt sound
        if (currentHp <= 0)
        {
            //Die
            Debug.Log("You dead");
        }
    }

    public void DrawGui()
    {
        Ui.DebugText.text = "";
        Ui.HpText.text = $"HP: {currentHp}/{MaxHp}";
        Ui.WeaponNameText.text = $"Name: {Guns[currentGun].Name}";
        Ui.WeaponAmmoText.text = $"Ammo: {Guns[currentGun].currentAmmo}";
        Ui.WeaponDamageText.text = $"Damage: {Guns[currentGun].Damage}";
        Ui.WeaponRangeText.text = $"Range: {Guns[currentGun].Range}";
        Ui.WeaponDelayText.text = $"Delay: {Guns[currentGun].Delay}";
        //Ui.PlayerImage
    }
}
