using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{

	public Material Mat;

	public Color Current;
	// Use this for initialization
	void Start ()
	{
		/*Mat = GetComponent<Renderer>().material;
		Mat.SetColor("_Color", Color.black);
		Current = Color.black;*/
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*float a = Random.value;
		Current.r = Mathf.PingPong(Current.r + Random.value * 0.1f, 1f);
		Current.g = Mathf.PingPong(Current.r + Random.value * 0.1f, 1f);
		Current.b = Mathf.PingPong(Current.r + Random.value * 0.1f, 1f);
		Mat.shader = Shader.Find("_Color");
		Mat.SetColor("_Color", Current);*/
	}
}
