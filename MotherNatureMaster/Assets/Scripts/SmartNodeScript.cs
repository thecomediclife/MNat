using UnityEngine;
using System.Collections;

public class SmartNodeScript : MonoBehaviour {
	public int index = 0;

	public bool triggerEnabled = true;

	public bool enabled1 = false;
	public bool enabled2 = false;
	public bool enabled3 = false;
	public bool enabled4 = false;

	public Transform snapToTarget;
	public bool lookAtTarget = false;
	public Transform lookDirection;

	public bool chooseDirection = false;
	public Transform chosenDirection;

	public float delay = 0.0f;

	public bool deactivateAfterPlay = false;
	public int playTimes = 1;
	public bool playNextAction = false;
	public GameObject nextActionInSequence;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Kid" && triggerEnabled) {
			if (enabled1) {
				other.GetComponent<CharController6> ().Pause (snapToTarget, lookAtTarget, lookDirection);
//				other.GetComponent<CharController6>().ChooseDirection(transform, connectedNode);
//				other.GetComponent<CharController6>().PauseTimed(transform, true, transform.position + new Vector3(1,0,0), 2f, CharController6.State.ChosenDir, connectedNode);

			} else if (enabled2) {
				other.GetComponent<CharController6>().Continue(chooseDirection, chosenDirection);

			} else if (enabled3) {
				other.GetComponent<CharController6>().PauseTimed(snapToTarget, lookAtTarget, lookDirection, delay, chooseDirection, chosenDirection);

			} else if (enabled4) {
				other.GetComponent<CharController6>().ChooseDirection(snapToTarget, chosenDirection);
			}
			Debug.Log ("play action " + transform.name);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Kid" && triggerEnabled) {
			if (deactivateAfterPlay) {
				playTimes--;

				if (playTimes <= 0) {
					if (playNextAction) {
						nextActionInSequence.SetActive(true);
						gameObject.SetActive(false);
					} else {
						triggerEnabled = false;
					}
				}
			}
		}
	}
}
