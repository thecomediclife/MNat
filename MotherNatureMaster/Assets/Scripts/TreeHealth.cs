using UnityEngine;
using System.Collections;

public class TreeHealth : MonoBehaviour {

	public float currentHealth;
	public float startHealth = 20;
	
	void Start () {
		currentHealth = startHealth;
	}

	void Update () 
	{
		if (currentHealth < 0)
			Rot ();
	}

	public void takeDamage (float i) {
		currentHealth -= i;
	}

	void Rot () {
		//	Call tree script to rot instantly
//		GetComponent<TreeController2> ().Rot ();
	}
}
