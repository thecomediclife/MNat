using UnityEngine;
using System.Collections;

public class CharController2 : MonoBehaviour {
	private Rigidbody rb;

	public float walkSpeed = 1.0f;
	public float turnSpeed = 10.0f;

	public float moveForce = 200f;

	public bool stop;
	private bool pause;
	public Transform checkFront;
	public Transform checkRight;
	public Transform checkLeft;
	public Transform checkBack;

	public float waitTime = 1.5f;
	private float timer;

	public float yCurrentRot;
	public float ySetRot;

	private bool rotating;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody> ();
		checkFront = transform.Find ("CheckFront");
		checkRight = transform.Find ("CheckRight");
		checkLeft = transform.Find ("CheckLeft");
		checkBack = transform.Find ("CheckBack");
		yCurrentRot = transform.rotation.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!stop) {
			MoveCheck ();
			rb.rotation = (Quaternion.Euler (new Vector3 (0f, yCurrentRot, 0f)));

			if (rb.velocity.magnitude < walkSpeed) {
				rb.AddForce (transform.forward * moveForce);
			}
			if (rb.velocity.magnitude > walkSpeed) {
				rb.velocity = transform.forward * walkSpeed + new Vector3 (0, rb.velocity.y, 0);
			}
		} else if (stop && !pause) {
			rb.velocity = new Vector3 (0f, rb.velocity.y, 0f);
			rb.MovePosition (new Vector3 (Mathf.Round (rb.position.x), rb.position.y, Mathf.Round (rb.position.z)));

			if (Time.time > timer) {
				if (!rotating) {
					yCurrentRot += ySetRot;
					if (yCurrentRot < 360f)
						yCurrentRot += 360f;
					if (yCurrentRot > 360f)
						yCurrentRot -= 360f;
					rotating = true;
				} else if (rotating) {
					rb.rotation = (Quaternion.Euler (new Vector3 (0f, yCurrentRot, 0f)));
					if (transform.rotation.eulerAngles.y - yCurrentRot < 0.1f) {
						stop = false;
						rotating = false;
					}
				}
			}
		} else if (stop && pause) {
			//Player shouldn't move here.
		}
		rb.angularVelocity = new Vector3 (0f, 0f, 0f);
	}

	void MoveCheck() {
		int groundLayerMask = 1 << 9;
		int ignorePlayer = ~(1 << 8);
		//Debug.DrawRay (checkFront.position, -transform.up * 1.5f, Color.green);
		//Debug.DrawRay (transform.position, transform.forward * 0.52f, Color.green);

		if (!Physics.Raycast (checkFront.position, -transform.up, 1.5f, groundLayerMask) || Physics.Raycast (transform.position, transform.forward, 0.55f, ignorePlayer)) {
			//Raycast has found something blocking movement, or has no viable ground in front of player
			//Debug.Log ("Can't move forward");
			stop = true;
			if (!Physics.Raycast (checkRight.position, -transform.up, 1.5f, groundLayerMask) || Physics.Raycast (transform.position, transform.right, 0.55f, ignorePlayer)) {
				if (!Physics.Raycast (checkLeft.position, -transform.up, 1.5f, groundLayerMask) || Physics.Raycast (transform.position, -transform.right, 0.55f, ignorePlayer)) {
					if (!Physics.Raycast (checkBack.position, -transform.up, 1.5f, groundLayerMask) || Physics.Raycast (transform.position, -transform.forward, 0.55f, ignorePlayer)) {
						//all has failed
						Debug.Log ("all has failed");
					} else {
						ySetRot = 180f;
						timer = Time.time + waitTime;
						//Debug.Log ("go back");
					}
				} else {
					ySetRot = -90f;
					timer = Time.time + 0f;
					//Debug.Log ("go left");
				}
			} else {
				ySetRot = 90f;
				timer = Time.time + 0f;
				//Debug.Log("go right");
			}
		} else {
			stop = false;
		}
	}

	void MoveCheckNoDelay() {
		int groundLayerMask = 1 << 9;
		int ignorePlayer = ~(1 << 8);
		//Debug.DrawRay (checkFront.position, -transform.up * 1.5f, Color.green);
		//Debug.DrawRay (transform.position, transform.forward * 0.52f, Color.green);
		
		if (!Physics.Raycast (checkFront.position, -transform.up, 1.5f, groundLayerMask) || Physics.Raycast (transform.position, transform.forward, 0.55f, ignorePlayer)) {
			//Raycast has found something blocking movement, or has no viable ground in front of player
			//Debug.Log ("Can't move forward");
			stop = true;
			if (!Physics.Raycast (checkRight.position, -transform.up, 1.5f, groundLayerMask) || Physics.Raycast (transform.position, transform.right, 0.55f, ignorePlayer)) {
				if (!Physics.Raycast (checkLeft.position, -transform.up, 1.5f, groundLayerMask) || Physics.Raycast (transform.position, -transform.right, 0.55f, ignorePlayer)) {
					if (!Physics.Raycast (checkBack.position, -transform.up, 1.5f, groundLayerMask) || Physics.Raycast (transform.position, -transform.forward, 0.55f, ignorePlayer)) {
						//all has failed
						Debug.Log ("all has failed");
					} else {
						ySetRot = 180f;
						timer = Time.time + 0f;
						//Debug.Log ("go back");
					}
				} else {
					ySetRot = -90f;
					timer = Time.time + 0f;
					//Debug.Log ("go left");
				}
			} else {
				ySetRot = 90f;
				timer = Time.time + 0f;
				//Debug.Log("go right");
			}
		} else {
			stop = false;
		}
	}

	public void OnElevator(Vector3 snapToVector) {
		pause = true;
		rb.velocity = new Vector3 (0, rb.velocity.y, 0);
		rb.MovePosition (snapToVector);
		rb.MoveRotation (Quaternion.Euler (new Vector3 (0, yCurrentRot, 0)));
	}

	public void OffElevator() {
		pause = false;
		MoveCheckNoDelay ();
	}
}
