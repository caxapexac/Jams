using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoneThrower : MonoBehaviour
{
	public GameObject BonePrefab;
	public Camera CameraObj;
	public float Delay;
	public float Tick;
	public int Score = 0;
	public Text ScoreText;
	
	void Awake ()
	{
		ScoreText = GameObject.FindGameObjectWithTag("Finish").GetComponent<Text>();
		CameraObj = GetComponent<Camera>();
	}
	
	
	void Update ()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
		{
			ThrowBone();
		}
	}

	public void ThrowBone()
	{
		RaycastHit hit;
		Ray ray = CameraObj.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out hit);
		Vector3 pos = hit.point;
		pos.z += 150;
		GameObject go = Instantiate(BonePrefab, hit.point, Quaternion.Euler(0, 0, Random.value * 360));
		go.GetComponent<Rigidbody>().AddForce(ray.direction * 1000);
		
	}

	public void UpdScore()
	{
		Score += 1;
		ScoreText.text = "Score: " + Score.ToString();
	}
}
