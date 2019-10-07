using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.XR;

public class Manager : MonoBehaviour
{
	public Transform Player;
	public GameObject Repost;
	private int b = 100;
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Random.value > 0.97)
		{
			GameObject a = Instantiate(Repost);
			a.transform.position = new Vector3(Player.position.x + Random.value * 20*Player.localScale.magnitude - Random.value * 20*Player.localScale.magnitude, Player.position.y + 30 * Random.value*Player.localScale.magnitude);
			a.transform.localScale = new Vector3(Player.localScale.x*Random.value*5, Player.localScale.y*Random.value*5);
			a.GetComponent<CollisionCoin>().Player = Player;
		}
		Player.GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.GetAxis("Horizontal")*Player.localScale.magnitude*100, Input.GetAxis("Vertical")*Player.localScale.magnitude*100*0));
		b++;
		if (Input.GetAxis("Jump") >= 0.01f && b > 50)
		{
			Player.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.value*100 - Random.value*100*Player.localScale.magnitude, Input.GetAxis("Jump")*Player.localScale.magnitude*1500));
			b = 0;
		}
		
		
	}
	
}
