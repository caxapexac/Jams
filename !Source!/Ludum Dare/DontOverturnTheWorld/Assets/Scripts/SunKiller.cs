using System;
using System.Collections;
using System.Collections.Generic;
using Creatures;
using UnityEngine;


public class SunKiller : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Human human))
        {
            _audioSource.Play();
            human.Die();
        }
    }
}
