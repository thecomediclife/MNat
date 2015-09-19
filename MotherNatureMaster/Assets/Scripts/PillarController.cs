using UnityEngine;
using System.Collections;

public class PillarController : MonoBehaviour {
	public bool pillarEnabled = true;
	public bool onStartGrown;
	public bool grow;
	public bool kidInRange;
	public bool kidAttached;

	private TouchController5 mainCam;
	public Transform kid;
	private Transform platform, touchCollider, node, pillarCollider;

	public float pillarHeight = 4f;
	public float moveSpeed = 2f;
	private Vector3 pillarFinalPosition;

	public bool atMaxHeight;
	public bool atGroundHeight;

	void Awake() {
		mainCam = Camera.main.GetComponent<TouchController5> ();
		kid = GameObject.Find ("Kid").transform;

		//Pillar is ALWAYS first child of the parent.
		platform = transform.GetChild (0);
		//TouchCollider is Always the second child of the parent.
		touchCollider = transform.GetChild (1);
		//Node is always first child of the first child.
		node = transform.GetChild (0).transform.GetChild (0);
		//Get PillarColliderchild
		pillarCollider = transform.GetChild (2);
	}

	void Start() {
		if (onStartGrown) {
			grow = true;
			pillarFinalPosition = transform.up * (pillarHeight - 0.5f);
			platform.position = transform.position + pillarFinalPosition;
		}
	}

	void Update() {
//		DetermineKidDistance ();
		FaceCollider ();

		//Determines where the final position of the pillar should be. Can be changed dynamically.
		pillarFinalPosition = transform.up * (pillarHeight - 0.5f);

		//Determines when player has tapped on pillar
		if (mainCam.target == this.transform  && mainCam.fingerDown) {
			grow = !grow;
			AttachKid();
		}

		if (Vector3.Distance (platform.position, transform.position + pillarFinalPosition) < 0.1) {
			atMaxHeight = true;
			atGroundHeight = false;
			node.gameObject.SetActive(true);
		} else if (Vector3.Distance (platform.position, transform.position) < 0.1) {
			atMaxHeight = false;
			atGroundHeight = true;
			node.gameObject.SetActive(true);
		} else {
			atMaxHeight = false;
			atGroundHeight = false;
			node.gameObject.SetActive(false);
		}
	}

	void FixedUpdate() {
		if (pillarEnabled) {
			if (grow) {
				Rise ();
			} else if (!grow) {
				Fall ();
			}
		}
	}

	//Raises the pillar upwards
	public void Rise() {
		platform.position = Vector3.MoveTowards (platform.position, transform.position + pillarFinalPosition, moveSpeed * Time.deltaTime);

		if (Vector3.Distance (platform.position, transform.position + pillarFinalPosition) < 0.1 && kidAttached) {
			DetachKid();
		}
	}

	//Lowers the pillar downwards
	public void Fall() {
		platform.localPosition = Vector3.MoveTowards (platform.localPosition, Vector3.zero, moveSpeed * Time.deltaTime);

		if (Vector3.Distance (platform.position, transform.position) < 0.1 && kidAttached) {
			DetachKid();
		}
	}

	//Faces the TouchCollider to always match the camera direction as well as scale the collider to match the size of the pillar.
	void FaceCollider() {
		Vector3 camForward = Camera.main.transform.forward;

		if (transform.up == new Vector3 (0f, 1f, 0f) || transform.up == new Vector3 (0f, -1f, 0f)) {
//			Debug.Log ("y");
			camForward = new Vector3 (camForward.x, 0f, camForward.z);
		} else if (transform.up == new Vector3 (1f, 0f, 0f) || transform.up == new Vector3 (-1f, 0f, 0f)) {
//			Debug.Log ("x");
			camForward = new Vector3 (0f, camForward.y, camForward.z);
		} else if (transform.up == new Vector3 (0f, 0f, 1f) || transform.up == new Vector3 (0f, 0f, -1f)) {
//			Debug.Log ("z");
			camForward = new Vector3 (camForward.x, camForward.y, 0f);
		} else {
			Debug.Log ("pillar isn't facing any uniform direction.");
		}

		touchCollider.rotation = Quaternion.LookRotation (camForward, transform.up);

		touchCollider.position = transform.position + transform.up * (Vector3.Magnitude (platform.localPosition) / 2f + 0.5f);
		touchCollider.GetComponent<BoxCollider> ().size = new Vector3 (2f, Vector3.Magnitude (platform.localPosition) + 2.5f, 0.25f);

		//Adjust the height of the Trigger detector to match the platform;
		this.GetComponent<BoxCollider> ().center = new Vector3 (0f, platform.localPosition.y + 0.35f, 0f);

		//Adjust the size of the PillarCollider;
		pillarCollider.position = transform.position + transform.up * (Vector3.Magnitude (platform.localPosition) / 2f);
		pillarCollider.GetComponent<BoxCollider> ().size = new Vector3 (1f, Vector3.Magnitude (platform.localPosition) + 0.5f, 1f);
	}

//	void DetermineKidDistance() {
//		//	Debug.Log (Vector3.Distance (kid.transform.position, transform.position) + " " + transform.name);
//		
//		if (Mathf.Abs(node.position.y - kid.transform.position.y) < 0.1f && Vector3.Distance (kid.transform.position, node.position) < 1.0f) {
//			kidInRange = true;
//		} else {
//			kidInRange = false;
//		}
//	}

	void AttachKid() {
		if (kidInRange && !kidAttached) {
			kid.GetComponent<CharController6> ().Pause (node, false, null);
			kid.transform.parent = platform.transform;
			kidAttached = true;
		}
	}

	void DetachKid() {
		if (kidAttached) {
			kid.GetComponent<CharController6> ().Continue (false, null);
			kid.transform.parent = null;
			kidAttached = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Kid") {
			kidInRange = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Kid") {
			kidInRange = false;
		}
	}
}
