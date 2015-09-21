using UnityEngine;
using System.Collections;

public class CrateScript : MonoBehaviour {
	//Crates need to block pathways.
	//kid needs to push blocks

	public bool pushF;
	public bool pushB;
	public bool pushL;
	public bool pushR;

	private int pillarLayerMask = 1 << 12;
	private int kidLayerMask = 1 << 13;

	private Transform[] nodeArray = new Transform[5];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay (transform.position, transform.forward * 0.75f, Color.green);
		Debug.DrawRay (transform.position, -transform.forward * 0.75f, Color.green);
		Debug.DrawRay (transform.position, transform.right * 0.75f, Color.green);
		Debug.DrawRay (transform.position, -transform.right * 0.75f, Color.green);

		this.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -100f, 0f));

		//PillarPushing
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, 0.77f, pillarLayerMask)) {
			pushF = true;

			Vector3 newPos = hit.point - transform.forward * 0.75f;
			transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
		} else {
			pushF = false;
		}

		if (Physics.Raycast(transform.position, -transform.forward, out hit, 0.77f, pillarLayerMask)) {
			pushB = true;

			Vector3 newPos = hit.point + transform.forward * 0.75f;
			transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
		} else {
			pushB = false;
		}

		if (Physics.Raycast(transform.position, transform.right, out hit, 0.77f, pillarLayerMask)) {
			pushR = true;

			Vector3 newPos = hit.point - transform.right * 0.75f;
			transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
		} else {
			pushR = false;
		}

		if (Physics.Raycast(transform.position, -transform.right, out hit, 0.77f, pillarLayerMask)) {
			pushL = true;

			Vector3 newPos = hit.point + transform.right * 0.75f;
			transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);;
		} else {
			pushL = false;
		}

		//KidPushing
		RaycastHit hit2;
		if (Physics.Raycast(transform.position, transform.forward, out hit2, 0.52f, kidLayerMask)) {
			pushF = true;
			
			Vector3 newPos = hit.point - transform.forward * 0.5f;
			transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
		} else {
			pushF = false;
		}
		
		if (Physics.Raycast(transform.position, -transform.forward, out hit2, 0.52f, kidLayerMask)) {
			pushB = true;
			
			Vector3 newPos = hit.point + transform.forward * 0.5f;
			transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
		} else {
			pushB = false;
		}
		
		if (Physics.Raycast(transform.position, transform.right, out hit2, 0.52f, kidLayerMask)) {
			pushR = true;
			
			Vector3 newPos = hit.point - transform.right * 0.5f;
			transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
		} else {
			pushR = false;
		}
		
		if (Physics.Raycast(transform.position, -transform.right, out hit2, 0.52f, kidLayerMask)) {
			pushL = true;
			
			Vector3 newPos = hit.point + transform.right * 0.5f;
			transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);;
		} else {
			pushL = false;
		}


		if (!pushF && !pushB && !pushL && !pushR) {
			Vector3 snapPos = transform.position * 2f;
			snapPos = new Vector3(Mathf.Round(snapPos.x), transform.position.y, Mathf.Round(snapPos.z));
			snapPos = snapPos / 2f;
			transform.position = new Vector3(snapPos.x, transform.position.y, snapPos.z);
		}

		//Clears out set inactive nodes once they're far enough.
		for (int i = 0; i < nodeArray.Length; i++) {
			if (nodeArray[i] != null) {
				if (Vector3.Distance(nodeArray[i].position, transform.position) > 0.75f) {
					nodeArray[i].gameObject.SetActive(true);
					nodeArray[i] = null;
				}
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Node") {
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
