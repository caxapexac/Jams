using Creatures;
using UnityEngine;


namespace DefaultNamespace
{
    public class PrefabService : MonoBehaviour
    {
        public static PrefabService Instance;
        public Material SimpleMaterial;
        public Material OutlineMaterial;

        public Human HumanPrefab;

        private void Awake()
        {
            Instance = this;
        }
    }
}
