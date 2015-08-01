using UnityEngine;
using System.Collections;

public class GrabAttention : MonoBehaviour {
	public int paths = 2;

	public int path1Size;
	public Transform[] path1 = new Transform[10];
	public int paths2Size;
	public Transform[] path2 = new Transform[10];

	private Transform boy;
	public int chosenPath = 0;

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

		if (currentPathCheck < 1) {

			for (int i = 0; i < path1.Length; i++) {
				if (path1[i] != null) {
					if (boy.GetComponent<CharController6>().currentNode == path1[i]) {
						if (path1Size - i < shortestPath) {
							shortestPath = path1Size - i;
							chosenPath = 1;
						}
					}
					if (boy.GetComponent<CharController6>().nextNode == path1[i]) {
						if (path1Size - i < shortestPath) {
							shortestPath = path1Size - i;
							chosenPath = 1;
						}
					}
				}
			}

			currentPathCheck++;
		}

		if (currentPathCheck < 2) {
			
			for (int i = 0; i < path2.Length; i++) {
				if (path2[i] != null) {
					if (boy.GetComponent<CharController6>().currentNode == path2[i]) {
						if (paths2Size - i < shortestPath) {
							shortestPath = paths2Size - i;
							chosenPath = 2;
						}
					}
					if (boy.GetComponent<CharController6>().nextNode == path2[i]) {
						if (paths2Size - i < shortestPath) {
							shortestPath = paths2Size - i;
							chosenPath = 2;
						}
					}
				}
			}
			
			currentPathCheck++;
		}

		Debug.Log (chosenPath + " chosen");
	}
}
