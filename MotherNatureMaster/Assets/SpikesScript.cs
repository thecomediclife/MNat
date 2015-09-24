using UnityEngine;
using System.Collections;

public class SpikesScript : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Kid") {
			other.GetComponent<CharController6>().Respawn();
		}
	}
}
