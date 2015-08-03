using UnityEngine;
using System.Collections;

public class GrabAttention : MonoBehaviour {
	public int paths = 2;

	public int path1Size;
	public Transform[] path1 = new Transform[10];
	public int path2Size;
	public Transform[] path2 = new Transform[10];
	public int path3Size;
	public Transform[] path3 = new Transform[10];
	public int path4Size;
	public Transform[] path4 = new Transform[10];

	private Transform boy;
	public int chosenPath = 0;

	public float delay = 5.0f;
	public float timer = 0.0f;

	// Use this for initialization
	void Start () {
		boy = GameObject.FindWithTag ("Kid").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			FindClosestPath();
		}
	}

	void FindClosestPath() {
		int shortestPath = 1000;
		int currentPathCheck = 0;
		chosenPath = 0;
		int closestNodeIndex = 0;

		if (Time.time >= timer) {

			if (currentPathCheck < 1) {

				for (int i = 0; i < path1.Length; i++) {
					if (path1 [i] != null) {
						if (boy.GetComponent<CharController6> ().currentNode == path1 [i]) {
							if (path1Size - i < shortestPath) {
								shortestPath = path1Size - i;
								chosenPath = 1;
								closestNodeIndex = i;
							}
						}
						if (boy.GetComponent<CharController6> ().nextNode == path1 [i]) {
							if (path1Size - i < shortestPath) {
								shortestPath = path1Size - i;
								chosenPath = 1;
								closestNodeIndex = i;
							}
						}
					}
				}

				currentPathCheck++;
			}

			if (currentPathCheck < 2) {
			
				for (int i = 0; i < path2.Length; i++) {
					if (path2 [i] != null) {
						if (boy.GetComponent<CharController6> ().currentNode == path2 [i]) {
							if (path2Size - i < shortestPath) {
								shortestPath = path2Size - i;
								chosenPath = 2;
								closestNodeIndex = i;;
							}
						}
						if (boy.GetComponent<CharController6> ().nextNode == path2 [i]) {
							if (path2Size - i < shortestPath) {
								shortestPath = path2Size - i;
								chosenPath = 2;
								closestNodeIndex = i;;
							}
						}
					}
				}
			
				currentPathCheck++;
			}

			if (currentPathCheck < 3) {
				
				for (int i = 0; i < path3.Length; i++) {
					if (path3 [i] != null) {
						if (boy.GetComponent<CharController6> ().currentNode == path3 [i]) {
							if (path3Size - i < shortestPath) {
								shortestPath = path3Size - i;
								chosenPath = 3;
								closestNodeIndex = i;
							}
						}
						if (boy.GetComponent<CharController6> ().nextNode == path3 [i]) {
							if (path3Size - i < shortestPath) {
								shortestPath = path3Size - i;
								chosenPath = 3;
								closestNodeIndex = i;
							}
						}
					}
				}
				
				currentPathCheck++;
			}

			if (currentPathCheck < 4) {
				
				for (int i = 0; i < path4.Length; i++) {
					if (path4 [i] != null) {
						if (boy.GetComponent<CharController6> ().currentNode == path4 [i]) {
							if (path4Size - i < shortestPath) {
								shortestPath = path4Size - i;
								chosenPath = 4;
								closestNodeIndex = i;
							}
						}
						if (boy.GetComponent<CharController6> ().nextNode == path4 [i]) {
							if (path4Size - i < shortestPath) {
								shortestPath = path4Size - i;
								chosenPath = 4;
								closestNodeIndex = i;
							}
						}
					}
				}
				
				currentPathCheck++;
			}

			timer = Time.time + delay;

			//Debug.Log (chosenPath + " chosen");
			//Debug.Log (currentPathCheck);
			Debug.Log (closestNodeIndex);
		}

		if (chosenPath > 0) {
			PathFound(closestNodeIndex);
		}
	}

	void PathFound(int closestNodeIndex) {
		if (chosenPath == 0)
			Debug.Log ("WHAT THE FUCK, this should never be printed.");

		if (chosenPath == 1) {
			for (int i = 0; i < path1.Length; i++) {
				boy.GetComponent<CharController6>().directedPathway[i] = path1[i];
			}
		} else if (chosenPath == 2) {
			for (int i = 0; i < path2.Length; i++) {
				boy.GetComponent<CharController6>().directedPathway[i] = path2[i];
			}
		} else if (chosenPath == 3) {
			for (int i = 0; i < path3.Length; i++) {
				boy.GetComponent<CharController6>().directedPathway[i] = path3[i];
			}
		} else if (chosenPath == 4) {
			for (int i = 0; i < path4.Length; i++) {
				boy.GetComponent<CharController6>().directedPathway[i] = path4[i];
			}
		}

		boy.GetComponent<CharController6> ().DirectedPathwayFunc (closestNodeIndex);
	}
}
