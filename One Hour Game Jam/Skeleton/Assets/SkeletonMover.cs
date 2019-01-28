using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonMover : MonoBehaviour
{
	public Sprite[] States;
	public int Hp = 5;
	private SpriteRenderer sr;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	void Update ()
	{
		transform.Translate((Vector3.up * 3 + Vector3.left * (Random.value - 0.5f) * 20) * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		Hp -= 1;
		if (Hp == 0)
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<BoneThrower>().UpdScore();
			Destroy(gameObject);
		}
		else
		{
			sr.sprite = States[6 - Hp];
		}
		Destroy(other.gameObject);
	}


}
