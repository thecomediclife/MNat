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
		if (other.tag == "Kid") {
			other.GetComponent<CharController3> ().ReverseDirection ();
			Destroy (gameObject);
		}

		//	If projectile hits tree
		if (other.tag == "Tree") {
			other.GetComponentInParent<TreeController4>().takeDamage(1);
			Destroy (gameObject);
		}

	}
}
