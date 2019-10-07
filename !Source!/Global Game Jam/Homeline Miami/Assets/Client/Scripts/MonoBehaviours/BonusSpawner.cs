using Client.Scripts.Scriptable;
using UnityEngine;


namespace Client.Scripts.MonoBehaviours
{
    public class BonusSpawner : MonoBehaviour
    {
        public Sprite[] Bonuses;

        public GameObject BonusPrefab;

        public float Delay;

        public float Tick;
        
        private void Update()
        {
            Tick += Time.deltaTime;
            if (Tick >= Delay)
            {
                Tick = 0;
                Spawn();
            }
        }

        public void Spawn()
        {
            int x;
            int y;
            do
            {
                x = Random.Range(0, God.Instance.MapSize);
                y = Random.Range(0, God.Instance.MapSize);
            } while (God.Instance.Player1.Walls.Edges[x, y] == false);
            GameObject go = Instantiate(BonusPrefab, new Vector3(x, y), Quaternion.identity);
            go.GetComponent<SpriteRenderer>().sprite = Bonuses[Random.Range(0, Bonuses.Length)];
        }
    }
}