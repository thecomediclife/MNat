using UnityEngine;
using System.Collections;

public class RingGateTest : MonoBehaviour {
	public Animator animController;


	// Use this for initialization
	void Start () {
		animController = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			animController.SetBool("Key1", true);
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			animController.SetBool("Key2", true);
		}
	}
}
