using UnityEngine;
using System.Collections;

public class SmartNodeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			other.GetComponent<CharController6>().Pause(transform, true, transform.position + new Vector3(0,0,-1));
		}
	}
}
