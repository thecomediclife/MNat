using UnityEngine;
using System.Collections;

public class Level15Blueprint : MonoBehaviour {
	public Transform[] spikes1, spikes2;

	public Transform button, crate, platform, kid, platform2;

	private bool reset = true;

	public Vector3 switch1, switch2;

	private bool move = false;

	public Transform pillar1, pillar2;

	public Transform origNode, origNexNode;

	private bool move2 = false;

	// Use this for initialization
	void Start () {
		kid = GameObject.Find ("Kid").transform;
	}
	
	// Update is called once per frame
	void Update () {

		//Resets the crate position when you press the button.
		if (button.GetComponent<PillarController>().grow == reset) {
			reset = !reset;

			crate.transform.position = new Vector3(-4f,22f,-4f);
		}

		if (crate.transform.position.y < -10f) {
			crate.transform.position = new Vector3(-4f,22f,-4f);
		}

		//Enables Blue spikes if crate is on the switch.
		if (Vector3.Distance (crate.position, switch1) < 0.1f) {
			for (int i = 0; i < spikes1.Length; i++) {
				spikes1[i].GetComponent<SpikesScript>().enableSpikes = true;
			}
		} else {
			for (int i = 0; i < spikes1.Length; i++) {
				spikes1[i].GetComponent<SpikesScript>().enableSpikes = false;
			}
		}

		if (Vector3.Distance (crate.position, switch2) < 0.1f) {
			for (int i = 0; i < spikes2.Length; i++) {
				spikes2 [i].GetComponent<SpikesScript> ().enableSpikes = true;
			}
		} else {
			for (int i = 0; i < spikes2.Length; i++) {
				spikes2[i].GetComponent<SpikesScript>().enableSpikes = false;
			}
		}

		if (Vector3.Distance (kid.position, new Vector3 (-1f, 4f, -3f)) < 0.1f) {
			move = true;
//			kid.GetComponent<CharController6>().SetRespawn(new Vector3(-1f, 4f, -3f), origNode, origNexNode);
		}

		if (move) {
			platform.position = Vector3.MoveTowards(platform.position, new Vector3(0f, -10f,0f), 2.5f * Time.deltaTime);
		}

		if (pillar1.GetComponent<PillarController> ().grow) {
			pillar2.GetComponent<PillarController> ().grow = false;
			pillar2.GetComponent<PillarController> ().inputEnabled = false;
		} else {
			pillar2.GetComponent<PillarController> ().inputEnabled = true;
		}

		if (pillar2.GetComponent<PillarController> ().grow) {
			pillar1.GetComponent<PillarController> ().grow = false;
			pillar1.GetComponent<PillarController> ().inputEnabled = false;
		} else {
			pillar1.GetComponent<PillarController> ().inputEnabled = true;
		}

		if (Input.GetKeyDown (KeyCode.Z)) {
			kid.GetComponent<CharController6>().Respawn();
		}

		if (Vector3.Distance(kid.position, new Vector3(-5f, 13.5f, 1f)) < 0.1f) {
			move2 = true;
		}

		if (move2) {
			platform2.position = Vector3.MoveTowards(platform2.position, Vector3.zero, 2.5f * Time.deltaTime);
		}
	}
}
