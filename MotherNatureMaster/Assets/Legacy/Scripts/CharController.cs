using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {
	private Rigidbody rb;

	public float walkSpeed = 3.0f;
	private float walkIncrement = 0.0f;
	public float turnSpeed = 3.0f;

	public bool isGrounded;

	public float moveForce = 200f;

	public bool stop;
	public Transform checkFront;
	public Transform checkRight;
	public Transform checkLeft;
	public Transform checkBack;

	public bool collideFront;

	public float yRotCurrent;

	public Vector3 fVector = Vector3.zero;

	public float waitTime = 1.5f;

	public bool kidAlreadyWaiting;

	private float timer;

	private bool pause;
	private bool continueMove;

	void Awake () {
		checkFront = transform.Find ("CheckFront");
		checkRight = transform.Find ("CheckRight");
		checkLeft = transform.Find ("CheckLeft");
		checkBack = transform.Find ("CheckBack");
	}


	void Start () {
		rb = GetComponent<Rigidbody> ();

		yRotCurrent = transform.rotation.y;
		fVector = transform.forward;
	}
	

	void Update () {
		GroundCheck ();

		if (!stop)
			transform.rotation = Quaternion.Euler (0, yRotCurrent, 0);
	}


	void FixedUpdate() {
//		if (!stop) {
////			Debug.Log ("test2");
////			transform.rotation = Quaternion.Euler(new Vector3(0,yRotCurrent,0));
////			transform.forward = fVector;
////			rb.rotation = Quaternion.Euler(new Vector3(0,yRotCurrent,0));
//			rb.MoveRotation( Quaternion.Euler (new Vector3(0,yRotCurrent,0)));
//			MoveCheck();
//
//			if (rb.velocity.magnitude < walkSpeed) {
//				rb.AddForce (transform.forward * moveForce);
//			}
//			if (rb.velocity.magnitude > walkSpeed) {
//				rb.velocity = transform.forward * walkSpeed + new Vector3(0,rb.velocity.y,0);
//			}
//		} else if (stop) {
//			KidWait2 ();
//		}
//
//		if (Physics.Raycast (transform.position, transform.forward, checkFront.transform.localPosition.z)) {
//			collideFront = true;
//		}
//
//		Debug.DrawRay (checkRight.position, -transform.up * 1.5f, Color.green);
//
//		if (Input.GetKey(KeyCode.B))
//			Debug.Log (yRotCurrent);

		if (!stop) {
			MoveCheck2 ();

			if (rb.velocity.magnitude < walkSpeed) {
				rb.AddForce (transform.forward * moveForce);
			}
			if (rb.velocity.magnitude > walkSpeed) {
				rb.velocity = transform.forward * walkSpeed + new Vector3 (0, rb.velocity.y, 0);
			}
			Debug.Log ("blah");
		} else {

		}
	}

	void GroundCheck () {
		int playerLayerMask = 1 << 8;

		if (Physics.Raycast (transform.position, -transform.up, 1f, playerLayerMask))
			isGrounded = true;
		else
			isGrounded = false;
	}

	void MoveCheck () {

		int groundLayerMask = 1 << 9;
		RaycastHit hit;
		Debug.DrawRay (checkFront.position, -transform.up * 1.5f, Color.green);
		Debug.DrawRay (transform.position, transform.forward * checkFront.transform.localPosition.z, Color.red);

		if (!Physics.Raycast (checkFront.position, -transform.up, 1.5f, groundLayerMask) || collideFront) {

			stop = true;

			if (Physics.Raycast (checkRight.position, -transform.up, out hit, 1.5f, groundLayerMask)) {
				yRotCurrent += 90f;
				fVector = transform.right;
				ObstacleCheck(transform.right);

//				if (collideFront)
//					StartCoroutine (KidWait (waitTime));
//				else
//					StartCoroutine (KidWait (0f));
			} else if (Physics.Raycast (checkLeft.position, -transform.up, out hit, 1.5f, groundLayerMask)) {
				yRotCurrent -= 90f;
				fVector = -transform.right;
				ObstacleCheck(-transform.right);

//				if (collideFront)
//					StartCoroutine (KidWait (waitTime));
//				else
//					StartCoroutine (KidWait (0f));
			} else if (Physics.Raycast (checkBack.position, -transform.up, out hit, 1.5f, groundLayerMask)) {
				yRotCurrent += 180f;
				fVector = -transform.forward;
//				StartCoroutine (KidWait (waitTime));
				timer = Time.time + waitTime;
			} else {
				MoveCheck ();
			}
		}
	}

	void MoveCheck2() {
		int groundLayerMask = 1 << 9;

		if (Physics.Raycast (transform.position, transform.forward, 0.52f)) {
			stop = true;
			//Pause for a moment, check sides.
			SideCheckDelay(waitTime);
		} else if (!Physics.Raycast (checkFront.transform.position, -transform.up, 1.5f, groundLayerMask) || Physics.Raycast (transform.position, transform.forward, 0.52f)) {
			stop = true;
			//Check sides.
			SideCheck ();
		}
	}

	void SideCheck() {
		int groundLayerMask = 1 << 9;

		if (Physics.Raycast (transform.position, transform.right, 0.52f)) {
			//Turn right and pause for a moment. Then check again.
			yRotCurrent += 90f;
			transform.rotation = Quaternion.Euler(0,yRotCurrent,0);
			MoveCheckDelay(waitTime);
		} else if (Physics.Raycast (checkRight.transform.position, -transform.up, 1.5f, groundLayerMask)) {
			//Obstacle doesnt exist. Turn right, and continue moving.
			yRotCurrent += 90f;
			stop = false;
		} else if (Physics.Raycast (transform.position, -transform.right, 0.52f)) {
			//turn left and pause for a moment. Then check again.
			yRotCurrent -= 90f;
			transform.rotation = Quaternion.Euler(0,yRotCurrent,0);
			MoveCheckDelay(waitTime);
		} else if (Physics.Raycast (checkLeft.transform.position, -transform.up, 1.5f, groundLayerMask)) {
			//Obstacle doesn't exist. Turn left, then continue moving.
			yRotCurrent -= 90f;
			stop = false;
		} else if (Physics.Raycast (transform.position, -transform.forward, 0.52f)) {
			//Obstacle behind player, turn back and pause. then Check again.
			yRotCurrent += 180f;
			MoveCheckDelay(waitTime);
		} else if (Physics.Raycast (checkBack.transform.position, -transform.up, 1.5f, groundLayerMask)) {
			//Turn back, pause, and continue moving.
			yRotCurrent += 180f;
			SetStopDelay(waitTime);
		} else {
			MoveCheck2 ();
		}
	}

	IEnumerator SideCheckDelay (float delayTime) {
		yield return new WaitForSeconds (delayTime);
		SideCheck ();
	}

	IEnumerator MoveCheckDelay (float delayTime) {
		yield return new WaitForSeconds (delayTime);
		MoveCheck2 ();
	}

	IEnumerator SetStopDelay (float delayTime) {
		yield return new WaitForSeconds (delayTime);
		stop = false;
	}

	void ObstacleCheck(Vector3 dir) {
		Debug.DrawRay (transform.position, transform.position + (dir * 0.52f), Color.black);

		if (!kidAlreadyWaiting) {
			if (Physics.Raycast (transform.position, dir, 0.52f) || collideFront) {
//				StartCoroutine (KidWait (waitTime));
				timer = Time.time + waitTime;

			} else {
//				StartCoroutine (KidWait (0f));
				timer = Time.time;
			}
		}
	}

//	void OnCollisionEnter (Collision other) {
//
//		foreach (ContactPoint contact in other.contacts) {
//			Debug.DrawRay(contact.point, contact.normal * 10f, Color.red);
//
//			if (Vector3.Dot (transform.forward, -contact.normal) > 0.98f) {
//				collideFront = true;
//			}
//		}
//	}

	IEnumerator KidWait (float time) {
		Debug.Log ("called");
		kidAlreadyWaiting = true;

		yield return new WaitForSeconds (time);

		kidAlreadyWaiting = false;
		Debug.Log ("blah");
		rb.velocity = new Vector3(0, rb.velocity.y, 0);
		stop = false;
		collideFront = false;
		transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
//		transform.rotation = Quaternion.Euler(new Vector3(0,yRotCurrent,0));
	}

	void KidWait2 () {

		if (Time.time > timer) {
			kidAlreadyWaiting = false;
			Debug.Log ("blah");
			rb.velocity = new Vector3 (0, rb.velocity.y, 0);
			stop = false;
			collideFront = false;
			transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
		}
	}
}
