using Client.Scripts.MonoBehaviours;
using UnityEngine;


namespace Client.Scripts.Scriptable
{
    [CreateAssetMenu(menuName = "Scriptable/Gun")]
    public class GunObject : ScriptableObject
    {
        public string Name;
        
        public float GunTime;
        
        public Sprite Texture;
        
        public Sprite DropTexture;
        
        public Sprite EnemyTexture;

        public int Count;
        
        public float Delay;

        public float Accuracy;

        public BulletObject Bullet;

    }
}