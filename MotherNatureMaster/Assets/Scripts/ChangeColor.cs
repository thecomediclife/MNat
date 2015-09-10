using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {

	public bool change = false;
	public Renderer rend;
	private Color curColor;
	private Color startColor;
	public Color endColor;
	private float t = 0;

	void Start () 
	{
		rend = GetComponent<Renderer> ();
		rend.material.shader = Shader.Find ("Custom/GradientAndDirColorY");

		startColor = rend.material.GetColor ("_ColorHigh");
	}

	void Update () {
		if (change) {
			t += Time.deltaTime;
			curColor = Vector4.Lerp (startColor, endColor, t);
			rend.material.SetColor ("_ColorHigh", curColor);
		} else {
			rend.material.SetColor("_ColorHigh", startColor);
		}

	}
}
