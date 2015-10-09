using UnityEngine;
using System.Collections;

public class BombScript2 : MonoBehaviour {
	public bool activate = false;

	public float detonateTime = 10f;

	private float timer;

	private Transform bomb;

	public bool detonate;

	public GameObject particle;

	void Awake() {
		bomb = transform.GetChild (0);
	}

	// Update is called once per frame
	void Update () {
		if (activate) {
			if (Time.time > timer - 6f && Time.time < timer - 2f) {
				bomb.GetComponent<Animator> ().SetInteger ("BlipState", 2);
			}

			if (Time.time > timer - 2f) {
				bomb.GetComponent<Animator> ().SetInteger ("BlipState", 3);
			}

			if (Time.time > timer) {
				//Debug.Log ("detonate");
				detonate = true;
			}
		}

//		if (Input.GetKeyDown(KeyCode.Z)) {
//			SetActive();
//		}
	}

	public void SetActive() {
		activate = true;
		timer = Time.time + detonateTime;
		bomb.GetComponent<Animator> ().SetInteger ("BlipState", 1);
	}

}
