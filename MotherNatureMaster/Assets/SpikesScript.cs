using UnityEngine;
using System.Collections;

public class SpikesScript : MonoBehaviour {
	public bool enableSpikes = true;

	void Update() {
		if (enableSpikes) {
			this.GetComponent<Animator>().SetBool("Loop", true);
		} else {
			this.GetComponent<Animator>().SetBool("Loop", false);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Kid") {
			other.GetComponent<CharController6>().Respawn();
		}
	}

	void EnableTrigger() {
		this.GetComponent<Collider> ().enabled = true;
	}

	void DisableTrigger() {
		this.GetComponent<Collider> ().enabled = false;
	}
}
