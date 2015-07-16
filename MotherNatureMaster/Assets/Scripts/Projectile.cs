using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float waitTime = 4f;

	void Start ()
	{
		StartCoroutine (WaitToDestruct (waitTime));
	}

	IEnumerator WaitToDestruct (float waitTime) {
		yield return new WaitForSeconds(waitTime);
		Destroy (gameObject);
	}

	void OnTriggerEnter (Collider other) {

		//	If projectile hits player
		if (other.tag == "Player") {
			other.GetComponent<CharController3> ().ReverseDirection ();
		}

		//	If projectile hits tree
//		if (other.tag == "") {
//			other.GetComponent<TreeHealth>().takeDamage();
//		}

	}
}
