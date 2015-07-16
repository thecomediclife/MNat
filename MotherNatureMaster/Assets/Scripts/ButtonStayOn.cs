using UnityEngine;
using System.Collections;

public class ButtonStayOn : MonoBehaviour {

	public bool isClicked;
	
	void Start ()
	{
		isClicked = false;
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") 
		{
			isClicked = !isClicked;
			ChangeColor();
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") 
		{
			isClicked = !isClicked;
			ChangeColor();
		}
	}
	
	void ChangeColor ()
	{
		switch (isClicked) 
		{
		case true:
			GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Color.blue);
			break;
		case false:
			GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Color.red);
			break;
		}
	}
}
