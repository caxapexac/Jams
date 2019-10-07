using System.Collections;
using System.Collections.Generic;
using LeopotamGroup.Pooling;
using UnityEngine;

public class Rocket : MonoBehaviour
{
	public Man Man;
	public IPoolObject Me;
	public float Speed;
	private float _tick;
	
	void Update ()
	{
		transform.Translate(Vector3.right * Time.deltaTime * Speed);
		if (Vector3.Distance(transform.localPosition, Man.PlayerM.Player.localPosition) > Man.ScreenW / 20)
		{
			Man.RocketM.Pool.Recycle(Me);
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		Man.DeathM.Extra += (int)((Random.value + 0.3f) * Man.RocketDamage);
		GameObject boom = Instantiate(Man.ParticleBreak);
		boom.transform.localPosition = transform.position;
		boom.SetActive(true);
		Man.RocketM.Pool.Recycle(Me);
	}
}
