using Client.Scripts.Extensions;
using Client.Scripts.Scriptable;
using UnityEngine;


namespace Client.Scripts.MonoBehaviours
{
    public class Bullet : MonoBehaviour
    {
        public BulletObject Origin;
        public AudioSource Audio;
        public Rigidbody2D Rigidbody;
        public CircleCollider2D Collider;
        public SpriteRenderer Sprite;

        [HideInInspector]
        public Vector2 Direction;

        [HideInInspector]
        public bool IsEnemy;

        [HideInInspector]
        public PlayerController Parent;

        private int _hp;
        private float _lifeTime;

        public void Enable()
        {
            gameObject.layer = IsEnemy ? 10 : 8;
            _hp = Origin.Hp;
            _lifeTime = 0;
            if (Origin.Sound)
            {
                //Audio.clip = Origin.Sound;
                //Audio.Play();
            }
            Collider.radius = Origin.Size;
            Sprite.sprite = Origin.Texture;
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            God.Instance.BulletPool.Recycle(this); //todo hp through enemy
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            transform.SetParent(God.Instance.BulletPoolTransform);
        }

        private void Update()
        {
            _lifeTime += Time.deltaTime;
            if (_lifeTime > Origin.LifeTime)
            {
                Disable();
            }
            Vector2 dir = (Vector2)transform.position + Direction * Origin.Speed * Time.deltaTime;
            transform.LookAt2D(dir);
            Rigidbody.MovePosition(dir);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _hp--;
            if (_hp == 0 || other.gameObject.layer == 12)
            {
                Disable();
            }

            //Debug.Log("Bump" + other.gameObject.layer + " " + gameObject.layer, gameObject);
        }
    }
}