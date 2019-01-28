using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFade : MonoBehaviour
{
	private TextMesh _textMesh;
	private float _tick = 0;
	void Start ()
	{
		_textMesh = GetComponent<TextMesh>();
	}
	
	void Update ()
	{
		_tick += Time.deltaTime;
		transform.Translate(Vector3.up * Time.deltaTime);
		if (_tick > 1.5f)
		{
			Destroy(gameObject);
		}
	}
}
