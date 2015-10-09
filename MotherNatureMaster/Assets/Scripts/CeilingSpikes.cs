using UnityEngine;
using System.Collections;

public class CeilingSpikes : MonoBehaviour {
	public float height = 4f;
	public float delay = 2f;

	public float fallSpeed = 10f;
	public float riseSpeed = 5f;

	private float timer;
	private bool fall = true;


	// Use this for initialization
	void Start () {
		this.transform.localPosition = new Vector3 (0f, height, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > timer + delay) {

			if (fall) {
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, fallSpeed * Time.deltaTime);
				if (Vector3.Distance(transform.localPosition, Vector3.zero) < 0.1f) {
					fall = false;
				}
			} else {
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0f, height, 0f), riseSpeed * Time.deltaTime);
				if (Vector3.Distance(transform.localPosition, new Vector3(0f, height, 0f)) < 0.1f) {
					fall = true;
					timer = Time.time;
				}
			}
		}


	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Kid") {
			other.GetComponent<CharController6>().Respawn ();
		}
	}

}
