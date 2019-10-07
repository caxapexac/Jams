using UnityEngine;


namespace Client.Scripts.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptable/Enemy")]
    public class EnemyObject : ScriptableObject
    {
        public string Animator;
        public Sprite Texture;
        
        public float Hp;

        public float Speed;
    }
}