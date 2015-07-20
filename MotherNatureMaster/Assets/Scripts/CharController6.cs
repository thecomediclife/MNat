﻿using UnityEngine;
using System.Collections;

public class CharController6 : MonoBehaviour {
	public float speed = 2.0f;
	public float rotateSpeed = 10.0f;
	
	public enum State {Default, Pause, Continue, PauseTimed, ChosenDir, SnapTo};
	public State currentState = State.Default;
	public State nextState = State.Default;
	
	public Transform currentNode;
	public Transform nextNode;
	public Transform previousNode;
	private Transform nullNode = null;
	private Vector3 lookTarget;
	private Transform chosenNode;
	
	public bool nextNodeExists;
	public bool previousNodeExists;

	private bool chosenSnapTo = false;
	
	private float timer;
	
	public Transform[] nodeArray = new Transform[6];

	private int nodeLayerMask = 1 << 10;
	
	void Start () {
		lookTarget = transform.forward;
	}
	
	void Update () {
		Debug.DrawRay (transform.position, transform.forward, Color.green);

		FillArrays ();

		switch (currentState) {
			
		case State.Default:
			if (nextNode != nullNode) {
				MoveToNext();
			} else {
				FindNextNode ();
				NextPathRandom();
			}
			
			if (nextNode != nullNode && Vector3.Distance (transform.position, nextNode.position) < 0.05) {
				FindNextNode ();
				NextPathRandom ();
			}
			break;
			
		case State.Pause:
			//transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z)), 10f * Time.deltaTime);
			transform.position = Vector3.MoveTowards(transform.position, nextNode.position, speed * Time.deltaTime);
			if (Vector3.Distance(transform.position, nextNode.position) < 0.05)
				if (lookTarget - transform.position != Vector3.zero)
					transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (lookTarget - transform.position, new Vector3 (0, 1, 0)), rotateSpeed * Time.deltaTime);
			
			break;

		case State.Continue:
			if (Vector3.Distance(transform.position, new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z))) < 0.05)
				currentState = nextState;

			break;

		case State.PauseTimed:
			transform.position = Vector3.MoveTowards(transform.position, nextNode.position, speed * Time.deltaTime);
			if (Vector3.Distance(transform.position, nextNode.position) < 0.05)
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (lookTarget - transform.position, new Vector3 (0, 1, 0)), rotateSpeed * Time.deltaTime);

			if (Time.time > timer) {
				currentState = nextState;
				nextState = State.Default;
			}
			break;

		case State.ChosenDir:
			if (nextNode != nullNode) {
				MoveToNext();
			} else {
				FindNextNode ();
				NextPathChosen();
			}

			if (nextNode != nullNode && Vector3.Distance (transform.position, nextNode.position) < 0.05 && !chosenSnapTo) {
				FindNextNode ();
				NextPathChosen ();
				chosenSnapTo = true;
			}

			if (nextNode != nullNode && Vector3.Distance(transform.position, chosenNode.position) < 0.05 && chosenSnapTo) {
				FindNextNode();
				NextPathRandom();
				currentState = State.Default;
				chosenSnapTo = false;
			}
			break;

		case State.SnapTo:
//			transform.position = Vector3.MoveTowards(transform.position, nextNode.position, 10f * Time.deltaTime);

//			if (Vector3.Distance(transform.position, nextNode.position) < 0.05)
//				currentState = State.Pause;
			transform.position = nextNode.position;

			break;
		}
		

		if (Input.GetKeyDown (KeyCode.P))
			SnapTo (nextNode);

		if (Input.GetKeyUp (KeyCode.P))
			Continue (false, nullNode);
	}

	//Legacy code. This part used to fill the node array by trigger
/*	void OnTriggerEnter(Collider other) {
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
				}
			}
		}
	}*/

	void FillArrays() {
		//Calculate the initial position of the raycasts
		Vector3 forwardVec = transform.position + new Vector3(0f,0.5f,1f);
		forwardVec = new Vector3(Mathf.Round (forwardVec.x), forwardVec.y, Mathf.Round(forwardVec.z));
		Vector3 backVec = transform.position + new Vector3 (0f, 0.5f, -1f);
		backVec = new Vector3 (Mathf.Round (backVec.x), backVec.y, Mathf.Round (backVec.z));
		Vector3 rightVec = transform.position + new Vector3 (1f, 0.5f, 0f);
		rightVec = new Vector3 (Mathf.Round (rightVec.x), rightVec.y, Mathf.Round (rightVec.z));
		Vector3 leftVec = transform.position + new Vector3 (-1f, 0.5f, 0f);
		leftVec = new Vector3 (Mathf.Round (leftVec.x), leftVec.y, Mathf.Round (leftVec.z));
		Vector3 downVec = new Vector3 (0f, -1f, 0f);
		float rayDistance = 1f;

		//Visually see raycasts in game if gizmos are on.
		Debug.DrawRay (forwardVec, downVec * rayDistance, Color.red);
		Debug.DrawRay (backVec, downVec * rayDistance, Color.red);
		Debug.DrawRay (rightVec, downVec * rayDistance, Color.red);
		Debug.DrawRay (leftVec, downVec * rayDistance, Color.red);

		//Save Raycast information
		RaycastHit hit1;
		RaycastHit hit2;
		RaycastHit hit3;
		RaycastHit hit4;

		//Fire raycasts and get information
		Physics.Raycast (forwardVec, downVec, out hit1, rayDistance, nodeLayerMask);
		Physics.Raycast (backVec, downVec, out hit2, rayDistance, nodeLayerMask);
		Physics.Raycast (rightVec, downVec, out hit3, rayDistance, nodeLayerMask);
		Physics.Raycast (leftVec, downVec, out hit4, rayDistance, nodeLayerMask);

		//Remove nodes that are too far
		for (int k = 0; k < nodeArray.Length; k++) {
			if (nodeArray[k] != null) {
				if (Vector3.Distance(transform.position, nodeArray[k].position) > 1f)
					nodeArray[k] = nullNode;
			}
		}

		//Check if new nodes are already in array.
		bool hit1In = false;
		bool hit2In = false;
		bool hit3In = false;
		bool hit4In = false;

		for (int i = 0; i < nodeArray.Length; i++) {
			if (nodeArray[i] != null) {
				if (nodeArray[i] == hit1.transform)
					hit1In = true;
				if (nodeArray[i] == hit2.transform)
					hit2In = true;
				if (nodeArray[i] == hit3.transform)
					hit3In = true;
				if (nodeArray[i] == hit4.transform)
					hit4In = true;
			}
		}

		//Place new nodes in the Array
		for (int j = 0; j < nodeArray.Length; j++) {
			if (!hit1In) {
				if (nodeArray[j] == null) {
					nodeArray[j] = hit1.transform;
					hit1In = true;
				}
			} else if (!hit2In) {
				if (nodeArray[j] == null) {
					nodeArray[j] = hit2.transform;
					hit2In = true;
				}
			} else if (!hit3In) {
				if (nodeArray[j] == null) {
					nodeArray[j] = hit3.transform;
				    hit3In = true;
				}
			} else if (!hit4In) {
				if (nodeArray[j] == null) {
					nodeArray[j] = hit4.transform;
					hit4In = true;
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

	void MoveToNext() {
		transform.position = Vector3.MoveTowards (transform.position, nextNode.position, speed * Time.deltaTime);
		if (lookTarget - transform.position != Vector3.zero)
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (lookTarget - transform.position, new Vector3 (0, 5, 0)), rotateSpeed * Time.deltaTime);
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
	
	
	void NextPathRandom () {
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
		
		if (nextNode != nullNode)
			lookTarget = nextNode.position;
	}
	
	public void Pause(Transform snapToTarget, bool lookAtObject, Transform lookAtTarget) {
		currentState = State.Pause;
		previousNode = currentNode;
		currentNode = nextNode;
		nextNode = snapToTarget;
		
		if (lookAtObject)
			lookTarget = lookAtTarget.position;
		else
			lookTarget = transform.position + transform.forward * 10f;
	}

	public void Continue(bool chooseDirection, Transform chosenDirection) {
		if (chooseDirection) {
			nextState = State.ChosenDir;
			chosenNode = chosenDirection;
		} else {
			nextState = State.Default;
		}
		currentState = State.Continue;
	}

	public void PauseTimed(Transform snapToTarget, bool lookAtObject, Transform lookAtTarget, float delaySeconds, bool chooseDir, Transform chosenDirection) {
		currentState = State.PauseTimed;
		previousNode = currentNode;
		currentNode = nextNode;
		nextNode = snapToTarget;

		if (lookAtObject)
			lookTarget = lookAtTarget.position;
		else
			lookTarget = transform.position + transform.forward * 10f;

		timer = Time.time + delaySeconds;

		if (chooseDir) {
			nextState = State.ChosenDir;
			chosenNode = chosenDirection;
		} else {
			nextState = State.Default;
		}
	}
	
	public void ChooseDirection(Transform selfSnapTo, Transform chosenDirection) {
		currentState = State.ChosenDir;
		chosenNode = chosenDirection;
		nextNode = selfSnapTo;
		lookTarget = selfSnapTo.position;
	}

	void NextPathChosen() {
		bool chosenNodeExists = false;

		for (int i = 0; i < nodeArray.Length; i++) {
			if (nodeArray[i] = chosenNode) {
				nextNode = chosenNode;
				lookTarget = chosenNode.position;
				chosenNodeExists = true;
				break;
			}
		}

		if (!chosenNodeExists) {
			NextPathRandom();
			Debug.Log ("couldn't find chosen node in possible pathways");
		}
	}

	public void SnapTo(Transform transformToSnapTo) {
		currentState = State.SnapTo;
		previousNode = nullNode;
		currentNode = nullNode;

		nextNode = transformToSnapTo;

/*		float distanceTemp = 100f;
		for (int i = 0; i < nodeArray.Length; i++) {
			if (nodeArray[i] != nullNode && Vector3.Distance(nodeArray[i].position, transform.position) < distanceTemp) {
				distanceTemp = Vector3.Distance(nodeArray[i].position, transform.position);
				nextNode = nodeArray[i];
				currentNode = nodeArray[i];
			}
		}*/
	}
}
