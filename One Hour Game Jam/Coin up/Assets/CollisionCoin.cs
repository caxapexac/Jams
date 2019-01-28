using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCoin : MonoBehaviour
{
	public Transform Player;

	// Use this for initialization
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Finish") return;
		if (other.tag == "Player")


		{
			Player.localScale = new Vector3(Player.localScale.x*1.1f, Player.localScale.y*1.1f);
            Player.GetComponentInChildren<Camera>().orthographicSize *= 1.1f;
			Player.GetComponent<Rigidbody2D>().mass *= 1.1f;
		}
		
		Destroy(gameObject);
	}
	

}
