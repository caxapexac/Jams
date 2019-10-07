using System.Collections;
using System.Collections.Generic;
using Client.Scripts.Algorithms.Legacy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BossScript : MonoBehaviour
{
    public PrefabPool<BulletScript> Pool;
    public PlayerScript Player;
    public Slider EnemyHpSlider;
    public float Speed;
    public float BulletSpeed;
    public float BulletSpeedMultipiler;

    public Text WinText;
    
    public int Hp;

    public float Delay;
    
    private float _tick = 0;

    private void Start()
    {
        Pool = new PrefabPool<BulletScript>("BossBullet");
        EnemyHpSlider.maxValue = Hp;
        EnemyHpSlider.value = Hp;
    }


    private void Update()
    {
        _tick += Time.deltaTime;
        if (_tick >= Delay)
        {
            _tick = 0;
            Shoot();
            ShootRandom();
            ShootRandom();

        }
        transform.position = Vector2.Lerp(transform.position, Player.transform.position, Time.deltaTime * Speed);
        EnemyHpSlider.value = Hp;
        if (Hp <= 0)
        {
            Diff.Instance.Difficulty++;
            SceneManager.LoadScene(0);
        }
    }


    private void ShootRandom()
    {
        BulletScript bullet = Pool.Get();
        bullet.Parent = "Enemy";
        bullet.Speed = BulletSpeed + Random.value * BulletSpeedMultipiler;
        bullet.Rotation = new Vector2(Random.value - 0.5f, Random.value - 0.5f);
        bullet.transform.position = transform.position;
        bullet.gameObject.SetActive(true);
        BulletSpeedMultipiler += 0.01f;
    }

    private void Shoot()
    {
        BulletScript bullet = Pool.Get();
        bullet.Parent = "Enemy";
        bullet.Speed = BulletSpeed + Random.value * BulletSpeedMultipiler;
        bullet.Rotation = (Player.transform.position - transform.position).normalized;
        bullet.transform.position = transform.position;
        bullet.gameObject.SetActive(true);
        BulletSpeedMultipiler += 0.01f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BulletScript>().Parent == "Player")
        {
            other.gameObject.SetActive(false);
            Player.Pool.Recycle(other.GetComponent<BulletScript>());
            Hp--;
        }
    }
}
