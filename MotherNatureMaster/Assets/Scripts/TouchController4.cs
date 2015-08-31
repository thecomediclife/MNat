using UnityEngine;
using System.Collections;

public class TouchController4 : MonoBehaviour {
	private float dist;
	private bool dragging;
	private Vector3 offset;
	public Transform target;

	public bool fingerDown;
	public bool fingerMove;
	public bool fingerStationary;
	public bool fingerUp;

	private Vector3 curV3;
	private Vector3 prevV3;
	
	private float deltaPosV3Y;

	private float currentPos;
	private float previousPos;
	public float deltaY;

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

			fingerDown = false;
			if (touch.phase == TouchPhase.Began)
			{
				fingerDown = true;

				//Obtain which tree is hit when player first begins touching screen
				int treeLayerMask = 1 << 11;
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

				previousPos = touch.position.y;
				currentPos = touch.position.y;
			}

			if (touch.phase == TouchPhase.Moved) {
				previousPos = currentPos;
				currentPos = touch.position.y;

				fingerMove = true;
				fingerStationary = false;
			}

			if (touch.phase == TouchPhase.Stationary) {
				previousPos = touch.position.y;
				currentPos = touch.position.y;
				
				fingerMove = false;
				fingerStationary = true;
			}

			fingerUp = false;
			if (touch.phase == TouchPhase.Ended) {
				fingerUp = true;
				fingerMove = false;
				fingerStationary = false;

				previousPos = touch.position.y;
				currentPos = touch.position.y;

				target = null;
			}

			deltaY = currentPos - previousPos;
		}
	}
}
