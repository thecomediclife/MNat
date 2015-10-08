using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	public bool reset = true;

	public Transform originTransform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -10f) {
			this.GetComponent<Rigidbody>().useGravity = false;
			this.GetComponent<Rigidbody>().velocity = Vector3.zero;
			this.GetComponent<Renderer>().enabled = false;
			reset = true;
		}
	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Kid" && other.transform != originTransform) {
			other.GetComponent<CharController6>().Respawn();
		}

		if (other.GetComponent<CrateScript2> () != null) {
			this.GetComponent<Rigidbody>().useGravity = false;
			this.GetComponent<Rigidbody>().velocity = Vector3.zero;
			this.GetComponent<Renderer>().enabled = false;
			reset = true;
		}
	}
}
