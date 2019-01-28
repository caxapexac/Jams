using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public MoveScript Player;
    public CameraShaker Shaker;
    public Transform Min;
    public Transform Max;
    public GameObject Valve;
    
    public float Delay;
    //public float Difficulty;
    public float Multipiler;
    public float Tick;
    
    void Start()
    {
        Player = FindObjectOfType<MoveScript>();
        Shaker = GetComponent<CameraShaker>();
        
    }

    void Update()
    {
        Tick += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F))
        {
            Shaker.ShakeOnce(5, 8, 0.6f, 0.6f);
            Multipiler *= 2;
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Tick >= Delay)
        {
            Spawn();
            Tick = 0;
        }

        Delay = Mathf.Clamp(Delay - Time.deltaTime * Multipiler, 0.1f, 10);
    }
    
    
    private void Spawn()
    {
        Vector3 pos = new Vector3(Random.Range(Min.position.x, Max.position.x),
            Random.Range(Min.position.y, Max.position.y), 2);
        GameObject go = Instantiate(Valve, pos, Quaternion.identity);
        go.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
    }
}
