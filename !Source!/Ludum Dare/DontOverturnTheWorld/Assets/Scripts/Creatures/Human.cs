using System;
using Common.Behaviors;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


namespace Creatures
{
    public sealed class Human : Creature
    {
        [Header("Settings")]
        public float MotionSpeed = 1f;
        public float StupidTime = 5f;
        public float MinBuildDistance = 1f;

        [Header("Blackboard")]
        public BTState State;
        public Side.JobType CurrentJob = Side.JobType.Rest;
        public bool IsInAir;
        public bool MustGoOut;
        public Building CurrentBuilding;
        public Building TargetBuilding;
        public float TargetPosition;
        public float StartTime;

        [Header("References")]
        public SpriteRenderer[] Renderers;

        private Mass _mass;
        private Draggable _draggable;
        private Collider2D _collider;
        private Animator _animator;
        private float _spawnTime;

        private void Start()
        {
            _mass = GetComponent<Mass>();
            _draggable = GetComponent<Draggable>();
            _collider = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
            _spawnTime = Time.time;
            Side.Register(this);
            State = new BTState(HumanBrain.RootNode);
        }

        private void Update()
        {
            IsInAir = Mathf.Abs(transform.localPosition.x) > Earth.Radius || Mathf.Abs(transform.localPosition.y) > 0.1f;
            if (_draggable.Grabbed)
            {
                _animator.Play("grab");
            }
            else if (CurrentBuilding != null)
            {
                _animator.Play("stay");
            }
            else if (IsInAir)
            {
                _animator.Play("fall");
            }
            else
            {
                _animator.Play("walk");
            }
            if (transform.localPosition.y < -15f)
            {
                Die();
                return;
            }
            if (Time.time > _spawnTime + StupidTime)
                HumanBrain.UpdateBrain(this);
        }

        private void OnDestroy()
        {
            Side.Unregister(this);
        }

        public void MoveTowards(float point)
        {
            Vector3 position = transform.localPosition;
            position.x = Mathf.MoveTowards(transform.localPosition.x, point, MotionSpeed * Time.deltaTime);
            Move(position);
        }

        public void Move(Vector3 position)
        {
            // TODO animation
            if (position.x > transform.localPosition.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            transform.localPosition = position;
        }

        public void Reserve(Building building)
        {
            if (TargetBuilding != null)
            {
                TargetBuilding.ReservedChildren.Remove(this);
                TargetBuilding = null;
            }
            TargetBuilding = building;
            TargetBuilding.ReservedChildren.Add(this);
        }

        public void Enter(Building building)
        {
            if (CurrentBuilding != null)
                Exit();

            CurrentBuilding = building;
            building.Children.Add(this);
            transform.SetParent(CurrentBuilding.transform, true);
            _collider.enabled = false;
            foreach (SpriteRenderer spriteRenderer in Renderers)
            {
                spriteRenderer.enabled = false;
            }
        }

        public void Exit()
        {
            if (Side == null)
                return;

            if (MustGoOut)
                _mass.WannaBonk = true;

            MustGoOut = false;
            if (CurrentBuilding != null)
            {
                CurrentBuilding.ReservedChildren.Remove(this);
                CurrentBuilding.Children.Remove(this);
            }
            CurrentBuilding = null;
            transform.SetParent(Side.transform);
            if (_collider != null)
                _collider.enabled = true;
            foreach (SpriteRenderer spriteRenderer in Renderers)
            {
                if (spriteRenderer != null)
                    spriteRenderer.enabled = true;
            }
        }

        public void Die()
        {
            // TODO exit building
            Exit();
            Destroy(gameObject);
        }
    }
}
