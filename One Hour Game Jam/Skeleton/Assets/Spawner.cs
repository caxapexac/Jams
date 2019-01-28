using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public float Delay;
	public float tick = 0;
	public float Mul = 10;
	public GameObject Skeleton;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		tick += Time.deltaTime;
		if (tick > Delay)
		{
			Spawn();
			Delay *= 0.95f;
			tick = 0;
		}
	}

	private void Spawn()
	{
		Instantiate(Skeleton, transform.position + Mul * Vector3.left * (Random.value - 0.5f), Quaternion.identity);
	}
}
