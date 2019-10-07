using Client.Scripts.Extensions;
using Client.Scripts.Scriptable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Client.Scripts.MonoBehaviours
{
    public class PlayerController : MonoBehaviour
    {
        public int PlayerNumber;
        public GunObject Gun;

        public Image GunImage;
        public Slider FireSlider;
        public TextMeshProUGUI ScoreText;

        public SpriteRenderer BodySprite;
        public SpriteRenderer GunSprite;
        public Animator Animator;

        public Graph Walls;
        public float PathDelay;

        public int Score;

        private float _tick;

        private float _gunTime;
        
        private string _horAxis;
        private string _verAxis;
        private string _fireAxis;

        private float _fireCoolDown;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Vector2 _lookDirection;


        private void Start()
        {
            //Gun = God.Instance.EnemyController.Guns[Random.Range(0, God.Instance.EnemyController.Guns.Length)];
            _horAxis = "P" + PlayerNumber + "Horizontal";
            _verAxis = "P" + PlayerNumber + "Vertical";
            _fireAxis = "P" + PlayerNumber + "Fire";
            Score = 0;
            _gunTime = 0;
            _tick = PathDelay;
            _fireCoolDown = Gun.Delay * God.Instance.FireCoolDown;
            _rigidbody = GetComponent<Rigidbody2D>();
            _direction = Vector2.up;
            FireSlider.maxValue = _fireCoolDown;
            Walls = TileArray.Get(God.Instance.WallsMap, God.Instance.MapSize);
            GunImage.sprite = Gun.DropTexture;
            GunSprite.sprite = Gun.Texture;
        }

        private void Update()
        {
            FireSlider.value = _gunTime;
            _gunTime = Mathf.Max(0, _gunTime - Time.deltaTime);
            if (_gunTime == 0)
            {
                Gun = God.Instance.DefaultGun;
                GunImage.gameObject.SetActive(false);
            }
            ScoreText.text = "SCORE: " + Score;
            _tick += Time.deltaTime;

            if (_tick >= PathDelay)
            {
                _tick = 0;
                Bfs.Search(Walls, new Vector2Int((int)transform.position.x, (int)transform.position.y));
            }
            if (God.Instance.UseMouse)
            {
                //transform.LookAt2D(God.Instance.Camera.ScreenToWorldPoint(Input.mousePosition));
                GunSprite.transform.LookAt2D(God.Instance.Camera.ScreenToWorldPoint(Input.mousePosition));
            }
            Vector2 moving = new Vector2(Input.GetAxis(_horAxis), Input.GetAxis(_verAxis));
            if (moving.sqrMagnitude > 0.01f)
            {
                Animator.speed = 1f;
                _direction = moving.normalized;
                transform.LookAt2D((Vector2)transform.position + _direction);
                _rigidbody.MovePosition((Vector2)transform.position
                    + moving.normalized * God.Instance.PlayerSpeed * Time.deltaTime);
            }
            else
            {
                Animator.speed = 0.1f;
            }
            _fireCoolDown -= Time.deltaTime;
            if (_fireCoolDown < 0 && Input.GetAxis(_fireAxis) > 0)
            {
                _fireCoolDown = Gun.Delay * God.Instance.FireCoolDown;
                Fire(_direction);
            }
        }

        private void Fire(Vector2 direction)
        {
            for (int i = 0; i < Gun.Count; i++)
            {
                Bullet bullet = God.Instance.BulletPool.Get();
                bullet.transform.position = transform.position;
                bullet.Origin = Gun.Bullet;
                if (God.Instance.UseMouse)
                {
                    bullet.Direction = ((Vector2)God.Instance.Camera.ScreenToWorldPoint(Input.mousePosition)
                        - (Vector2)transform.position
                        + (Vector2)Random.insideUnitCircle.normalized * Gun.Accuracy).normalized;
                }
                else
                {
                    bullet.Direction = (direction + Random.insideUnitCircle.normalized * Gun.Accuracy).normalized;
                }
                bullet.IsEnemy = false;
                bullet.Parent = this;
                bullet.Enable();
            }

            //Debug.Log("Fire", gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 13)
            {
                Score += Random.Range(100, 300) * (God.Instance.Level + 1);
                God.Instance.Shaker.ShakeOnce(0.5f, 0.2f, 0.2f, 0.2f);
            }
            else if (other.gameObject.layer == 14)
            {
                Gun = other.GetComponent<GunPickable>().Gun;
                GunImage.sprite = Gun.DropTexture;
                GunSprite.sprite = Gun.Texture;
                GunImage.gameObject.SetActive(true);
                FireSlider.maxValue = Gun.GunTime;
                _gunTime = Gun.GunTime;
            }
            Destroy(other.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == 11)
            {
                Enemy ene = other.transform.GetComponent<Enemy>();
                if (!God.Instance.GodMode)
                {
                    God.Instance.CurrentHp -= ene.Origin.Speed;
                }
                God.Instance.Shaker.ShakeOnce(0.5f, 0.2f, 0.2f, 0.2f);
            }
            else if (other.gameObject.layer == 10)
            {
                Bullet bul = other.transform.GetComponent<Bullet>();
                if (!God.Instance.GodMode)
                {
                    God.Instance.CurrentHp -= bul.Origin.Damage;
                }
            }
        }
    }
}