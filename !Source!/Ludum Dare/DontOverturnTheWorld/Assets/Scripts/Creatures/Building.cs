using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;


namespace Creatures
{
    public sealed class Building : Creature
    {
        [Header("Settings")]
        public float AirHeight = 2;
        public int Capacity = 3;
        public Side.JobType JobType;
        public float SpawnInterval;
        public int SpawnMaxHumans;

        [Header("Read Only")]
        public List<Human> ReservedChildren = new List<Human>();
        public List<Human> Children = new List<Human>();

        private Animator _animator;
        private float _startTime;

        private void Start()
        {
            Side.Register(this);
            _animator = GetComponent<Animator>();
            _startTime = Time.time;

            StartCoroutine(HumanCoroutine());
        }

        private void Update()
        {
            if (Time.time > _startTime + 2f)
            {
                if (Children.Count == 0)
                {
                    _animator.Play("idle");
                }
                else
                {
                    _animator.Play("work");
                }
            }
            else
            {
                _animator.Play("build");
            }

            if (transform.localPosition.y > AirHeight)
            {
                foreach (Human child in Children)
                {
                    child.MustGoOut = true;
                }
            }
            if (transform.localPosition.y < -15f)
            {
                // foreach (Human child in Children)
                // {
                //     child.Exit();
                // }
                Destroy(gameObject);
                return;
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            Side.Unregister(this);
        }

        private IEnumerator HumanCoroutine()
        {
            if (SpawnInterval <= 0)
                yield break;

            if (JobType != Side.JobType.Sleep)
                yield break;

            int spawnedHumans = 0;
            while (spawnedHumans < SpawnMaxHumans)
            {
                yield return new WaitForSeconds(SpawnInterval);

                spawnedHumans++;
                Human human = Instantiate(PrefabService.Instance.HumanPrefab, Side.transform);
                human.transform.localPosition = transform.localPosition;
            }
        }
    }
}
