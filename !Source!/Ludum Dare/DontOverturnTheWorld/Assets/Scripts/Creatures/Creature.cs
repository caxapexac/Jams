using System;
using UnityEngine;


namespace Creatures
{
    public class Creature : MonoBehaviour
    {
        public World World;
        public Earth Earth;
        public Side Side;

        protected virtual void Awake()
        {
            World = GetComponentInParent<World>();
            Earth = GetComponentInParent<Earth>();
            Side = GetComponentInParent<Side>();
        }
    }
}
