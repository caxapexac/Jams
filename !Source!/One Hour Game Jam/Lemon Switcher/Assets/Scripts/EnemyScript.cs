using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public InputManager Manager;
    

    public bool IsRocket = true;
        
    private void Start()
    {
        transform.LookAt2D(Manager.Player.position);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * Manager.EnemySpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("COL");
        if (other.gameObject.GetComponent<EnemyScript>() == null)
        {
            if (IsRocket)
            {
                Manager.Hp -= 1;
                //Manager.Shaker.ShakeOnce(1, 1, 1, 1);
            }
            else
            {
                Manager.Hp += 1;
            }
			Manager.Enemies.Remove(this);
            Destroy(gameObject);
        }
    }

    public void Change()
    {
        IsRocket = !IsRocket;
        if (IsRocket)
        {
            GetComponent<SpriteRenderer>().sprite = Manager.Rocket;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Manager.Lemon;
        }
    }
}