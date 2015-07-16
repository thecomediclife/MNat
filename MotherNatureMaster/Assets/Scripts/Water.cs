using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {
	
	void OnTriggerEnter (Collider other) {
		Destroy (gameObject);
	}
}
