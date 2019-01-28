using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
	[HideInInspector]
	public Man Man;

	public GameObject But1;
	public GameObject But2;
	public GameObject But3;
	public Sprite[] StartDialogue;
	public RectTransform[] Lines;
	public RectTransform Death;
	public RectTransform Inventory;
	public Image DialogueImage;
	public float UiScale;

	void Start()
	{
		Man = GetComponent<Man>();
		Inventory.localScale = Vector2.one * Man.ScreenH * UiScale;
		Inventory.gameObject.SetActive(false);
	}

	public void ButtonStart(float diff)
	{
		SetStart(diff);
		But1.SetActive(false);
		But2.SetActive(false);
		But3.SetActive(false);
	}
	
	void SetStart(float diff)
	{
		Man.DiffMultipiler = diff;
		
		if (Man.SkipScenes)
		{
			Man.InputM.enabled = true;
			Man.MouseM.enabled = true;
			Man.DeathM.enabled = true;
			Man.RocketM.enabled = true;
			
			Inventory.anchoredPosition += Vector2.right * 300;
			Man.Res.SetActive(true);
		}
		else
		{
			foreach (var line in Lines) StartCoroutine(UpHeight(line, Man.ScreenH / 5));
		}
	}

	private IEnumerator UpHeight(RectTransform line, int size)
	{
		while (line.sizeDelta.y < size)
		{
			line.sizeDelta += Vector2.up * 65 * Time.deltaTime;
			yield return null;
		}

		StartCoroutine(SwipeDeath(Death, -Man.ScreenW / 4));
	}
	
	private IEnumerator SwipeDeath(RectTransform death, int size)
	{
		death.gameObject.SetActive(true);
		death.sizeDelta = Vector2.one * Man.ScreenH / 2;
		while (death.anchoredPosition.x > size)
		{
			death.anchoredPosition -= Vector2.right * 90 * Time.deltaTime;
			yield return null;
		}
		StartCoroutine(Dialogue(DialogueImage, StartDialogue));
	}

	private IEnumerator Dialogue(Image image, Sprite[] startDialogue)
	{
		image.rectTransform.sizeDelta = Vector2.one * Man.ScreenH / 4;
		image.color = Color.white;
		foreach (var sprite in startDialogue)
		{
			image.sprite = sprite;
			yield return new WaitForSeconds(0.2f);
			while (!Input.anyKeyDown)
			{
				yield return null;
			}
			yield return null;
		}
		image.color = Color.clear;
		StartCoroutine(UnSwipeDeath(Death, 0));
	}

	private IEnumerator UnSwipeDeath(RectTransform death, int size)
	{
		Man.Alive = true;
		Man.InputM.enabled = true;
		Man.MouseM.enabled = true;
		Man.DeathM.enabled = true;
		Man.RocketM.enabled = true;
		Man.Res.SetActive(true);
		while (death.anchoredPosition.x < size)
		{
			death.anchoredPosition += Vector2.right * 80 * Time.deltaTime;
			yield return null;
		}
		death.gameObject.SetActive(false);
		foreach (var line in Lines) StartCoroutine(DownHeight(line, 0));
		StartCoroutine(InventoryRight(Inventory, 0));
	}

	private IEnumerator InventoryRight(RectTransform line, int size)
	{
		Inventory.gameObject.SetActive(true);
		while (line.anchoredPosition.x < 50)
		{
			line.anchoredPosition += Vector2.right * 70 * Time.deltaTime;
			yield return null;
		}
	}

	private IEnumerator DownHeight(RectTransform line, int size)
	{
		while (line.sizeDelta.y > 0)
		{
			line.sizeDelta -= Vector2.up * 40 * Time.deltaTime;
			yield return null;
		}

	}
	

	void Update ()
	{
		//Inventory.localScale = Vector2.one * Man.ScreenH * UiScale;
	}
}
