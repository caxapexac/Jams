using Client.Scripts.Extensions;
using Client.Scripts.Scriptable;
using UnityEngine;


namespace Client.Scripts.MonoBehaviours
{
    public class Enemy : MonoBehaviour
    {
        public EnemyObject Origin;

        public GunObject Gun;

        private float _hp;
        private float _coolDown;
        private PlayerController _target;
        private Vector2Int _path;
        private Vector2 _direction;
        private float _lastDistance;
        public Rigidbody2D Rigidbody;
        public CircleCollider2D Collider;
        public SpriteRenderer BodySprite;
        public SpriteRenderer GunSprite;
        public Animator Animator;

        public void Enable()
        {
            _target = Random.value > 0.5f || God.Instance.PlayerCount == 1
                ? God.Instance.Player1
                : God.Instance.Player2;
            Vector2Int me = new Vector2Int((int)transform.position.x, (int)transform.position.y);

            //Debug.Log(me.x + " " + me.y);
            foreach (Vector2Int path in _target.Walls.Neighbors(me))
            {
                if (_target.Walls.Weights[path.x, path.y] < _target.Walls.Weights[me.x, me.y])
                {
                    _path = path;
                    _direction = (_path + Vector2.one * 0.32f - (Vector2)transform.position).normalized;
                    _lastDistance = 1000;
                    break;
                }
            }
            _hp = Origin.Hp;
            _coolDown = Gun.Delay;
            BodySprite.sprite = Origin.Texture;
            GunSprite.sprite = Gun.Texture;
            RuntimeAnimatorController con = Resources.Load<RuntimeAnimatorController>(Origin.Animator);
            Animator.runtimeAnimatorController = con;
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            if (Random.value > 0.5f)
            {
                GameObject go = Instantiate(God.Instance.GunPrefab, transform.position, Random.rotation);
                go.GetComponent<GunPickable>().Gun = Gun;
            }
            God.Instance.EnemyPool.Recycle(this); //todo hp through enemy
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            transform.SetParent(God.Instance.EnemyPoolTransform);
        }

        private void Update()
        {
            float newDist = Vector2.Distance(transform.position, _path);
            if ((int)transform.position.x != _path.x || (int)transform.position.y != _path.y)
            {
                _lastDistance = newDist;
                Rigidbody.MovePosition((Vector2)transform.position + _direction * Origin.Speed * Time.deltaTime);
                BodySprite.transform.LookAt2D(_target.transform);
            }
            else
            {
                
                foreach (Vector2Int path in _target.Walls.Neighbors(_path))
                {
                    if (_target.Walls.Weights[path.x, path.y] < _target.Walls.Weights[_path.x, _path.y])
                    {
                        _path = path;
                    }
                }
                _direction =
                    (_path
                        + Vector2.one * 0.32f
                        + Random.insideUnitCircle * 0.16f
                        - (Vector2)transform.position).normalized;
                _lastDistance = 1000;
            }
            _coolDown -= Time.deltaTime;
            if (_coolDown <= 0)
            {
                _coolDown = Gun.Delay;
                if (Random.value < God.Instance.Difficulty)
                {
                    Fire(_direction);
                }
            }
        }

        private void Fire(Vector2 direction)
        {
            for (int i = 0; i < Gun.Count; i++)
            {
                Bullet bullet = God.Instance.BulletPool.Get();
                bullet.transform.position = transform.position;
                bullet.Origin = Gun.Bullet;
                bullet.Direction = (direction + Random.insideUnitCircle.normalized * Gun.Accuracy).normalized;
                bullet.IsEnemy = true;
                bullet.Parent = null;
                bullet.Enable();
            }

            //Debug.Log("Fire", gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == 8)
            {
                Bullet bul = other.transform.GetComponent<Bullet>();
                _hp -= bul.Origin.Damage;
                if (!God.Instance.NoBlood)
                {
                    Instantiate(God.Instance.Bleed[Random.Range(0, God.Instance.Bleed.Length)],
                        transform.position + Vector3.one * (Random.value - 0.5f), Quaternion.Euler(0, 0, Random.Range(0, 360)));
                }
                if (_hp <= 0)
                {
                    God.Instance.Shaker.ShakeOnce(0.5f, 0.1f, 0.3f, 0.3f);
                    bul.Parent.Score += (int)Origin.Hp;
                    Disable();
                }
            }
            else if (other.gameObject.layer == 12)
            {
                _target = Random.value > 0.5f || God.Instance.PlayerCount == 1
                    ? God.Instance.Player1
                    : God.Instance.Player2;
                Vector2Int me = new Vector2Int((int)transform.position.x, (int)transform.position.y);

                //Debug.Log(me.x + " " + me.y);
                foreach (Vector2Int path in _target.Walls.Neighbors(me))
                {
                    if (_target.Walls.Weights[path.x, path.y] < _target.Walls.Weights[me.x, me.y])
                    {
                        _path = path;
                        _direction = (_path + Vector2.one * 0.32f - (Vector2)transform.position).normalized;
                        _lastDistance = 1000;
                        break;
                    }
                }
            }
        }
    }
}