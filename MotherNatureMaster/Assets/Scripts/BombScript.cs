using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {
	public bool activate = false;

	public float detonateTime = 7f;

	private float timer;

	public Material defaultMat, redMat;

	private Transform bomb;

	public bool detonate;

	void Awake() {
		bomb = transform.GetChild (0);
	}

	// Update is called once per frame
	void Update () {
		if (activate) {
			if (Time.time > timer - detonateTime * 0.6f && Time.time < timer - detonateTime * 0.2f) {
				bomb.GetComponent<Animator> ().SetInteger ("BlipState", 2);
			}

			if (Time.time > timer - detonateTime * 0.2f) {
				bomb.GetComponent<Animator> ().SetInteger ("BlipState", 3);
			}

			if (Time.time > timer) {
//				Debug.Log ("detonate");
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

	public void Reset() {
		activate = false;
		detonate = false;
		bomb.GetComponent<Animator> ().SetInteger ("BlipState", 0);
	}
}
