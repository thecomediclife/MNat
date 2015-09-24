using UnityEngine;
using System.Collections;

public class TouchController5 : MonoBehaviour {
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
	
//	private float deltaPosV3Y;
	
	private float currentPos;
	private float previousPos;
//	public float deltaY;

	public bool enableCursor;

	public Transform cursor;

	private bool animationPlaying;
	private bool animGrow = true;

	void Start() {
		cursor = this.transform.Find ("FingerTouchIndicator");
	}

	void Update ()
	{
		if (Input.touchCount > 0) 
		{
			//Determines where the player has touched.
			Touch touch = Input.touches[0];
			Vector3 cur = touch.position;
			cur += new Vector3(-2f,5f,0f);
//			Vector3 deltaPos = touch.deltaPosition;
			
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

				DetermineClosestTarget(hits);
				
//				previousPos = touch.position.y;
//				currentPos = touch.position.y;

				if (enableCursor) {
					cursor.GetComponent<SpriteRenderer>().enabled = true;
					animationPlaying = true;
				}
			}
			
			if (touch.phase == TouchPhase.Moved) {
//				previousPos = currentPos;
//				currentPos = touch.position.y;
				
				fingerMove = true;
				fingerStationary = false;
			}
			
			if (touch.phase == TouchPhase.Stationary) {
//				previousPos = touch.position.y;
//				currentPos = touch.position.y;
				
				fingerMove = false;
				fingerStationary = true;
			}
			
			fingerUp = false;
			if (touch.phase == TouchPhase.Ended) {
				fingerUp = true;
				fingerMove = false;
				fingerStationary = false;
				
//				previousPos = touch.position.y;
//				currentPos = touch.position.y;
				
				target = null;

				if (enableCursor) {
					cursor.GetComponent<SpriteRenderer>().enabled = false;;
				}
			}

			if (enableCursor) {
				Vector3 curPos = touch.position;
				cursor.position = Camera.main.ScreenToWorldPoint(curPos) + Camera.main.transform.forward * 5f;
			}
			
//			deltaY = currentPos - previousPos;
		}

		if (animationPlaying) {
			if (animGrow) {
				
				cursor.localScale = Vector3.MoveTowards (cursor.localScale, new Vector3 (1.5f, 1.5f, 1f), 5f * Time.deltaTime);
				
				if (Vector3.Distance (cursor.localScale, new Vector3 (1.5f, 1.5f, 1f)) < 0.1f) {
					animGrow = false;
				}
				
			} else {
				
				cursor.localScale = Vector3.MoveTowards (cursor.localScale, new Vector3 (1f, 1f, 1f), 5f * Time.deltaTime);
				
				if (Vector3.Distance (cursor.localScale, new Vector3 (1f, 1f, 1f)) < 0.1f) {
					animGrow = true;
					animationPlaying = false;
				}
			}
		}
	}

	void DetermineClosestTarget(RaycastHit[] hits) {
		float closestDistance = 1000f;
		for (int i = 0; i < hits.Length; i++) {
			if (hits[i].transform != null) {
				Vector3 hitPoint = hits[i].point;
				
				Vector3 treePoint = hits[i].transform.parent.transform.position;

				if (hits[i].transform.up == new Vector3(1f,0f,0f) || hits[i].transform.up == new Vector3(-1f,0f,0f)) {
					hitPoint = new Vector3(0f, hitPoint.y, hitPoint.z);
					treePoint = new Vector3(0f, treePoint.y, treePoint.z);
				} else if (hits[i].transform.up == new Vector3(0f,1f,0f) || hits[i].transform.up == new Vector3(0f,-1f,0f)) {
					hitPoint = new Vector3(hitPoint.x, 0f, hitPoint.z);
					treePoint = new Vector3(treePoint.x, 0f, treePoint.z);
				} else if (hits[i].transform.up == new Vector3(0f,0f,1f) || hits[i].transform.up == new Vector3(0f,0f,-1f)) {
					hitPoint = new Vector3(hitPoint.x, hitPoint.y, 0f);
					treePoint = new Vector3(treePoint.x, treePoint.y, 0f);
				} else {
					Debug.Log ("hit target isn't facing any uniform direction.");
				}
				
				if (Vector3.Distance(hitPoint, treePoint) < closestDistance) {
					closestDistance = Vector3.Distance(hitPoint, treePoint);
					target = hits[i].transform.parent.transform;
				}
			}
		}
		
		if (closestDistance > 999f) {
			target = null;
		}
	}

}
