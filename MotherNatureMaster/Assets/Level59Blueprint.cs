using UnityEngine;
using System.Collections;

public class Level59Blueprint : MonoBehaviour {
	public Transform button, crate;

	private bool reset = true;

	void Start () {
	
	}
	

	void Update () {
		if (button.GetComponent<PillarController> ().grow == reset) {
			reset = !reset;

			crate.position = new Vector3(-3f,12f,-2f);
		}
	}
}
