using UnityEngine;
using System.Collections;

public class OrbitalCamera : MonoBehaviour {
	public Vector3 centerPoint = Vector3.zero;

	public float omega = 45f;
	public float phi = 45f;

	public float radius = 10f;

	public float xScrollSpeed = 100f;
	public float yScrollSpeed = 100f;

	private bool moveRight;
	private bool moveLeft;

	private float lastOmega;

	void Awake () {
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		omega += Input.GetAxis ("Horizontal") * xScrollSpeed * Time.deltaTime;
//		phi -= Input.GetAxis ("Vertical") * yScrollSpeed * Time.deltaTime;

		if (Input.GetKeyDown (KeyCode.RightArrow) && !moveLeft && !moveRight) {
			moveRight = true;
			lastOmega = omega;
		}
		
		if (Input.GetKeyDown (KeyCode.LeftArrow) && !moveLeft && !moveRight) {
			moveLeft = true;
			lastOmega = omega;
		}

		if (moveRight) {
			omega += xScrollSpeed * Time.deltaTime;
			if (omega > lastOmega + 90f) {
				omega = lastOmega + 90f;
				moveRight = false;
			}
		}

		if (moveLeft) {
			omega -= xScrollSpeed * Time.deltaTime;
			if (omega < lastOmega - 90f) {
				omega = lastOmega - 90f;
				moveLeft = false;
			}
		}

		if (phi > 175f) 
			phi = 175f;

		if (phi < 5f)
			phi = 5f;

		float x = radius * Mathf.Cos (Mathf.Deg2Rad * omega) * Mathf.Sin (Mathf.Deg2Rad * phi);
		float y = radius * Mathf.Cos (Mathf.Deg2Rad * phi);
		float z = radius * Mathf.Sin	 (Mathf.Deg2Rad * omega) * Mathf.Sin (Mathf.Deg2Rad * phi);
	
		transform.position = new Vector3 (x, y, z) + centerPoint;
		transform.rotation = Quaternion.LookRotation (centerPoint - transform.position);
	}
}
