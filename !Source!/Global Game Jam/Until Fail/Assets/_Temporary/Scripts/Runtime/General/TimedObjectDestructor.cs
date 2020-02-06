using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class TimedObjectDestructor : MonoBehaviour
    {
        [SerializeField] private float timeOut = 1.0f;
        [SerializeField] private bool detachChildren = false;


        private void Awake()
        {
            Invoke("DestroyNow", timeOut); // Bad call!
        }


        private void DestroyNow()
        {
            if (detachChildren)
            {
                transform.DetachChildren();
            }
            Destroy(gameObject);
        }
    }
}
