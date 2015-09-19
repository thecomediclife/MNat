using UnityEngine;
using System.Collections;

public class Level3GameController : MonoBehaviour {
	//THIS SCRIPT IS FOR USE ON LEVEL 3 (THE GATE) ONLY. IT PROVIDES NO OTHER PURPOSE THAN TO MAKE SURE LEVEL 3 WORKS AS INTENDED.

	private GameObject kid;
	public bool checkpoint1;

	public Transform gate; //the gate itself
	public Transform smartNodeForward, smartNodeLeft, smartNodeRight, smartNodeBack; //these nodes will tell the player which way to go.
	public Transform gateSmartNode; //this node is the one in front of the gate.
	public Transform middleNodeLookTarget; //this node is the one in the middle.

	public Transform gateKey1, gateKey2;
	public Transform key1, key2;

	public bool obtainedKey;
	public bool key1Placed, key2Placed = false;
	private bool key1Obtained, key2Obtained = false;

	//For gateNode
	private Vector3 lookAtPosition1 = new Vector3(-3f, 5f, -5f);
	private Vector3 lookAtPosition2 = new Vector3(-3f, 2f, 6f);

	//For middleNode
	private Vector3 lookAtPosition3 = new Vector3(9f,0f,-8f);
	
	void Start () {
		kid = GameObject.FindWithTag ("Kid");
		gateSmartNode.Find ("LookTarget").transform.localPosition = lookAtPosition1;
	}

	void Update () {

		//Kid has obtained key1
		if (Vector3.Distance (kid.transform.position, new Vector3 (-5f, 6f, -14f)) < 0.01 && !obtainedKey) {
			key1.parent = kid.transform;
			key1.localPosition = new Vector3(0f, 0f, 1f);
			obtainedKey = true;
			key1Obtained = true;
		}

		//Kid has obtained key2
		if (Vector3.Distance (kid.transform.position, new Vector3 (-9f, 0f, -8f)) < 0.01 && !obtainedKey) {
			key2.parent = kid.transform;
			key2.localPosition = new Vector3(0f, 0f, 1f);
			obtainedKey = true;
			key2Obtained = true;
			middleNodeLookTarget.transform.localPosition = lookAtPosition3;
		}

		//Kid is holding a key
		if (obtainedKey) {
			gateSmartNode.GetComponent<SmartNodeScript> ().triggerEnabled = false;

			if (!key1Placed) {
				smartNodeForward.gameObject.SetActive(false);
				smartNodeRight.gameObject.SetActive(false);
				smartNodeLeft.gameObject.SetActive(true);
				smartNodeBack.gameObject.SetActive(false);

				//Kid is placing key1
				if (Vector3.Distance(kid.transform.position, new Vector3(-17f, 1f, -15f)) < 0.01) {

					if (key1Obtained)
						key1.gameObject.SetActive(false);
					if (key2Obtained)
						key2.gameObject.SetActive(false);

					gateKey1.gameObject.SetActive(true);
					key1Placed = true;
					obtainedKey = false;

					smartNodeForward.gameObject.SetActive(false);
					smartNodeRight.gameObject.SetActive(false);
					smartNodeLeft.gameObject.SetActive(false);
					smartNodeBack.gameObject.SetActive(true);

					gateSmartNode.Find ("LookTarget").transform.localPosition = lookAtPosition2;
				}

			} else if (!key2Placed) {
				smartNodeForward.gameObject.SetActive(false);
				smartNodeRight.gameObject.SetActive(true);
				smartNodeLeft.gameObject.SetActive(false);
				smartNodeBack.gameObject.SetActive(false);

				//Kid is placing key2
				if (Vector3.Distance(kid.transform.position, new Vector3(-17f, 1f, -9f)) < 0.01) {

					if (key1Obtained)
						key1.gameObject.SetActive(false);
					if (key2Obtained)
						key2.gameObject.SetActive(false);

					gateKey2.gameObject.SetActive(true);
					key2Placed = true;
					obtainedKey = false;

					smartNodeForward.gameObject.SetActive(true);
					smartNodeRight.gameObject.SetActive(false);
					smartNodeLeft.gameObject.SetActive(false);
					smartNodeBack.gameObject.SetActive(false);
				}
			}

		} else {
			gateSmartNode.GetComponent<SmartNodeScript> ().triggerEnabled = true;
		}

		//Both Keys have been placed
		if (key1Placed && key2Placed && Vector3.Distance (kid.transform.position, new Vector3 (-17f, 1f, -12f)) < 0.01) {
			gate.GetComponent<Level3Gate>().open = true;
		}

	}
}
