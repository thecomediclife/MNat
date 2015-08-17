using UnityEngine;
using System.Collections;

public class HeavyObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody>().AddForce(0f,-100f,0f);
	}
}
