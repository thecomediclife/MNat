using UnityEngine;
using System.Collections;

public class BombScript3 : MonoBehaviour {
	public bool activate = false;
	
	public float detonateTime = 7f;
	
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
			if (Time.time > timer - detonateTime * 0.6f && Time.time < timer - detonateTime * 0.2f) {
				bomb.GetComponent<Animator> ().SetInteger ("BlipState", 2);
			}
			
			if (Time.time > timer - detonateTime * 0.2f) {
				bomb.GetComponent<Animator> ().SetInteger ("BlipState", 3);
			}
			
			if (Time.time > timer) {
				detonate = true;
				particle.SetActive(true);
			}
		}

	}
	
	public void SetActive() {
		activate = true;
		timer = Time.time + detonateTime;
		bomb.GetComponent<Animator> ().SetInteger ("BlipState", 1);
	}
	
	public void Reset(Vector3 returnVec) {
		StartCoroutine (ResetAction (returnVec));
		activate = false;

		for (int i = 0; i < transform.GetChild(0).childCount; i++) {
			transform.GetChild(0).GetChild(i).GetComponent<Renderer>().enabled = false;
		}

//		particle.SetActive (false);
//		activate = false;
//		detonate = false;
//		bomb.GetComponent<Animator> ().SetInteger ("BlipState", 0);
	}

	IEnumerator ResetAction(Vector3 retVec) {
		yield return new WaitForSeconds (0.5f);
		particle.SetActive (false);
		detonate = false;
		bomb.GetComponent<Animator> ().SetInteger ("BlipState", 0);
		transform.position = retVec;
		for (int i = 0; i < transform.GetChild(0).childCount; i++) {
			transform.GetChild(0).GetChild(i).GetComponent<Renderer>().enabled = true;
		}
	}
}
