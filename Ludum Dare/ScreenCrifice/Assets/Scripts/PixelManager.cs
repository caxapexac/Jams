using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelManager : MonoBehaviour
{
	private void OnEnable()
	{
		transform.localScale = Vector3.zero;
		StartCoroutine(FadeIn());
	}

	private IEnumerator FadeIn()
	{
		while (transform.localScale.x < 1)
		{
			//transform.localScale = Vector3.one;
			transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);//Vector3.one * Time.deltaTime);
			yield return null;
		}
		transform.localScale = Vector3.one;
	}
	
	void Update ()
	{
			
	}
}
