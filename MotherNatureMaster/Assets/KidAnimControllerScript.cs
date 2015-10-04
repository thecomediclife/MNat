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

			DetermineKidMovement();

			break;

		case CharController6.State.Pause:

			anim.SetBool("Locomotion", false);
			anim.speed = 1f;

			break;

		case CharController6.State.Continue:

			DetermineKidMovement();

			break;

		case CharController6.State.PauseTimed:

			anim.SetBool("Locomotion", false);
			anim.speed = 1f;

			break;

		case CharController6.State.ChosenDir:

			DetermineKidMovement();

			break;

		case CharController6.State.SnapTo:

			DetermineKidMovement();

			break;

		case CharController6.State.DirectedPath:

			DetermineKidMovement();

			break;
		}
	}

	void DetermineKidMovement() {
//		int counter = 0;
//		for (int i = 0; i < kid.nodeArray.Length; i++) {
//			if (kid.nodeArray[i] != null) {
//				counter++;
//			}
//		}
//		
//		if (counter > 1) {
//			anim.SetBool("Locomotion", true);
//			anim.speed = kid.speed / 2f;
//		} else { 
//			anim.SetBool("Locomotion", false);
//			anim.speed = 1f;
//		}

		if (kid.currentNode != kid.nextNode) {
			anim.SetBool("Locomotion", true);
			anim.speed = kid.speed / 2f;
		} else {
			anim.SetBool("Locomotion", false);
			anim.speed = 1f;
		}
	}
}
