using System.Collections.Generic;
using MonoBehs;
using UnityEngine;

namespace Player
{
    public class CarPartScript : MonoBehaviour
    {
        public List<Transform> CarParts;
        public int CurrentPart;
        private void Start()
        {
            
        }

        private void Update()
        {
            if (CurrentPart == CarParts.Count)
            {
                God.I.State = GameState.Finish;
            }
            else
            {
                // Todo
            }
        }
    }
}