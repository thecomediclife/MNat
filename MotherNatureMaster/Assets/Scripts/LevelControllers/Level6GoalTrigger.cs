using UnityEngine;
using System.Collections;

public class Level6GoalTrigger : MonoBehaviour {
	public bool triggered;

	void OnTriggerEnter(Collider other) {
		triggered = true;
		this.transform.GetChild (0).GetComponent<Light> ().color = Color.cyan;
	}
}
