using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{

	private float Speed;
	void Start ()
	{
		Speed = (Random.value + 0.5f) * 5;
		GetComponent<SpriteRenderer>().color = Random.ColorHSV();
	}
	

	void Update ()
	{
		transform.Translate((Vector2.right + Vector2.down * (Random.value - 0.5f) * 2) * Time.deltaTime * Speed);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.CompareTag("Finish"))
		{
			Destroy(gameObject);
		}
	}
}
