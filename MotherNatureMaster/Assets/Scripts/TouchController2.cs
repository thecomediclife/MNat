using UnityEngine;
using System.Collections;

public class TouchController2 : MonoBehaviour 
{
//	public LayerMask treeLayerMask;

	private float dist;
	public bool dragging;
	private Vector3 offset;
	private Transform target;

	private Vector3 curV3;
	private Vector3 prevV3;

	public float deltaPosV3Y;

	void Update ()
	{
		if (Input.touchCount > 0) 
		{
			Touch touch = Input.touches[0];
			Vector3 cur = touch.position;
			Vector3 deltaPos = touch.deltaPosition;

			if (touch.phase == TouchPhase.Began)
			{
				int treeLayerMask = 1 << 11;
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(cur);

				if(Physics.Raycast(ray, out hit, Mathf.Infinity, treeLayerMask))
				{
					target = hit.transform;
					print (target.name);

					dist = Vector3.Distance(hit.transform.position, Camera.main.transform.position);

					curV3 = new Vector3(cur.x, cur.y, dist);
					curV3 = Camera.main.ScreenToWorldPoint(curV3);
					curV3 = new Vector3 (target.position.x, curV3.y, target.position.z);

					prevV3 = curV3;

					dragging = true;

					target.GetComponent<TreeController5> ().dragging = dragging;
					target.GetComponent<TreeController5> ().activate = true;
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

			if (touch.phase == TouchPhase.Stationary && dragging)
			{
				deltaPosV3Y = 0;
			}
			
			if (touch.phase == TouchPhase.Ended && dragging) 
			{
				dragging = false;
				target.GetComponent<TreeController5> ().dragging = dragging;
				target.GetComponent<TreeController5> ().growing = true;
			}
		}
	}
}
