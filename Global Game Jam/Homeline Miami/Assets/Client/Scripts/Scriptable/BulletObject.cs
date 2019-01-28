using Client.Scripts.MonoBehaviours;
using UnityEngine;


namespace Client.Scripts.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptable/Bullet")]
    public class BulletObject : ScriptableObject
    {
        public Sprite Texture;
        
        public AudioClip Sound;
        
        public float Size;

        public float LifeTime;
        
        public int Hp;
        
        public float Damage;
        
        public float Speed;
    }
}