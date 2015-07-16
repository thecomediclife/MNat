using UnityEngine;
using System.Collections;

public class ButtonPermanent : MonoBehaviour {

	public bool isClicked;
	
	void Start ()
	{
		isClicked = false;
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") 
		{
			isClicked = true;
			ChangeColor();
		}
	}
	
	void ChangeColor ()
	{
		switch (isClicked) 
		{
		case true:
			GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Color.green);
			break;
		case false:
			GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Color.red);
			break;
		}
	}
}
