using UnityEngine;
using System.Collections;

public class CharController3 : MonoBehaviour {
	public bool paused;

	public Transform checkFront;
	public Transform checkRight;
	public Transform checkLeft;
	public Transform checkBack;

	public float moveSpeed = 2;

	public Vector3[] endPosArray = new Vector3[3];
	public Vector3 endPos;
	private float[] addedAngle = new float[4];
	public float currentAngle;

	int counter = 0;
	int index;

	private int groundLayerMask = 1 << 9;

	Vector3 imposVec = new Vector3 (1000,0,0);

	void Awake () 
	{
		checkFront = transform.Find ("CheckFront");
		checkRight = transform.Find ("CheckRight");
		checkLeft = transform.Find ("CheckLeft");
		checkBack = transform.Find ("CheckBack");
	}

	void Start () 
	{
		endPos = transform.position;
		currentAngle = transform.rotation.y;
		addedAngle [0] = 0f;
		addedAngle [1] = 90f;
		addedAngle [2] = -90f;
		addedAngle [3] = 180f;
	}

	void Update () 
	{
		Debug.DrawRay (transform.position, transform.forward * 5f, Color.green);

		if (!paused) {

			RaycastHit hit;

			//	Check if front of child is available and has no obstacle
			if (Physics.Raycast (checkFront.position, -transform.up, out hit, 1f, groundLayerMask) && !Physics.Raycast (transform.position, transform.forward, 1f, 10) && Vector3.Distance (transform.position, endPos) < moveSpeed * Time.deltaTime) {
				endPosArray [0] = new Vector3 (Mathf.Round (hit.point.x), transform.position.y, Mathf.Round (hit.point.z));
			} else {
				endPosArray [0] = imposVec;
			}

			//	Check if right of child is available and has no obstacle
			if (Physics.Raycast (checkRight.position, -transform.up, out hit, 1f, groundLayerMask) && !Physics.Raycast (transform.position, transform.right, 1f, 10) && Vector3.Distance (transform.position, endPos) < moveSpeed * Time.deltaTime) {
				endPosArray [1] = new Vector3 (Mathf.Round (hit.point.x), transform.position.y, Mathf.Round (hit.point.z));
			} else {
				endPosArray [1] = imposVec;
			}

			//	Check if left of child is available and has no obstacle
			if (Physics.Raycast (checkLeft.position, -transform.up, out hit, 1f, groundLayerMask) && !Physics.Raycast (transform.position, -transform.right, 1f, 10) && Vector3.Distance (transform.position, endPos) < moveSpeed * Time.deltaTime) {
				endPosArray [2] = new Vector3 (Mathf.Round (hit.point.x), transform.position.y, Mathf.Round (hit.point.z));
			} else {
				endPosArray [2] = imposVec;
			}

			//	Move back is available if front/right/left have no ground or have obstacles
			if ((Physics.Raycast (checkBack.position, -transform.up, out hit, 1f, groundLayerMask) && !Physics.Raycast(transform.position, -transform.forward, 1f, 10)) && (!Physics.Raycast (checkFront.position, -transform.up, 1f, groundLayerMask) || Physics.Raycast (transform.position, transform.forward, 1f, 10)) && (!Physics.Raycast (checkRight.position, -transform.up, 1f, groundLayerMask) || Physics.Raycast (transform.position, transform.right, 1f, 10)) && (!Physics.Raycast (checkLeft.position, -transform.up, 1f, groundLayerMask) || Physics.Raycast (transform.position, -transform.right, 1f, 10)) && Vector3.Distance (transform.position, endPos) < moveSpeed * Time.deltaTime) {
				endPos = new Vector3 (Mathf.Round (hit.point.x), transform.position.y, Mathf.Round (hit.point.z));
				currentAngle += addedAngle [3];
				transform.rotation = Quaternion.Euler (new Vector3 (0f, currentAngle, 0f));
			}


			//	Movement function
			transform.position = Vector3.MoveTowards (transform.position, endPos, Time.deltaTime * moveSpeed);


			//	Look through array and find all available lanes detected by raycasts
			for (int i = 0; i < endPosArray.Length; i++) {
				if (endPosArray [i] != imposVec)
					counter++;
			}


			//	No lane found, do nothing
			if (counter == 0) {
			} 
			//	One land found, do function SimpleMove
			else if (counter == 1) {
				SimpleMove ();
				counter = 0;
			}
			//	More than one land found, do function RandomMove
			else {
				RandomMove ();
				counter = 0;
			}
		}
	}
	
	void SimpleMove () 
	{
		for (int i = 0; i < endPosArray.Length; i++) {
			if (endPosArray[i] != imposVec) {
				endPos = endPosArray[i];
				if (i != 0) {
					currentAngle += addedAngle[i];
					transform.rotation = Quaternion.Euler(new Vector3(0f, currentAngle, 0f));
				}
			}
		}
	}

	void RandomMove () 
	{
		while (endPos == transform.position || endPos == imposVec) {
			index = Random.Range (0, 3);
			endPos = endPosArray [index];
		}
		currentAngle += addedAngle [index];
		transform.rotation = Quaternion.Euler(new Vector3(0f, currentAngle, 0f));
	}

	public void PauseMove() {
		paused = true;
		transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
		endPos = new Vector3 (Mathf.Round (transform.position.x), Mathf.Round(transform.position.y), Mathf.Round (transform.position.z));
	}

	public void ContinueMove() {
		paused = false;
		transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
		endPos = new Vector3 (Mathf.Round (transform.position.x), Mathf.Round(transform.position.y), Mathf.Round (transform.position.z));
	}

	public void ReverseDirection () {
//		endPos = new Vector3 (Mathf.Round (transform.position.x), Mathf.Round(transform.position.y), Mathf.Round (transform.position.z));
//		transform.rotation = Quaternion.Euler (new Vector3 (0, 180, 0));
	}
}
