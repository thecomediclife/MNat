using UnityEngine;
using System.Collections;

public class KidAnimControllerScript : MonoBehaviour {
	private Animator anim;
	private CharController6 kid;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
		kid = this.transform.parent.GetComponent<CharController6> ();
	}

	//{Default, Pause, Continue, PauseTimed, ChosenDir, SnapTo, DirectedPath};

	// Update is called once per frame
	void Update () {
		switch (kid.currentState) {
		case CharController6.State.Default:

			anim.SetBool("Locomotion", true);
			anim.speed = kid.speed / 2f;

			break;

		case CharController6.State.Pause:

			anim.SetBool("Locomotion", false);
			anim.speed = 1f;

			break;

		case CharController6.State.Continue:

			anim.SetBool("Locomotion", true);
			anim.speed = kid.speed / 2f;

			break;

		case CharController6.State.PauseTimed:

			anim.SetBool("Locomotion", false);
			anim.speed = 1f;

			break;

		case CharController6.State.ChosenDir:

			anim.SetBool("Locomotion", true);
			anim.speed = kid.speed / 2f;

			break;

		case CharController6.State.SnapTo:

			anim.SetBool("Locomotion", false);
			anim.speed = 1f;

			break;

		case CharController6.State.DirectedPath:

			anim.SetBool("Locomotion", true);
			anim.speed = kid.speed / 2f;

			break;
		}
	}
}
