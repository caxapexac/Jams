using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class God : MonoBehaviour
{
    public enum StateType
    {
        Menu,
        Playing,
        Lose,
        Won
    }
    
    [HideInInspector] public static God Instance;

    public StateType State;

    public GameObject DigitsPrefab;
    public GameObject MainMenu;
    public GameObject WinMenu;
    public GameObject LoseMenu;
    public Camera CurrentCamera;
    public AudioSource WinSound;
    public AudioSource LoseSound;
    public TextMeshProUGUI TimerText;
    public Animator TimerAnimator;
    public TextMeshProUGUI MoneyText;
    public Animator MoneyAnimator;
    public AudioSource MoneyPlusSound;
    public AudioSource MoneyMinusSound;
    public Animator PanelAnimator;
    public AudioSource GlobalMusicMenu;
    public AudioSource GlobalMusicGameplay;

    public int Money;
    public float SecondsToWin = 60;
    public float RestartDelay = 4f;
    public GameObject CurrentTile;
    public int Durability;
    public float TextSpeed;
    public float TextTime;

    //Level dependent
    public GameObject CameraStartPos;
    public GameObject CarLogic;
    public List<GameObject> GovermentCars;
    public List<GameObject> CivilianCars;
    public AnimationCurve DifficultyCurve;
    public float TimeDifficultyMultipiler;

    private List<SpawnerPoint> spawners;
    private List<CrossExplosionBoom> booms;
    private float lastTime;
    private float startTime;

    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        spawners = new List<SpawnerPoint>(FindObjectsOfType<SpawnerPoint>());
        booms = new List<CrossExplosionBoom>(FindObjectsOfType<CrossExplosionBoom>());
        lastTime = Time.time;
        GlobalMusicMenu.Play();
        MoneyText.text = Money.ToString();
        MoneyText.text = "Money: $" + Money.ToString();
        MoneyText.color = Color.Lerp(Color.red, Color.white, Mathf.Clamp01(Money / 100.0f));
    }

    void Update()
    {
        switch (State)
        {
            case StateType.Menu:
                //Курим сидим
                break;
            case StateType.Playing:
                string t = "Time left: " + (60f - Time.time + startTime).ToString("00");
                if (TimerText.text != t)
                {
                    TimerAnimator.SetTrigger("Time");
                    TimerText.text = t;
                }
                float delay = Time.time - startTime;
                if (delay >= SecondsToWin)
                {
                    lastTime = Time.time;
                    WinSound.Play();
                    State = StateType.Won;
                }
                float delay2 = Time.time - lastTime;
                if (delay2 > TimeDifficultyMultipiler * DifficultyCurve.Evaluate(delay / 60))
                {
                    Debug.Log(DifficultyCurve.Evaluate(delay / 60).ToString());
                    lastTime = Time.time;
                    SpawnCar();
                }
                break;
            case StateType.Lose:
                //Перезагрузить
                CameraShaker.Instance.Shake();
                LoseMenu.SetActive(true);
                break;
            case StateType.Won:
                CameraShaker.Instance.Shake();
                WinMenu.SetActive(true);
                //Поздравить, перейти на следующий уровень
                break;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Restart();
        }
    }

    public void Lose()
    {
        lastTime = Time.time;
        State = StateType.Lose;
        LoseSound.Play();
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SpawnCar()
    {
        SpawnerPoint point = spawners[(int)Random.Range(0, spawners.Count)];
        GameObject go = Instantiate(CarLogic, point.transform.position, point.transform.rotation);
        go.transform.Rotate(Vector3.down * 90);
        CarComponent car = go.GetComponent<CarComponent>();
        car.ChangeWaypoint(point.Waypoint);
        
        switch (point.Type)
        {
            case SpawnerPoint.SpawnerType.Goverment:
                go.GetComponent<CarComponent>().SetupMesh(GovermentCars[(int)Random.Range(0, GovermentCars.Count)]);
                break;
            case SpawnerPoint.SpawnerType.Cilivian:
                go.GetComponent<CarComponent>().SetupMesh(CivilianCars[(int)Random.Range(0, CivilianCars.Count)]);
                break;
        }
    }

    public void ChangeMoney(int change, bool canlose = false)
    {
        Money += change;
        if (Money <= 0 && canlose) Lose();
        if (Money < 0) Lose();
        MoneyAnimator.SetTrigger("Money");
        MoneyText.text = "Money: $" + Money.ToString();
        MoneyText.color = Color.Lerp(Color.red, Color.white, Mathf.Clamp01(Money / 100.0f));
        if (change > 0) MoneyPlusSound.Play();
        else MoneyMinusSound.Play();
    }

    public void StartLevel()
    {
        MainMenu.SetActive(false);
        GlobalMusicMenu.Stop();
        GlobalMusicGameplay.Play();
        PanelAnimator.SetTrigger("Panel");
        State = StateType.Playing;
        Transform cam = CurrentCamera.transform;
        cam.SetParent(CameraStartPos.transform);
        cam.position = CameraStartPos.transform.position;
        cam.rotation = CameraStartPos.transform.rotation;
        foreach (CrossExplosionBoom cb in booms)
        {
            cb.Boom();
        }
        startTime = Time.time;
    }

    public void ChangeTile(GameObject tile)
    {
        CurrentTile = tile;
    }
    
    public void ChangeTileDurability(int count)
    {
        Durability = count;
    }
}
