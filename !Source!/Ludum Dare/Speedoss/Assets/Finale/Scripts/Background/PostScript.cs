using System;
using System.Collections.Generic;
using MonoBehs;
using UnityEngine;

namespace Finale.Scripts.Background
{
    public class PostScript : MonoBehaviour
    {
        public Transform PostPrefab;
        public int MaxPosts;
        public float Distance;

        private List<Transform> _posts;
        private int _current;

        private void Start()
        {
            _current = 0;
            _posts = new List<Transform>();
            for (int i = 0; i < MaxPosts; i++)
            {
                _posts.Add(Instantiate(PostPrefab, gameObject.transform).transform);
                _posts[i].localPosition = Distance * i * Vector3.forward;
            }
        }

        private void Update()
        {
            if (_posts[_current % MaxPosts].transform.position.z < God.I.CameraT.transform.position.z - 20)
            {
                _posts[_current % MaxPosts].transform.Translate(MaxPosts * Distance * Vector3.forward);
                _current++;
            }
        }
    }
}