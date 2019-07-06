using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class God : MonoBehaviour
{
    public static float Drunk = 0;
    public static float Want = 1;
    public Color NextColor;
    public Color CurColor;
    public float Delay;
    public Camera Camera;
    public Slider Slider;
    public GameObject Alkohol;
    public List<Transform> Anchors;
    public Text WinT;
    public Text LoseT;
    public GameObject Hide;
    public Slider Diff;

    private float curtime;
    // Start is called before the first frame update
    private void Awake()
    {
        Time.timeScale = 0;
    }

    private void Start()
    {
        Camera = Camera.main;
        curtime = Time.time;
        CurColor = Camera.backgroundColor;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        Want = Diff.value;
        Slider.maxValue = Want;
        Hide.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        Slider.value = Drunk;
        if (Time.time > curtime + Delay)
        {
            curtime = Time.time;
            CurColor = Camera.backgroundColor;
            NextColor = Random.ColorHSV(0, 1, 0.5f, 1, 0.5f, 1);
        }
        Camera.backgroundColor = Color.Lerp(CurColor, NextColor, (Time.time - curtime) / Delay);

        if (Drunk < 0)
        {
            LoseT.gameObject.SetActive(true);
            Invoke(nameof(Restart), 3);
        }
        if (Drunk > Want)
        {
            WinT.gameObject.SetActive(true);
            Invoke(nameof(Restart), 3);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Instantiate(Alkohol, Anchors[0]);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Instantiate(Alkohol, Anchors[1]);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Instantiate(Alkohol, Anchors[2]);
        }
        if (Input.GetKey(KeyCode.F))
        {
            Instantiate(Alkohol, Anchors[3]);
        }
        if (Input.GetKey(KeyCode.G))
        {
            Instantiate(Alkohol, Anchors[4]);
        }
        if (Input.GetKey(KeyCode.H))
        {
            Instantiate(Alkohol, Anchors[5]);
        }
        if (Input.GetKey(KeyCode.J))
        {
            Instantiate(Alkohol, Anchors[6]);
        }
        if (Input.GetKey(KeyCode.K))
        {
            Instantiate(Alkohol, Anchors[7]);
        }
        if (Input.GetKey(KeyCode.L))
        {
            Instantiate(Alkohol, Anchors[8]);
        }
    }

    public void Restart()
    {
        LoseT.gameObject.SetActive(false);
        WinT.gameObject.SetActive(false);
        Drunk = 0;
        SceneManager.LoadScene(0);
    }
}
