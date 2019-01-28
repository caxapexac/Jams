using Client.Scripts.Extensions;
using Client.Scripts.Scriptable;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Client.Scripts.MonoBehaviours
{
    public class EnemyController : MonoBehaviour
    {
        public Transform[] Spawners;

        public EnemyObject[] Enemies;

        public GunObject[] Guns;

        private float _tick;

        private void Start()
        {
            _tick = 0;
        }

        private void Update()
        {
            _tick += Time.deltaTime;
            if (_tick > God.Instance.EnemyDelay)
            {
                _tick = 0;
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()
        {
            Enemy enemy = God.Instance.EnemyPool.Get();
            enemy.transform.position = Spawners[Random.Range(0, Spawners.Length)].transform.position;
            if (Vector2.Distance(enemy.transform.position, God.Instance.Player1.transform.position) < 10
            && Vector2.Distance(enemy.transform.position, God.Instance.Player2.transform.position) < 10)
            {
                enemy.transform.position = Spawners[Random.Range(0, Spawners.Length)].transform.position;
            }
            if (Vector2.Distance(enemy.transform.position, God.Instance.Player1.transform.position) < 10
                && Vector2.Distance(enemy.transform.position, God.Instance.Player2.transform.position) < 10)
            {
                enemy.transform.position = Spawners[Random.Range(0, Spawners.Length)].transform.position;
            }
            enemy.Origin = Enemies[Random.Range(0, Enemies.Length)];
            enemy.Gun = Guns[Random.Range(0, Guns.Length)];
            enemy.Enable();
        }
    }
}