using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;


public sealed class Stars : MonoBehaviour
{
    public float StartInterval = 10f;
    public float FallInterval = 10f;
    public Transform Sun;
    public Side UpperSide;
    public Side BottomSide;
    private List<StarAlpha> _stars;
    private World _world;

    private void Start()
    {
        _stars = GetComponentsInChildren<StarAlpha>().ToList();
        _world = FindObjectOfType<World>();
        StartCoroutine(FallStar());
    }

    private IEnumerator FallStar()
    {
        yield return new WaitForSeconds(StartInterval);
        while (!_world.IsGameOver)
        {
            yield return new WaitForSeconds(FallInterval);

            if (_world.IsGameOver)
                continue;

            if (_stars.Count > 0)
            {
                StarAlpha star = null;
                while (star == null)
                {
                    int randomIndex = Random.Range(0, _stars.Count);

                    if (_stars[randomIndex].SpriteRenderer.material.color.a < 0.1f)
                    {
                        yield return null;
                    }
                    else
                    {
                        star = _stars[randomIndex];
                        _stars.RemoveAt(randomIndex);
                    }
                }

                // TODO destr что-то хочет
                float position;
                float upperDistance = Vector3.Distance(star.transform.position, UpperSide.transform.position);
                float bottomDistance = Vector3.Distance(star.transform.position, BottomSide.transform.position);
                if (upperDistance > bottomDistance)
                {
                    star.transform.SetParent(BottomSide.transform, true);
                    position = Random.Range(-_world.Earth.Radius, _world.Earth.Radius);
                }
                else
                {
                    star.transform.SetParent(UpperSide.transform, true);
                    position = Random.Range(-_world.Earth.Radius, _world.Earth.Radius);
                }
                star.Fall(new Vector3(position, 0, 0));
            }
            else
            {
                // _world.IsGameOver = true;
                yield break;
            }
        }
    }

    private void Update()
    {
    }
}
