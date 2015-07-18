using UnityEngine;
using System.Collections;

public class PlatformScript : MonoBehaviour {

	public bool boyOnPlatform;

	void Update () 
	{
		GetComponentInParent<TreeController3> ().boyOnPlatform = boyOnPlatform;
	}

	void OnTriggerEnter (Collider other) 
	{
		if (other.name == "Boy") 
		{
			boyOnPlatform = true;
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		if (other.name == "Boy") 
		{
			boyOnPlatform = false;
		}	}
}
