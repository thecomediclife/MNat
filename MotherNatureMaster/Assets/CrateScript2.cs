using UnityEngine;
using System.Collections;

public class CrateScript2 : MonoBehaviour {
	//Order of logic.
	//1. Check if floor is below. If no floor, crate falls.
	//2. If there is floor, crate checks if there are obstructions in its path. If there is an obstruction, it can't be pushed in that direction (For kid only)
	//3. if there is no obstruction in its path, check if any pillars or kids are touching the crate.
	//	A. If pillar is touching crate, check if pillar is Growing and not at max height (therefore pushing the crate).
	//	B. If kid is pushing crate, checks if it is main character or not.
	//4. If conditions for (3) are met, crate parents to pillar or kid.
	//5. As it is being pushed, it checks for obstructions in the direction it is being pushed.
	//6. If there is an obstruction, crate orphans itself (de-parenting from parent) and disables the node it is touching (blocks pathways).
	//7. Reset to (2). (1) plays at all times, regardless of steps (2) through (6). This ensures that the crate prioritizes falling over being pushed.

	public bool fall = false;

	public float fallSpeed = 10f;

	public bool attached = false;

	enum ParentedObj{Pillar, Kid, None};

	ParentedObj parent = ParentedObj.None;
	public Transform parentTransform;

//	public bool pushable;
//	public Transform[] nodeArray = new Transform[5];
	public Transform kid;

	//if true, then that direction is blocked
	public bool blockFor = false, blockBack = false, blockRight = false, blockLeft = false;

	public enum PushDirection{Forward, Backward, Right, Left, None}

	public PushDirection pushDir = PushDirection.None;

	public Transform node;

	// Use this for initialization
	void Start () {
		kid = GameObject.Find ("Kid").transform;
	}
	
	// Update is called once per frame
	void Update () {
		CheckBlockage ();

		Debug.DrawRay (transform.position + transform.forward * 0.35f, new Vector3 (0f, -0.55f, 0f), Color.green);
		Debug.DrawRay (transform.position - transform.forward * 0.35f, new Vector3 (0f, -0.55f, 0f), Color.green);
		Debug.DrawRay (transform.position + transform.right * 0.35f, new Vector3 (0f, -0.55f, 0f), Color.green);
		Debug.DrawRay (transform.position - transform.right * 0.35f, new Vector3 (0f, -0.55f, 0f), Color.green);

		transform.rotation = Quaternion.identity;

		//ignore tree layer and node layer;
		int fallLayerMask = (1 << 10);
		fallLayerMask |= (1 << 11);
		fallLayerMask = ~fallLayerMask;

		if (!Physics.Raycast (transform.position + transform.forward * 0.35f, new Vector3 (0f, -1f, 0f), 0.55f, fallLayerMask) &&
			!Physics.Raycast (transform.position - transform.forward * 0.35f, new Vector3 (0f, -1f, 0f), 0.55f, fallLayerMask) &&
			!Physics.Raycast (transform.position + transform.right * 0.35f, new Vector3 (0f, -1f, 0f), 0.55f, fallLayerMask) &&
			!Physics.Raycast (transform.position - transform.right * 0.35f, new Vector3 (0f, -1f, 0f), 0.55f, fallLayerMask)) {

			fall = true;

			Orphanize();
		} else {

			fall = false;
		
		}

		if (fall) {
			transform.position -= new Vector3 (0f, fallSpeed * Time.deltaTime, 0f);
		} else {

			Debug.DrawRay(transform.position, transform.forward * 0.6f, Color.red);
			Debug.DrawRay(transform.position, -transform.forward * 0.6f, Color.red);
			Debug.DrawRay(transform.position, transform.right * 0.6f, Color.red);
			Debug.DrawRay(transform.position, -transform.right * 0.6f, Color.red);

			RaycastHit hit1, hit2, hit3, hit4;

			//Ignores everything but pillar layer.
			int newLayerMask = 1 << 12;
			newLayerMask |= 1 << 13;

			Physics.Raycast(transform.position, transform.forward, out hit1, 0.55f, newLayerMask);
			Physics.Raycast(transform.position, -transform.forward, out hit2, 0.55f, newLayerMask);
			Physics.Raycast(transform.position, transform.right, out hit3, 0.55f, newLayerMask);
			Physics.Raycast(transform.position, -transform.right, out hit4, 0.55f, newLayerMask);

			if ((blockBack && hit1.transform != null) ||
			    (blockFor && hit2.transform != null) ||
			    (blockLeft && hit3.transform != null) ||
			    (blockRight && hit4.transform != null)) {
				DisableNodes();
				Orphanize();
			} else {
				EnableNodes();
			}

			if (!blockBack && hit1.transform != null) {
				ParentToHitTarget(hit1.transform, PushDirection.Forward);
			}
			if (!blockFor && hit2.transform != null) {
				ParentToHitTarget(hit2.transform, PushDirection.Backward);
			}
			if (!blockLeft && hit3.transform != null) {
				ParentToHitTarget(hit3.transform, PushDirection.Right);
			}
			if (!blockRight && hit4.transform != null) {
				ParentToHitTarget(hit4.transform, PushDirection.Left);
			}

//			if (kid.GetComponent<CharController6>().nextNode == node) {
//				if (DetermineKidForwardDirection() != Vector3.zero) {
//					if (DetermineKidForwardDirection() == new Vector3(0f,0f,1f) && !blockFor) {
//						ParentToHitTarget(kid, PushDirection.Backward);
//					} else if (DetermineKidForwardDirection() == new Vector3(0f,0f,-1f) && !blockBack) {
//						ParentToHitTarget(kid, PushDirection.Forward);
//					} else if (DetermineKidForwardDirection() == new Vector3(1f,0f,0f) && !blockRight) {
//						ParentToHitTarget(kid, PushDirection.Left);
//					} else if (DetermineKidForwardDirection() == new Vector3(-1f,0f,0f) && !blockLeft) {
//						ParentToHitTarget(kid, PushDirection.Right);
//					}
//				}
//			}

			switch (parent) {
			case ParentedObj.None:

				break;

			case ParentedObj.Kid:

//				transform.localPosition = Vector3.MoveTowards(transform.localPosition, DetermineKidForwardDirection() * -1f, 20f * Time.deltaTime);
//				Debug.Log (DetermineKidForwardDirection());


//				if (Vector3.Angle(parentTransform.forward, new Vector3(transform.position.x, 0f, transform.position.z) - new Vector3(parentTransform.position.x, 0f, parentTransform.position.z)) > 2.5f) {
//					Orphanize();
//				}

//				if (kid.GetComponent<CharController6>().nextNode != node) {
//					Orphanize();
//					Debug.Log ("test");
//				}

				switch (pushDir) {
				case PushDirection.None:

					Orphanize();

					break;

				case PushDirection.Forward:

					if (DetermineKidForwardDirection() != new Vector3(0f, 0f, -1f)) {
						Orphanize();
					}


					break;

				case PushDirection.Backward:

					if (DetermineKidForwardDirection() != new Vector3(0f, 0f, 1f)) {
						Orphanize();
					}

					break;

				case PushDirection.Right:

					if (DetermineKidForwardDirection() != new Vector3(-1f, 0f, 0f)) {
						Orphanize();
					}

					break;

				case PushDirection.Left:

					if (DetermineKidForwardDirection() != new Vector3(1f, 0f, 0f)) {
						Orphanize();
					}

					break;
				}

				break;

			case ParentedObj.Pillar:

				if (!parentTransform.parent.GetComponent<PillarController>().grow || parentTransform.parent.GetComponent<PillarController>().atMaxHeight) {
					Orphanize();
				}

				break;
			}

			//Check if there is a blockage and de-parent.
			switch (pushDir) {
			case PushDirection.None:

				//EnableNodes();

				break;

			case PushDirection.Forward:

				if (blockBack) {
					Orphanize();
					DisableNodes();
				} else {
					EnableNodes();
				}

				break;

			case PushDirection.Backward:

				if (blockFor) {
					Orphanize();
					DisableNodes();
				} else {
					EnableNodes();
				}

				break;

			case PushDirection.Right:

				if (blockLeft) {
					Orphanize();
					DisableNodes();
				} else {
					EnableNodes();
				}

				break;

			case PushDirection.Left:

				if (blockRight) {
					Orphanize();
					DisableNodes();
				} else {
					EnableNodes();
				}

				break;
			}

			if (!attached) {
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z)), 5f * Time.deltaTime);
			}
		}

	}

	void CheckBlockage() {
		//ignore tree layer and node layer;
		int fallLayerMask = (1 << 10);
		fallLayerMask |= (1 << 11);
		fallLayerMask = ~fallLayerMask;

		if (Physics.Raycast (transform.position, transform.forward, 0.6f, fallLayerMask)) {
			blockFor = true;
		} else {
			blockFor = false;
		}

		if (Physics.Raycast (transform.position, -transform.forward, 0.6f, fallLayerMask)) {
			blockBack = true;
		} else {
			blockBack = false;
		}

		if (Physics.Raycast (transform.position, transform.right, 0.6f, fallLayerMask)) {
			blockRight = true;
		} else {
			blockRight = false;
		}

		if (Physics.Raycast (transform.position, -transform.right, 0.6f, fallLayerMask)) {
			blockLeft = true;
		} else {
			blockLeft = false;
		}
	}

	void ParentToHitTarget(Transform hit, PushDirection pushdirec) {
		if (hit.transform != null) {
			if (hit.transform.tag == "Tree") {
				if (Vector3.Distance(Vector3.Normalize(transform.position - hit.transform.parent.transform.position), hit.transform.parent.up) < 0.1f) {

					if (hit.transform.parent.GetComponent<PillarController>().grow && !hit.transform.parent.GetComponent<PillarController>().atMaxHeight) {
						this.transform.parent = hit.transform.parent.transform.GetChild(0);
						parent = ParentedObj.Pillar;
						parentTransform = this.transform.parent;
						attached = true;
						pushDir = pushdirec;
					}

				}

			} else if (hit.transform.tag == "Kid") {
//				Debug.Log (hit.transform.name);
//				if (hit.transform.GetComponent<CharController6>().channel == 0) {

					//if (hit.transform.GetComponent<CharController6>().nextNode == node) {
//					if (Vector3.Angle(hit.transform.forward, new Vector3(transform.position.x, 0f, transform.position.z) - new Vector3(hit.transform.position.x, 0f, hit.transform.position.z)) < 2.5f) {

						this.transform.parent = hit.transform.GetChild(1);
						parent = ParentedObj.Kid;
						parentTransform = this.transform.parent;
						attached = true;
						pushDir = pushdirec;
//					}

//				}
				
			}
		}

	}


	void DisableNodes() {

		if (node != null)
			node.gameObject.SetActive (false);

	}

	void EnableNodes() {

		if (node != null)
			node.gameObject.SetActive (true);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Node") {
			node = other.transform;
		}

		if (fall) {
			if (other.tag == "Kid") {
				other.GetComponent<CharController6>().Respawn();
			}
		}
	}

	void Orphanize() {
		this.transform.parent = null;
		attached = false;
		parent = ParentedObj.None;
		pushDir = PushDirection.None;
		parentTransform = null;
	}

	Vector3 DetermineKidForwardDirection() {
		Vector3 nextNodePos = kid.GetComponent<CharController6>().nextNode.position;
		nextNodePos = new Vector3(nextNodePos.x, 0f, nextNodePos.z);
		Vector3 currentNodePos = kid.GetComponent<CharController6>().currentNode.position;
		currentNodePos = new Vector3(currentNodePos.x, 0f, currentNodePos.z);

		return Vector3.Normalize (nextNodePos - currentNodePos);
	}
}
