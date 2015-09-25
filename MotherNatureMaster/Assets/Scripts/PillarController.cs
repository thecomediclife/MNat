using UnityEngine;
using System.Collections;

public class PillarController : MonoBehaviour {
	public bool pillarEnabled = true;
	public bool inputEnabled = true;
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

	public enum Direction{Xdir, Ydir, Zdir};
	public Direction currentDirection = Direction.Ydir;

	private Transform[] nodeArray;
	private float previousHeight;
	private Direction previousDir;

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

		AdjustEnumDirection ();

		//Instantiate extra nodes if pillar is sideways.
		previousHeight = pillarHeight;
		nodeArray = new Transform[Mathf.FloorToInt(previousHeight)];
		if (currentDirection == Direction.Xdir || currentDirection == Direction.Zdir) {
			node.gameObject.SetActive(false);

			for (int i = 0; i < nodeArray.Length; i++) {
				nodeArray[i] = Instantiate(node, transform.position, Quaternion.identity) as Transform;
				nodeArray[i].parent = this.transform;
			}
		}

		previousDir = currentDirection;
	}

	void Start() {
		if (onStartGrown) {
			grow = true;
			pillarFinalPosition = transform.up * (pillarHeight - 0.5f);
			platform.position = transform.position + pillarFinalPosition;
		}
	}

	void Update() {
		AdjustEnumDirection ();

		FaceCollider ();
		AdjustTriggerCollider ();
		AdjustPillarCollider ();

		//Determines where the final position of the pillar should be. Can be changed dynamically.
		pillarFinalPosition = transform.up * (pillarHeight - 0.5f);

		//Determines when player has tapped on pillar
		if (mainCam.target == this.transform && mainCam.fingerDown && inputEnabled) {
			grow = !grow;

			if (currentDirection == Direction.Ydir) {
				AttachKid ();
			}
		}

		if (Vector3.Distance (platform.position, transform.position + pillarFinalPosition) < 0.1) {
			atMaxHeight = true;
			atGroundHeight = false;
		} else if (Vector3.Distance (platform.position, transform.position) < 0.1) {
			atMaxHeight = false;
			atGroundHeight = true;
		} else {
			atMaxHeight = false;
			atGroundHeight = false;
		}

		//Update nodes if the pillar has been rotated.
		if (previousDir != currentDirection) {
			node.gameObject.SetActive(false);
			
			for (int i = 0; i < nodeArray.Length; i++) {
				nodeArray[i] = Instantiate(node, transform.position, Quaternion.identity) as Transform;
				nodeArray[i].parent = this.transform;
			}
			previousDir = currentDirection;
		}

		//If pillar height was changed dynamically in game, new sideways nodes must be created.
		if (Mathf.FloorToInt(previousHeight) < Mathf.FloorToInt(pillarHeight)) {
			InstantiateExtraNodes();
		}

		//The below Switch Case affects how the nodes are used.
		if (currentDirection == Direction.Ydir) {
			inputEnabled = true;

			if (Vector3.Distance (platform.position, transform.position + pillarFinalPosition) < 0.1) {
				node.gameObject.SetActive(true);
			} else if (Vector3.Distance (platform.position, transform.position) < 0.1) {
				node.gameObject.SetActive(true);
			} else {
				node.gameObject.SetActive(false);
			}

		} else if (currentDirection == Direction.Xdir || currentDirection == Direction.Zdir) {
			node.gameObject.SetActive(false);

			if (Vector3.Distance (platform.position, transform.position) < 0.1) {
				for (int i = 0; i < nodeArray.Length; i++) {
					nodeArray[i].gameObject.SetActive(false);
				}
			} else {
				int index = Mathf.FloorToInt(Vector3.Distance(platform.position, transform.position));

				if (grow) {
					index -= 1;
					if (index > -1) {
						nodeArray[index].transform.position = transform.position + transform.up * (index * 1f + 1f) + new Vector3(0f,1f,0f);
						nodeArray[index].gameObject.SetActive(true);
					}
				} else {
					nodeArray[index].transform.position = transform.position;
					nodeArray[index].gameObject.SetActive(false);
				}

				if (Vector3.Distance (platform.position, transform.position + pillarFinalPosition) < 0.1) {
					nodeArray[nodeArray.Length - 1].transform.position = transform.position + transform.up * (nodeArray.Length * 1f) + new Vector3(0f,1f,0f);
					nodeArray[nodeArray.Length - 1].gameObject.SetActive(true);
				}
			}

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

	void AdjustEnumDirection() {
		Vector3 up = transform.up;
		up = new Vector3 (Mathf.Abs (up.x), Mathf.Abs (up.y), Mathf.Abs (up.z));
		if (up.x > 0f && up.y <= 0.1f && up.z <= 0.1f) {
			currentDirection = Direction.Xdir;
		} else if (up.y > 0f && up.x <= 0f && up.z <= 0.1f) {
			currentDirection = Direction.Ydir;
		} else if (up.z > 0f && up.x <= 0f && up.y <= 0.1f) {
			currentDirection = Direction.Zdir;
		} else {
			Debug.Log (this.name + " is not facing a uniform direction. " + transform.up);
		}
	}

	//Raises the pillar upwards
	public void Rise() {
        //ignore tree layer and node layer;
        //int layerMask = (1 << 10);
        //layerMask |= (1 << 11);
        //layerMask |= (1 << 14);
        //layerMask = ~layerMask;
        int layerMask = 1 << 12;

        if (!Physics.Raycast(platform.position + transform.up * 0.5f, transform.up, 0.05f, layerMask)) {
            platform.position = Vector3.MoveTowards(platform.position, transform.position + pillarFinalPosition, moveSpeed * Time.deltaTime);
        }

		Debug.DrawRay (platform.position + transform.up * 0.5f, transform.up * 0.05f, Color.green);

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

	}

	void AdjustPillarCollider() {
		//PillarCollider is used to block objects.
		//Adjust the size of the PillarCollider;
		pillarCollider.position = transform.position + transform.up * (Vector3.Magnitude (platform.localPosition) / 2f);
		pillarCollider.GetComponent<BoxCollider> ().size = new Vector3 (1f, Vector3.Magnitude (platform.localPosition) + 0.5f, 1f);
	}

	void AdjustTriggerCollider() {
		//Trigger collider is used to detect if kid is on top of the pillar.

		//Adjust the height of the Trigger detector to match the platform;
		switch (currentDirection) {
		case Direction.Ydir:

			this.GetComponent<BoxCollider> ().center = new Vector3 (0f, platform.localPosition.y + 0.4f, 0f);
			this.GetComponent<BoxCollider>().size = new Vector3(1f,0.2f,1f);

			break;

		case Direction.Xdir:

			this.GetComponent<BoxCollider> ().center = new Vector3(0f, platform.localPosition.y / 2f, 0f) - transform.up * 0.4f;
			this.GetComponent<BoxCollider>().size = new Vector3(0.2f,1f,1f) + new Vector3(0f,platform.localPosition.y,0f);

			break;

		case Direction.Zdir:

			this.GetComponent<BoxCollider> ().center = new Vector3(0f, platform.localPosition.y / 2f, 0f) - transform.up * 0.4f;
			this.GetComponent<BoxCollider>().size = new Vector3(1f,1f,0.2f) + new Vector3(0f,platform.localPosition.y,0f);

			break;
		}
	}

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

//	void OnTriggerEnter(Collider other) {
//		if (other.tag == "Kid") {
//			kidInRange = true;
//
//			if (currentDirection != Direction.Ydir) {
//				inputEnabled = false;
//			}
//		}
//	}
//
//	void OnTriggerExit(Collider other) {
//		if (other.tag == "Kid") {
//			kidInRange = false;
//
//			inputEnabled = true;
//		}
//	}

	void InstantiateExtraNodes() {
		Transform[] newNodeArray = new Transform[Mathf.FloorToInt (pillarHeight)];
		for (int i = 0; i < nodeArray.Length; i++) {
			if (nodeArray[i] != null) {
				newNodeArray[i] = nodeArray[i];
			} else {
				newNodeArray[i] = Instantiate(node, transform.position, Quaternion.identity) as Transform;
				newNodeArray[i].parent = this.transform;
			}
		}

		for (int i = Mathf.FloorToInt(previousHeight); i < newNodeArray.Length; i++) {
			newNodeArray[i] = Instantiate (node, transform.position, Quaternion.identity) as Transform;
			newNodeArray[i].parent = this.transform;
		}

		nodeArray = new Transform[Mathf.FloorToInt (pillarHeight)];

		for (int i = 0; i < nodeArray.Length; i++) {
			nodeArray[i] = newNodeArray[i];
		}

		previousHeight = pillarHeight;
	}
}
