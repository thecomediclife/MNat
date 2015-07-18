using UnityEngine;
using System.Collections;

public class CharController6 : MonoBehaviour {
	public float speed = 2.0f;
	public float rotateSpeed = 10.0f;

	public Transform currentNode;
	public Transform nextNode;
	private Transform previousNode;
	private Transform nullNode = null;

	public bool nextNodeExists;
	public bool previousNodeExists;

	public Transform[] nodeArray = new Transform[8];
	
	void Start () {
		
	}

	void Update () {
		Debug.DrawRay (transform.position, transform.forward, Color.green);

		if (nextNode != nullNode) {
			transform.position = Vector3.MoveTowards (transform.position, nextNode.position, speed * Time.deltaTime);
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (nextNode.position - transform.position, new Vector3 (0, 1, 0)), rotateSpeed * Time.deltaTime);
		} else {
			FindNextNode ();
			RandomlySelectNode();
		}

		if (Vector3.Distance (transform.position, nextNode.position) < 0.05) {
			FindNextNode ();
			RandomlySelectNode ();
		}


		//	Gets from trigger
	}


	void FindNextNode() {
		previousNode = currentNode;
		currentNode = nextNode;
		nextNode = nullNode;

		nextNodeExists = false;
		previousNodeExists = false;

		for (int i = 0; i < nodeArray.Length; i++) {
			if (nodeArray[i] != nullNode && nodeArray[i] != previousNode && nodeArray[i] != currentNode) {
				nextNodeExists = true;
			}
			if (nodeArray[i] == previousNode) {
				previousNodeExists = true;
			}
		}
	}



	void RandomlySelectNode () {
		if (nextNodeExists) {
			while (nextNode == nullNode) {
				int randomIndex = Random.Range (0, nodeArray.Length);
				if (nodeArray [randomIndex] != previousNode && nodeArray [randomIndex] != currentNode)
					nextNode = nodeArray [randomIndex];
			}
		} else if (!nextNodeExists && previousNodeExists) {
			nextNode = previousNode;
		} else if (!nextNodeExists && !previousNodeExists) {
			nextNode = nullNode;
		}
	}





	void OnTriggerEnter(Collider other) {
		if (other.tag == "Node" && other.transform != currentNode) {
			for (int i = 0; i < nodeArray.Length; i++) {
				if (nodeArray[i] == nullNode) {
					nodeArray[i] = other.transform;
					break;
				}
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Node") {
			for (int i = 0; i < nodeArray.Length; i++) {
				if (other.transform == nodeArray[i]) {
					nodeArray[i] = nullNode;
					break;
				}
			}
		}
	}

	public void DisableNode(Transform nodeToDisable) {
		for (int i = 0; i < nodeArray.Length; i++) {
			if (nodeToDisable == nodeArray[i])
				nodeArray[i] = nullNode;
		}
	}
}
