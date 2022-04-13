using System;
using UnityEngine;


namespace Utils
{
    public class DontPlayAgain : MonoBehaviour
    {
        public static DontPlayAgain Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Screen.fullScreen = false;
            }
        }
    }
}
