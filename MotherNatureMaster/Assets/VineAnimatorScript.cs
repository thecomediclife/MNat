using UnityEngine;
using System.Collections;

public class VineAnimatorScript : MonoBehaviour {

	public float playbackTime;

	private Animator anim;
	public PillarController parentObj;

	public float playSpeed = 1.0f;

	private bool play;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
		play = !parentObj.grow;
		playSpeed = parentObj.moveSpeed / 4f;
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo (0);

		playbackTime = currentState.normalizedTime % 1;

		if (parentObj.grow) {

			if (playbackTime < parentObj.pillarHeight * 0.1f) {
				anim.speed = playSpeed;

				if (!play) {
					anim.Play("Grow", -1, playbackTime);
					play = true;
				}
			} else {
				anim.speed = 0;
			}

		} else if (!parentObj.grow) {

			if (playbackTime > 0.1f) {
				anim.speed = playSpeed;

				if (play) {
					anim.Play("Shrink", -1, 1f - playbackTime);
					play = false;
				}
			} else {
				anim.speed = 0;
			}

		}

	}
}
