using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class MoveScript : MonoBehaviour
{
    public bool IsStand = false;
    public CameraShakeInstance Instance;
    public Manager Manager;
    public float Gravity;
    public Sprite Present;
    public TextMeshProUGUI Text;
    public int Score = 0;
    
    void Start()
    {
        Manager = FindObjectOfType<Manager>();
        //Instance = new CameraShakeInstance(1, 1, 0.3f, 0.3f);
    }

    void Update()
    {
        
        if (IsStand && (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f))
        {
            Physics2D.gravity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Gravity;
        }


    }


    void OnCollisionEnter2D(Collision2D other)
    {
        IsStand = true;
        Manager.Shaker.ShakeOnce(2, 5, 0.6f, 0.6f);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        IsStand = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Score++;
        Text.text = "Presents dropped: " + Score;
        other.GetComponent<SpriteRenderer>().sprite = Present;
        Destroy(other.gameObject, 1f);
    }
}
