using UnityEngine;
using System.Collections;

public class CharController4 : MonoBehaviour {
	public float moveSpeed = 5.0f;
	private float percent = 0f;

	private Vector3 nextPoint = Vector3.zero;
	private Vector3 originalPos = Vector3.zero;
	private Vector3 moveTo = Vector3.zero;

	//private RaycastHit hit;
	private RaycastHit hit1;
	private RaycastHit hit2;
	private RaycastHit hit3;
	private RaycastHit hitNull;

	private bool noHit;
	private bool hit1Detected;
	private bool hit2Detected;
	private bool hit3Detected;

	private int groundLayerMask = 1 << 9;

	public bool pointChosen;
	private Vector3 tempPoint;

	// Use this for initialization
	void Start () {
		moveTo = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay (transform.position, transform.forward * 5f, Color.green);

		ForwardPathCheck ();

//		ForkCheck ();

		if (Vector3.Distance (transform.position, moveTo) < 0.01 && nextPoint != moveTo) {
			originalPos = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
			moveTo = nextPoint;
			percent = 0f;
		//	pointChosen = false;
		}

		percent += moveSpeed * Time.deltaTime;
		transform.position = Vector3.Lerp (originalPos, moveTo, percent);
	}

	void ForwardPathCheck() {
		Vector3 forwardPos = transform.position + transform.forward;
		forwardPos = new Vector3 (Mathf.Round (forwardPos.x), forwardPos.y, Mathf.Round (forwardPos.z));
		Vector3 rightPos = transform.position + transform.right;
		rightPos = new Vector3 (Mathf.Round (rightPos.x), rightPos.y, Mathf.Round (rightPos.z));
		Vector3 leftPos = transform.position - transform.right;
		leftPos = new Vector3 (Mathf.Round (leftPos.x), leftPos.y, Mathf.Round (leftPos.z));
		Debug.DrawRay (forwardPos, -transform.up * 1.5f, Color.red);
		Debug.DrawRay (rightPos, -transform.up * 1.5f, Color.red);
		Debug.DrawRay (leftPos, -transform.up * 1.5f, Color.red);

		int arraySize = 0;

		if (Physics.Raycast (forwardPos, -transform.up, out hit1, 1.5f, groundLayerMask)) {
			arraySize += 1;
		}
		if (Physics.Raycast (rightPos, -transform.up, out hit2, 1.5f, groundLayerMask)) {
			arraySize += 1;
		}
		if (Physics.Raycast (leftPos, -transform.up, out hit3, 1.5f, groundLayerMask)) {
			arraySize += 1;
		}

		Vector3[] hitPointArray = new Vector3[arraySize];

		if (arraySize > 0) {
			if (Physics.Raycast (forwardPos, -transform.up, out hit1, 1.5f, groundLayerMask)) {
				for (int i = 0; i < hitPointArray.Length; i++) {
					if (hitPointArray[i] == Vector3.zero)
						hitPointArray[i] = hit1.point;
				}
			}
			if (Physics.Raycast (rightPos, -transform.up, out hit2, 1.5f, groundLayerMask)) {
				for (int i = 0; i < hitPointArray.Length; i++) {
					if (hitPointArray[i] == Vector3.zero)
						hitPointArray[i] = hit2.point;
				}
			}
			if (Physics.Raycast (leftPos, -transform.up, out hit3, 1.5f, groundLayerMask)) {
				for (int i = 0; i < hitPointArray.Length; i++) {
					if (hitPointArray[i] == Vector3.zero)
						hitPointArray[i] = hit3.point;
				}
			}

			if (new Vector3(Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z)) != originalPos) {
				int randNum = Mathf.RoundToInt(Random.Range (0f, arraySize - 1f));
				nextPoint = hitPointArray[randNum] + new Vector3 (0f, GetComponent<Collider> ().bounds.size.y / 2f, 0f);
				Debug.Log ("test");
			}

/*			bool chosen = false;
			for (int i = 0; i < hitPointArray.Length; i++) {
				if (nextPoint == hitPointArray[i])
					chosen = true;
			}

			if (!chosen) {
				int randNum = Mathf.RoundToInt(Random.Range (0f, arraySize - 1f));
				nextPoint = hitPointArray[randNum] + new Vector3 (0f, GetComponent<Collider> ().bounds.size.y / 2f, 0f);
				Debug.Log (randNum);
			}*/

//			if (!pointChosen) {
//				int randNum = Mathf.RoundToInt(Random.Range (0f, arraySize - 1f));
//				Debug.Log (randNum);
//				nextPoint = hitPointArray[randNum] + new Vector3 (0f, GetComponent<Collider> ().bounds.size.y / 2f, 0f);
//				pointChosen = true;
//			}
//			nextPoint = hitPointArray[randNum] + new Vector3 (0f, GetComponent<Collider> ().bounds.size.y / 2f, 0f);
		}

/*		int arraySize = 0;
		Vector3[] hitArray = new Vector3[3];

		if (Physics.Raycast (forwardPos, -transform.up, out hit1, 1.5f, groundLayerMask)) {
			arraySize += 1;
			hitArray[0] = hit1.point;
		}
		if (Physics.Raycast (rightPos, -transform.up, out hit2, 1.5f, groundLayerMask)) {
			arraySize += 1;
			hitArray[1] = hit2.point;
		}
		if (Physics.Raycast (leftPos, -transform.up, out hit3, 1.5f, groundLayerMask)) {
			arraySize += 1;
			hitArray[2] = hit3.point;
		}

		if (arraySize > 0) {
			int[] validRays = new int[arraySize];

			if (Physics.Raycast (forwardPos, -transform.up, out hit1, 1.5f, groundLayerMask)) {
				for (int i = 0; i < validRays.Length; i++) {
					if (validRays [i] == 0)
						validRays [i] = 1;
				}
			}
			if (Physics.Raycast (rightPos, -transform.up, out hit2, 1.5f, groundLayerMask)) {
				for (int i = 0; i < validRays.Length; i++) {
					if (validRays [i] == 0)
						validRays [i] = 2;
				}
			}
			if (Physics.Raycast (leftPos, -transform.up, out hit3, 1.5f, groundLayerMask)) {
				for (int i = 0; i < validRays.Length; i++) {
					if (validRays [i] == 0)
						validRays [i] = 3;
				}
			}

			int randNum = Mathf.RoundToInt (Random.Range (0f, arraySize - 1));
			randNum = validRays[randNum];
			nextPoint = hitArray [randNum - 1] + new Vector3 (0f, GetComponent<Collider> ().bounds.size.y / 2f, 0f);
			Debug.Log (randNum - 1);
		} else {
			//no hits detected, turn around
		}*/

/*		if (Physics.Raycast (forwardPos, -transform.up, out hit1, 1.5f, groundLayerMask))
			hit1Detected = true;
		else 
			hit1Detected = false;
		if (Physics.Raycast (rightPos, -transform.up, out hit2, 1.5f, groundLayerMask))
			hit2Detected = true;
		else
			hit2Detected = false;
		if (Physics.Raycast (leftPos, -transform.up, out hit3, 1.5f, groundLayerMask))
			hit3Detected = true;
		else
			hit3Detected = false;

		if (!hit1Detected && !hit2Detected && !hit3Detected) {
			//no point was found.
			noHit = false;
		} else {
			while (noHit) {
				int randNum = Mathf.RoundToInt (Random.Range (1f,3f));
				if (randNum == 1 && !hit1Detected ) {
					noHit = true;
					nextPoint = hit1.point + new Vector3 (0f, GetComponent<Collider> ().bounds.size.y / 2f, 0f);
				} else if (randNum == 2 && !hit2Detected) {
					noHit = true;
					nextPoint = hit2.point + new Vector3 (0f, GetComponent<Collider> ().bounds.size.y / 2f, 0f);
				} else if (randNum == 3 && !hit3Detected) {
					noHit = true;
					nextPoint = hit3.point + new Vector3 (0f, GetComponent<Collider> ().bounds.size.y / 2f, 0f);
				} else {
					noHit = false;
				}
			}
		}

		//nextPoint = hit.point + new Vector3 (0f, GetComponent<Collider> ().bounds.size.y / 2f, 0f);
*/
/*		if (!Physics.Raycast (transform.position, transform.forward, 0.55f)) {
			if (Physics.Raycast (forwardPos, -transform.up, out hit, 1.5f, groundLayerMask)) {
				nextPoint = hit.point + new Vector3 (0f, GetComponent<Collider> ().bounds.size.y / 2f, 0f);
			}
		}*/
	}

	void ForkCheck() {
		Vector3 rightFork = transform.position + transform.forward + transform.right;
		rightFork = new Vector3 (Mathf.Round (rightFork.x), rightFork.y, Mathf.Round (rightFork.z));
		Vector3 forwardFork = transform.position + transform.forward + transform.forward;
		forwardFork = new Vector3 (Mathf.Round (forwardFork.x), forwardFork.y, Mathf.Round (forwardFork.z));
		Vector3 leftFork = transform.position + transform.forward - transform.right;
		leftFork = new Vector3 (Mathf.Round (leftFork.x), leftFork.y, Mathf.Round (leftFork.z));

//		if (
	}
}
