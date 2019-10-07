using System.Collections;
using System.Collections.Generic;
using Client.Scripts.Scriptable;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Spawner : MonoBehaviour
{
    public Sprite[] Guns;

    public float Delay;

    public float Tick;

    public SpriteRenderer Prefab;

    public float Min;
    public float Max;

    public SettingsObject Settings;

    public Camera MyCamera;

    public Toggle NoBloodModeToggle;
    public Toggle GodModeToggle;
    public Toggle IsMouseToggle;

    private void Start()
    {
        NoBloodModeToggle.isOn = Settings.NoBlood;
        GodModeToggle.isOn = Settings.GodMode;
        IsMouseToggle.isOn = Settings.IsMouse;
        Physics2D.gravity = new Vector2(0, -1);
        MyCamera = Camera.main;
        Max = MyCamera.ViewportToWorldPoint(new Vector3(0, 1)).x;
        Min = MyCamera.ViewportToWorldPoint(new Vector3(0, 1)).x;
    }

    private void Update()
    {
        Tick += Time.deltaTime;
        if (Tick >= Delay)
        {
            Tick = 0;
            Spawn();
        }
        MyCamera.backgroundColor = Color.Lerp(MyCamera.backgroundColor, Random.ColorHSV(), Time.deltaTime);
    }

    private void Spawn()
    {
        SpriteRenderer sr = Instantiate(Prefab, new Vector3(Random.Range(-9, 9), 8),
            Quaternion.Euler(0, 0, Random.Range(0, 360)));
        sr.sprite = Guns[Random.Range(0, Guns.Length)];
        sr.GetComponent<Rigidbody2D>().mass = Random.value / 2;
        sr.transform.localScale = Vector3.one * Random.value * 5;
        Destroy(sr.gameObject, 20f);
    }

    public void Single()
    {
        Settings.PlayerCount = 1;
        SceneManager.LoadScene(1);
    }

    public void Coop()
    {
        Settings.PlayerCount = 2;
        SceneManager.LoadScene(1);
    }

    public void NoBlood(bool value)
    {
        Settings.NoBlood = value;
    }
    
    public void GodMode(bool value)
    {
        Settings.GodMode = value;
    }

    public void UseMouse(bool value)
    {
        Settings.IsMouse = value;
    }
    
}