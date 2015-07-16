using UnityEngine;
using System.Collections;

public class CharControllerTest : MonoBehaviour {
	public bool paused;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!paused) {
			transform.position += new Vector3(0,0,1f * Time.deltaTime);
		}
	}

	public void PauseMove() {
		paused = true;
		transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
	}

	public void ContinueMove() {
		paused = false;
		transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
	}
}
