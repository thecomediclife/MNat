using UnityEngine;
using System.Collections;

public class CrateScript : MonoBehaviour {
	//Crates need to block pathways.
	//kid needs to push blocks
	public Transform kid;
	public bool pushable;

	public bool pushF;
	public bool pushB;
	public bool pushL;
	public bool pushR;

	private int pillarLayerMask = 1 << 12;
	private int kidLayerMask = 1 << 13;

	private Transform[] nodeArray = new Transform[5];

	// Use this for initialization
	void Start () {
		kid = GameObject.Find ("Kid").transform;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay (transform.position, transform.forward * 0.75f, Color.green);
		Debug.DrawRay (transform.position, -transform.forward * 0.75f, Color.green);
		Debug.DrawRay (transform.position, transform.right * 0.75f, Color.green);
		Debug.DrawRay (transform.position, -transform.right * 0.75f, Color.green);

		this.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -100f, 0f));

		//PillarPushing
		float str = 0.8f;
		if (PillarRayCast (transform.forward, str)) {
			pushF = true;
		} else {
			pushF = false;
		}

		if (PillarRayCast (-transform.forward, str)) {
			pushB = true;
		} else {
			pushB = false;
		}

		if (PillarRayCast (transform.right, str)) {
			pushR = true;
		} else {
			pushR = false;
		}

		if (PillarRayCast (-transform.right, str)) {
			pushL = true;
		} else {
			pushL = false;
		}

		//KidPushing
		str = 0.55f;
		if (KidRayCast(transform.forward, str)) {
			pushF = true;
		} else {
			pushF = false;
		}

		if (KidRayCast(-transform.forward, str)) {
			pushB = true;
		} else {
			pushB = false;
		}

		if (KidRayCast(transform.right, str)) {
			pushR = true;
		} else {
			pushR = false;
		}

		if (KidRayCast(-transform.right, str)) {
			pushL = true;
		} else {
			pushL = false;
		}

		if (!pushF && !pushB && !pushL && !pushR) {

			Vector3 snapPos = transform.position * 2f;
			snapPos = new Vector3(Mathf.Round(snapPos.x), transform.position.y, Mathf.Round(snapPos.z));
			snapPos = snapPos / 2f;
			transform.position = Vector3.MoveTowards(transform.position,new Vector3(snapPos.x, transform.position.y, snapPos.z), 1.0f);

		}

		//Determines if kid can push the block
		if ((Mathf.Abs(transform.position.x - kid.transform.position.x) < 0.1f || Mathf.Abs (transform.position.z - kid.transform.position.z) < 0.1f) && Vector3.Distance(transform.position, kid.transform.position) < 2f) {

			//ignore tree layer and node layer;
			int layerMask = (1 << 10);
			layerMask |= (1 << 11);
			layerMask = ~layerMask;

			RaycastHit hit3;
			Debug.DrawRay(transform.position, kid.transform.forward * 1f, Color.blue);
			if (Physics.Raycast(transform.position, kid.transform.forward, out hit3, 1f, layerMask)) {
				pushable = false;
			} else {
				pushable = true;
			}

		}

		//Clears out set inactive nodes once they're far enough.
		if (!pushable) {
			for (int i = 0; i < nodeArray.Length; i++) {
				if (nodeArray [i] != null) {
					if (Vector3.Distance (nodeArray [i].position, transform.position) > 0.75f) {
						nodeArray [i].gameObject.SetActive (true);
						nodeArray [i] = null;
					}
				}
			}
		} else {
			for (int i = 0; i < nodeArray.Length; i++) {
				if (nodeArray [i] != null) {
					nodeArray [i].gameObject.SetActive (true);
					nodeArray [i] = null;
				}
			}
		}
	}


	void OnTriggerStay(Collider other) {
		if (other.tag == "Node") {
			if (!pushable) {
				if (Vector3.Distance(other.transform.position, transform.position) < 0.75f) {
					other.transform.gameObject.SetActive(false);
					for (int i = 0; i < nodeArray.Length; i++) {
						if (nodeArray[i] == null) {
							nodeArray[i] = other.transform;
							break;
						}
					}
				}
			}
		}
	}

	bool PillarRayCast(Vector3 direction, float strength) {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, strength, pillarLayerMask)) {

			PillarController pillCont = hit.transform.parent.GetComponent<PillarController>();

			if (pillCont.grow && pillCont.currentDirection != PillarController.Direction.Ydir) {
				
				Vector3 newPos = hit.point - direction * 0.75f;
				transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);

				return true;

			}

			return false;
		} else {

			return false;
		}
	}

	bool KidRayCast(Vector3 direction, float strength) {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, strength, kidLayerMask)) {
				
			Vector3 newPos = hit.point - direction * 0.5f;
			transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
			
			return true;
				
		} else {
			
			return false;
		}
	}
}
