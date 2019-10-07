using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;
using Random = UnityEngine.Random;
using Scripts;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InputManager : MonoBehaviour
{

    public Sprite Rocket;
    public Sprite Lemon;
    
    public string Console;
    public int Hp;
    public float Distance;
    public float Speed;
    public float EnemySpeed;
    public float Tick;
    public float Delay;
    public float Chance;
    public float Radius;
    public Text Text;
    public Text HpText;

    public Transform Player;
    public Vector3 FinalPos;

    public CameraShaker Shaker;
    public Camera Camera;

    public List<EnemyScript> Enemies;
    

    public GameObject EnemyPrefab;

    public string[] Commands = new[] {"Q", "W", "E", "A", "S", "D", "Z", "X", "C"};
    void Start()
    {
        Camera = Camera.main;
        Console = "";
        FinalPos = Player.position;
        Enemies = new List<EnemyScript>();
    }

    void Update()
    {
        Camera.transform.position = Vector3.Lerp(Camera.transform.position, Player.position + Vector3.forward * -5, Time.deltaTime * Speed);
        Tick += Time.deltaTime;
        if (Tick >= Delay)
        {
            Tick = 0;
            Delay = Mathf.Max(Delay - 0.1f, 0.3f);
            Spawn();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            
            if (Console == "Q")
            {
                FinalPos.x -= Distance;
                FinalPos.y += Distance;
            }
            else if (Console == "W")
            {
                FinalPos.y += Distance;
            }
            else if (Console == "E")
            {
                FinalPos.x += Distance;
                FinalPos.y += Distance;
            }
            else if (Console == "A")
            {
                FinalPos.x -= Distance;
            }
            else if (Console == "S")
            {
                ChangeSprite();
            }
            else if (Console == "D")
            {
                FinalPos.x += Distance;
            }
            else if (Console == "Z")
            {
                FinalPos.x -= Distance;
                FinalPos.y -= Distance;
            }
            else if (Console == "X")
            {
                FinalPos.y -= Distance;
            }
            else if (Console == "C")
            {
                FinalPos.x += Distance;
                FinalPos.y -= Distance;
            }
            else
            {
                Hp -= 1;
            }
            Console = "";
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Console = Console + "Q";
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Console = Console + "W";
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Console = Console + "E";
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Console = Console + "A";
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Console = Console + "S";
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Console = Console + "D";
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            Console = Console + "Z";
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Console = Console + "X";
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Console = Console + "C";
        }
        else if (Input.anyKeyDown)
        {
            Console = Console + " STACKOVERFLOWEXCEPTION ";
        }
        Player.position = Vector3.Lerp(Player.position, FinalPos, Speed * Time.deltaTime);
        Player.LookAt2D(FinalPos);
        Text.text = Console;
        HpText.text = "HP: " + Hp;
        if (Hp <= 0)
        {
            StartCoroutine(LoseMsg());
        }
    }

    public IEnumerator LoseMsg()
    {
        //Shaker.ShakeOnce(1, 5, 5, 5);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
    }

    private void Spawn()
    {
        Vector3 pos = Player.position + (Vector3)(Random.insideUnitCircle.normalized) * Radius;
        GameObject go = Instantiate(EnemyPrefab, pos, Quaternion.identity);
        go.transform.LookAt2D(Player.position);
        go.GetComponent<EnemyScript>().Manager = this;
        Enemies.Add(go.GetComponent<EnemyScript>());
    }

    private void ChangeSprite()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Random.value > Chance)
            {
                Enemies[i].Change();
            }
        }
    }

   
}
