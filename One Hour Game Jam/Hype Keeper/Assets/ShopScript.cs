using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
	public MainManager Man;
	public int Level = 0;
	public int Cost = 100;
	public int Hp = 10;

	public SpriteRenderer Sprite;
	
	void Start ()
	{
		Man = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainManager>();
		Sprite = GetComponent<SpriteRenderer>();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.CompareTag("Player"))
		{
			Sprite.color = new Color(1,Hp/10f,Hp/10f);
			Man.Money += (int) (Random.value * 10 + 5);
			Destroy(other.gameObject);
			Hp--;
			if (Hp == 0)
			{
				Destroy(gameObject);
			}
		}
	}

	void OnMouseDown()
	{
		if (Man.Money >= Cost)
		{
			Man.Money -= Cost;
			Cost *= 2;
			Level++;
		}
		
	}
}
