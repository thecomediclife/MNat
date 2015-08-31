using UnityEngine;
using System.Collections;

public class TouchController4 : MonoBehaviour {
	private float dist;
	private bool dragging;
	private Vector3 offset;
	public Transform target;

	public bool fingerDown;
	public bool fingerHold;
	public bool fingerUp;

	private Vector3 curV3;
	private Vector3 prevV3;
	
	public float deltaPosV3Y;
	
	void Update ()
	{
		if (Input.touchCount > 0) 
		{
			//Determines where the player has touched.
			Touch touch = Input.touches[0];
			Vector3 cur = touch.position;
			cur += new Vector3(-2f,5f,0f);
			Vector3 deltaPos = touch.deltaPosition;
			
			Debug.DrawRay(Camera.main.ScreenToWorldPoint(cur), Camera.main.transform.forward * 100f, Color.green);
			
			if (touch.phase == TouchPhase.Began)
			{
				int treeLayerMask = 1 << 11;
				//				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(cur);
				
				//RaycastAll to determine which tree is closest to finger
				RaycastHit[] hits;
				hits = Physics.RaycastAll(ray, Mathf.Infinity, treeLayerMask);
				float closestDistance = 1000f;
				for (int i = 0; i < hits.Length; i++) {
					if (hits[i].transform != null) {
						Vector3 hitPoint = hits[i].point;
						hitPoint = new Vector3(hitPoint.x, 0f, hitPoint.z);
						
						Vector3 treePoint = hits[i].transform.parent.transform.position;
						treePoint = new Vector3(treePoint.x, 0f, treePoint.z);
						
						if (Vector3.Distance(hitPoint, treePoint) < closestDistance) {
							closestDistance = Vector3.Distance(hitPoint, treePoint);
							target = hits[i].transform.parent.transform;
						}
					}
				}
				
				if (closestDistance > 999f) {
					target = null;
				}
				
				//				if(Physics.Raycast(ray, out hit, Mathf.Infinity, treeLayerMask))
				if (target != null)
				{
					//					target = hit.transform.parent.transform;
					
					if (target.GetComponent<TreeController5>().inSunLight == true)
					{
						dist = Vector3.Distance(target.transform.position, Camera.main.transform.position);
						
						curV3 = new Vector3(cur.x, cur.y, dist);
						curV3 = Camera.main.ScreenToWorldPoint(curV3);
						curV3 = new Vector3 (target.position.x, curV3.y, target.position.z);
						
						prevV3 = curV3;
						
						dragging = true;
						
						target.GetComponent<TreeController5> ().activate = true;
						target.GetComponent<TreeController5> ().dragging = true;
						target.GetComponent<TreeController5> ().growing = false;
						target.GetComponent<TreeController5> ().decaying = false;
					}
				}
			}
			
			if (touch.phase == TouchPhase.Moved && dragging) 
			{
				
				//	Find the current position of the thumb as the player is dragging
				curV3 = new Vector3(cur.x, cur.y, dist);
				curV3 = Camera.main.ScreenToWorldPoint(curV3);
				curV3 = new Vector3 (target.position.x, curV3.y, target.position.z);
				
				// 	Find the previous position of the thumb on the previous update
				prevV3 = new Vector3(cur.x - deltaPos.x, cur.y - deltaPos.y, dist);
				prevV3 = Camera.main.ScreenToWorldPoint(prevV3);
				prevV3 = new Vector3 (target.position.x, prevV3.y, target.position.z);
				
				deltaPosV3Y = curV3.y - prevV3.y;
				target.GetComponent<TreeController5> ().deltaY = deltaPosV3Y;
			}

			if (touch.phase == TouchPhase.Moved) {
				Debug.Log ("blah");
			}
			
			if (touch.phase == TouchPhase.Stationary && dragging)
			{
				deltaPosV3Y = 0;
			}
			
			if (touch.phase == TouchPhase.Ended && dragging) 
			{
				dragging = false;
				target.GetComponent<TreeController5> ().dragging = false;
				target.GetComponent<TreeController5> ().growing = true;
				target.GetComponent<TreeController5> ().decaying = false;
			}
		}
	}
}
