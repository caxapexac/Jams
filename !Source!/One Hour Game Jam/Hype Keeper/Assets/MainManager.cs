using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
	public int Money;
	public int Hp;
	public float Difficulty;
	public float Tick;
	public Text MoneyText;
	public Text HpText;
	public GameObject ShopPrefab;
	public Text LoseText;

	public GameObject HumanPrefab;
	public GameObject Music;
	void Start ()
	{
		GameObject music = GameObject.FindWithTag("Respawn");
		if (music == null)
		{
			music = Instantiate(Music);
			music.SetActive(true);
		}
		DontDestroyOnLoad(music);
	}
	
	
	void Update ()
	{
		MoneyText.text = Money.ToString();
		HpText.text = Hp.ToString();
		Tick += Time.deltaTime;
		if (Tick > Difficulty)
		{
			Spawn();
			Difficulty *= 0.965f;
			Tick = 0;
		}
	}

	private void Spawn()
	{
		GameObject go = Instantiate(HumanPrefab);
		
		go.transform.localPosition = transform.localPosition;
		go.SetActive(true);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		LoseText.gameObject.SetActive(true);
		GetComponent<AudioSource>().Play();
		StartCoroutine(Restart());
	}

	private IEnumerator Restart()
	{
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene(0);
	}
}
