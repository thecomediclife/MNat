using UnityEngine;
using System.Collections;

public class PlatformScript : MonoBehaviour {

	private bool boyOnPlatform;

	void Update () 
	{
		GetComponentInParent<TreeController3> ().boyOnPlatform = boyOnPlatform;
	}

	void OnTriggerEnter (Collider other) 
	{
		if (other.tag == "Boy") 
		{
			boyOnPlatform = true;
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Boy") 
		{
			boyOnPlatform = false;
		}	
	}
}
