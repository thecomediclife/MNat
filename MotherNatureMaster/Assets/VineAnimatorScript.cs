using UnityEngine;
using System.Collections;

public class VineAnimatorScript : MonoBehaviour {

	public float height = 4.0f;

	public bool grow = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo currentState = GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0);

		float playbackTime = currentState.normalizedTime % 1;

/*		if (grow) {

			GetComponent<Animator> ().speed = 1;
			if (playbackTime > height / 10f) {
				GetComponent<Animator> ().speed = 0;
			}

		} else {

			GetComponent<Animator> ().speed = -1;

			Debug.Log (playbackTime);

		} */

		if (Input.GetKeyDown (KeyCode.Z)) {
			GetComponent<Animator> ().speed = 0;
		//	Debug.Log (GetComponent<Animator>().time
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			grow = true;
			GetComponent<Animator> ().speed = 1;
			//GetComponent<Animator>().Play(currentState.shortNameHash, -1, playbackTime);
		}

		if (Input.GetKeyDown(KeyCode.C)) {
			grow = false;
			GetComponent<Animator> ().speed = 1;
			//GetComponent<Animator>().Play(currentState.shortNameHash, -1, playbackTime);
		}


		GetComponent<Animator>().SetBool("Grow", grow);
		//Debug.Log (playbackTime);
	}
}
