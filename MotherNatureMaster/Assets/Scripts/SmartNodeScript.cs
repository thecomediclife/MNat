using UnityEngine;
using System.Collections;

public class SmartNodeScript : MonoBehaviour {
	public int index = 0;

	public bool triggerEnabled = true;					//Enables the smart node script
	public bool ignorable = true;						//Checks if directing path will ignore this smart node.

	public bool enabled1 = false;						//Pause function
	public bool enabled2 = false;						//Continue function
	public bool enabled3 = false;						//PauseTimed function
	public bool enabled4 = false;						//Choosedirection

	public Transform snapToTarget;						//target to snap to.
	public bool lookAtTarget = false;					//bool to check if boy will look at something
	public Transform lookDirection;						//transform to look at

	public bool chooseDirection = false;				//bool to check if direction will be chosen.
	public Transform chosenDirection;					//Transform that is the next chosen direction

	public float delay = 0.0f;							//float that determines how many seconds to wait for PauseTimed.

	public bool deactivateAfterPlay = false;			//Will object be deactivated after it plays the action?
	public int playTimes = 1;							//How many times will node play the action before deactivating.
	public bool playNextAction = false;					//bool to check if there is another smart node to be activated after this one deactivates.
	public GameObject nextActionInSequence;				//the node that has the next action already set up.

	private bool played = false;						//bool to check if node has actually played the action or not when colliding with boy.

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Kid" && triggerEnabled) {
			if (ignorable) {

				if (other.GetComponent<CharController6>().currentState != CharController6.State.DirectedPath)
					PlayAction(other);

			} else if (!ignorable) {
				PlayAction(other);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Kid" && triggerEnabled) {
			ReducePlayTimes(other);

			played = false;
		}
	}

	void PlayAction(Collider other) {
		if (enabled1) {
			other.GetComponent<CharController6> ().Pause (snapToTarget, lookAtTarget, lookDirection);
			//other.GetComponent<CharController6>().ChooseDirection(transform, connectedNode);
			//ther.GetComponent<CharController6>().PauseTimed(transform, true, transform.position + new Vector3(1,0,0), 2f, CharController6.State.ChosenDir, connectedNode);
			played = true;

		} else if (enabled2) {
			other.GetComponent<CharController6> ().Continue (chooseDirection, chosenDirection);
			played = true;

		} else if (enabled3) {
			other.GetComponent<CharController6> ().PauseTimed (snapToTarget, lookAtTarget, lookDirection, delay, chooseDirection, chosenDirection);
			played = true;

		} else if (enabled4) {
			other.GetComponent<CharController6> ().ChooseDirection (snapToTarget, chosenDirection);
			played = true;

		} else if (!enabled1 && !enabled2 && !enabled3 && !enabled4) {
			played = true;
		}
		//Debug.Log ("play action " + transform.name);
	}

	void ReducePlayTimes(Collider other) {
		if (deactivateAfterPlay && played) {
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
