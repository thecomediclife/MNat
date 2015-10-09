using UnityEngine;
using System.Collections;

public class OrbitalCamera : MonoBehaviour {
	public bool allowRotate = false;
    public bool panningCamera = false;
	public Vector3 centerPoint = Vector3.zero;

	public float omega = 45f;
	public float phi = 45f;

	public float radius = 10f;

	public float xScrollSpeed = 100f;
	public float yScrollSpeed = 100f;

	private bool moveRight;
	private bool moveLeft;

	private float lastOmega;

	public float panningCamSpeed = 8f;

	public Transform centerP;

	private Vector3 addedVec;

	void Awake() {
		centerP.position = centerPoint;
	
	}

	// Update is called once per frame
	void Update () {
//		omega += Input.GetAxis ("Horizontal") * xScrollSpeed * Time.deltaTime;
//		phi -= Input.GetAxis ("Vertical") * yScrollSpeed * Time.deltaTime;
		if (allowRotate) {
			if (Input.GetKeyDown (KeyCode.RightArrow) && !moveLeft && !moveRight) {
				moveRight = true;
				lastOmega = omega;
			}
		
			if (Input.GetKeyDown (KeyCode.LeftArrow) && !moveLeft && !moveRight) {
				moveLeft = true;
				lastOmega = omega;
			}
		}

        if (panningCamera)
        {
			centerPoint += Input.GetAxis("Horizontal") * panningCamSpeed * Time.deltaTime * transform.right;
			centerPoint += Input.GetAxis("Vertical") * panningCamSpeed * Time.deltaTime * transform.up;

            if (Input.GetKey(KeyCode.Q))
            {
				this.GetComponent<Camera>().orthographicSize -= panningCamSpeed * Time.deltaTime;
            } else if (Input.GetKey(KeyCode.E))
            {
				this.GetComponent<Camera>().orthographicSize += panningCamSpeed * Time.deltaTime;
            }
            //this.GetComponent<Camera>().orthographicSize -= Input.GetAxis("Vertical") * 0.25f;
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
	
		//transform.position = new Vector3 (x, y, z) + centerPoint;
		addedVec = new Vector3 (x, y, z);
		centerP.position = centerPoint;
		transform.rotation = Quaternion.LookRotation (centerPoint - transform.position);
	}

	void LateUpdate() {
		transform.position = addedVec + centerP.position;
	}
}
