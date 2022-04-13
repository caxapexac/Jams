using System;
using System.Collections;
using Creatures;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


namespace DefaultNamespace
{
    public class StarAlpha : MonoBehaviour
    {
        public Transform Sun;
        public float Falloff = 1;
        public float SizeRandomizaion;
        public float SizeSpeed;
        public bool IsFall = false;
        public bool StartFalling = false;
        public float BeforeStartTime = 5f;
        public SpriteRenderer SpriteRenderer;
        private float Offset;
        private Stars _stars;
        private float _startTime;

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            // transform.localScale *= Random.Range(1 - SizeRandomizaion, 1 + SizeRandomizaion);
            Offset = Random.Range(0, 1000);
            _stars = FindObjectOfType<Stars>();
            _startTime = Time.time;
        }

        private void Update()
        {
            if (IsFall)
            {
                // SpriteRenderer.material.color = new Color(1, 1, 1, 0.5f);
                return;
            }
            float dot = -Vector3.Dot(Sun.transform.position.normalized, transform.position.normalized);
            SpriteRenderer.material.color = new Color(1, 1, 1, Mathf.Clamp(dot * Falloff, 0, 0.5f));
            transform.localScale = Vector3.one * (1 + Mathf.Sin(Offset + Time.time * SizeSpeed) * SizeRandomizaion);
            if (Time.time > _startTime + BeforeStartTime && !IsFall && StartFalling && SpriteRenderer.material.color.a > 0.2f)
            {
                Fall(new Vector3(Random.Range(-FindObjectOfType<Earth>().Radius / 2, FindObjectOfType<Earth>().Radius / 2), 0, 0));
            }
        }

        public void Fall(Vector3 localPosition)
        {
            IsFall = true;
            Color orange = Color.Lerp(Color.red, Color.yellow, 0.5f);
            orange.a = 0.5f;
            SpriteRenderer.material
                .DOColor(orange, 3f)
                .SetEase(Ease.InQuint)
                .OnComplete(() => SpriteRenderer.material.color = new Color(1, 1, 1, 0.3f));
            transform
                .DOLocalRotate(new Vector3(0, 0, 720), 3f)
                .SetEase(Ease.InQuint);
            transform
                .DOLocalMove(localPosition, 3f)
                .SetEase(Ease.InQuint)
                .OnComplete(() =>
                {
                    Mass mass = gameObject.AddComponent<Mass>();
                    mass.Weight = 2;
                    _stars.GetComponent<AudioSource>().Play();
                });

            StartCoroutine(SpawnHuman());
        }

        private IEnumerator SpawnHuman()
        {
            yield return new WaitForSeconds(3f);

            Human human = Instantiate(PrefabService.Instance.HumanPrefab, transform.parent);
            human.transform.localPosition = transform.localPosition;
        }

        private void OnDestroy()
        {
            SpriteRenderer.material.DOKill();
            transform.DOKill();
        }
    }
}
