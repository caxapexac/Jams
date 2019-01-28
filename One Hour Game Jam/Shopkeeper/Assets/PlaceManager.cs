using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{

	public MainManager Man;

	private void Start()
	{
		Man = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainManager>();
		GameObject go = Instantiate(Man.ShopPrefab, Vector3.zero, new Quaternion());
		go.SetActive(true);
	}

	private void OnMouseDown()
	{
		if (Man.Money >= 100)
		{
			Man.Money -= 100;
			GameObject go = Instantiate(Man.ShopPrefab, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), new Quaternion());
			go.SetActive(true);
		}
		
	}
}
