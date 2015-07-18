using UnityEngine;
using System.Collections;

public class CharController6 : MonoBehaviour {
	public float speed = 2.0f;
	public float rotateSpeed = 10.0f;
	
	public enum State {Default, Pause, ChosenDir};
	public State currentState = State.Default;
	
	public Transform currentNode;
	public Transform nextNode;
	private Transform previousNode;
	private Transform nullNode = null;
	private Vector3 lookTarget;
	
	public bool nextNodeExists;
	public bool previousNodeExists;
	
	private float timer;
	
	public Transform[] nodeArray = new Transform[8];
	
	void Start () {
		lookTarget = transform.forward;
	}
	
	void Update () {
		Debug.DrawRay (transform.position, transform.forward, Color.green);
		
		switch (currentState) {
			
		case State.Default:
			if (nextNode != nullNode) {
				transform.position = Vector3.MoveTowards (transform.position, nextNode.position, speed * Time.deltaTime);
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (lookTarget - transform.position, new Vector3 (0, 1, 0)), rotateSpeed * Time.deltaTime);
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
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (lookTarget - transform.position, new Vector3 (0, 1, 0)), rotateSpeed * Time.deltaTime);
			
			break;
			
		case State.ChosenDir:
			
			break;
		}
		
		
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
	
	public void Pause(Transform snapToTarget, bool lookAtObject, Vector3 lookAtTarget) {
		currentState = State.Pause;
		nextNode = snapToTarget;
		
		if (lookAtObject)
			lookTarget = lookAtTarget;
	}
	
	public void ChooseDirection() {
		currentState = State.ChosenDir;
	}
}
