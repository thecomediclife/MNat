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
		if (other.tag != "Obstacle")
			Destroy (gameObject);
	}
}
