using UnityEngine;
using System.Collections;

public class SpawnTree2 : MonoBehaviour {
	private RaycastHit hit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		int triggerLayerMask = 1 << 10;
		int groundLayerMask = 1 << 9;
		
		if (Input.GetButtonDown ("Fire1")) {
			Vector3 point = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			
			//	Shoot raycast, ignore all layers except layer 10 (Trigger)
/*			if (Physics.Raycast (point, Camera.main.transform.forward, out hit, Mathf.Infinity, triggerLayerMask)) {
					hit.transform.GetComponent<TreeController2>().buttonDown = true;
			}*/

			if (Physics.Raycast (point, Camera.main.transform.forward, out hit, Mathf.Infinity, groundLayerMask)) {
				if (hit.transform.parent != null)
					hit.transform.parent.transform.GetComponent<TreeController2>().buttonDown = true;
			}

		}
	}
}
