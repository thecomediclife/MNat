using UnityEngine;
using System.Collections;

public class Level4GameController : MonoBehaviour {
	public Transform door;

	public Transform bucket1, bucket2, bucket3, bucket4;

	public bool activated;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (bucket1.GetComponent<BucketTrigger> ().ballCounter == 2 && bucket2.GetComponent<BucketTrigger> ().ballCounter == 1 && bucket3.GetComponent<BucketTrigger> ().ballCounter == 2 && bucket4.GetComponent<BucketTrigger> ().ballCounter == 3) {
			activated = true;
		}

		if (activated) {
//			panel1.localPosition = Vector3.MoveTowards(panel1.localPosition, new Vector3(-4f, 18.895f, -6f), 0.05f);
//			panel2.localPosition = Vector3.MoveTowards(panel2.localPosition, new Vector3(-4f, 18.895f, -7f), 0.05f);
			door.localPosition = Vector3.MoveTowards(door.localPosition, new Vector3(2.5f, 4.5f, -2f), 0.02f);
		}
	}
}
