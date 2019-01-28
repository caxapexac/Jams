using System.Collections;
using System.Collections.Generic;
using LeopotamGroup.Pooling;
using UnityEngine;

public class RocketManager : MonoBehaviour
{
	public Man Man;
	public PoolContainer Pool;
	public float _tick;
	
	void Start ()
	{
		Man = GetComponent<Man>();
		Pool = PoolContainer.CreatePool("Prefabs/RocketPrefab");
		_tick = -Man.RocketDelay;
	}

	void Update ()
	{
		_tick += Time.deltaTime;
		if (_tick >= Man.RocketFrequency)
		{
			IPoolObject go = Pool.Get();
			go.PoolTransform.GetComponent<Rocket>().Me = go;
			go.PoolTransform.GetComponent<Rocket>().Man = Man;
			go.PoolTransform.GetComponent<Rocket>().Speed = Random.value * Man.RocketSpeed + 0.5f;
			go.PoolTransform.localPosition = Man.PlayerM.Player.localPosition + (Vector3)(Random.insideUnitCircle.normalized) * Man.ScreenW / 30;
			go.PoolTransform.LookAt2D(Man.PlayerM.Player.localPosition);
			go.PoolTransform.gameObject.SetActive(true);
			_tick = 0;
		}
	}
}
