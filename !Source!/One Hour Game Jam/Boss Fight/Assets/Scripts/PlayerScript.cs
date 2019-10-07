using System.Collections;
using System.Collections.Generic;
using Client.Scripts.Algorithms.Legacy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
    public PrefabPool<BulletScript> Pool;

    public Text LoseText;

    public Slider PlayerHpSlider;

    public BossScript Boss;
    
    public float Speed;
    public float BulletSpeed;
    public float BulletSpeedMultipiler;

    public int Hp;

    public float Delay;
    
    private float _tick = 0;

    private Camera _camera;

    private void Start()
    {
        Pool = new PrefabPool<BulletScript>("PlayerBullet");
        PlayerHpSlider.maxValue = Hp;
        PlayerHpSlider.value = Hp;
        _camera = Camera.main;

    }

    private void Update()
    {
        _tick += Time.deltaTime;
        if (_tick >= Delay || Input.GetKey(KeyCode.Space))
        {
            _tick = 0;
            Delay *= 0.99f;
            Shoot();
        }
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.Translate(move * Time.deltaTime * Speed);
        PlayerHpSlider.value = Hp;
        if (Hp <= 0)
        {
            if (Diff.Instance.Difficulty != 1)
            {
                Diff.Instance.Difficulty--;
            }
            SceneManager.LoadScene(0);
        }
    }

    private void Shoot()
    {
        BulletScript bullet = Pool.Get();
        bullet.Parent = "Player";
        bullet.Speed = BulletSpeed + Random.value * BulletSpeedMultipiler;
        bullet.Rotation = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position + new Vector3(Random.value - 0.5f, Random.value - 0.5f,0);
        bullet.transform.position = transform.position;
        bullet.gameObject.SetActive(true);
        BulletSpeedMultipiler += 0.01f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BulletScript>().Parent == "Enemy")
        {
            other.gameObject.SetActive(false);
            Boss.Pool.Recycle(other.GetComponent<BulletScript>());
            Hp--;
        }
    }

    private void OnBecameInvisible()
    {
        transform.position = Vector3.zero;;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<BossScript>() != null)
        {
            Hp -= Diff.Instance.Difficulty;
        }
    }
}
