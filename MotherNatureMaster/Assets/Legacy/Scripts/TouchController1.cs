﻿using UnityEngine;
using System.Collections;

public class TouchController1 : MonoBehaviour {

	private bool dragging;
	private Vector3 dragStart;
	private Vector3 dragEnd;
	private GameObject selectedVeggie;

	void Update ()
	{
		if (Input.GetButtonDown ("Fire1")) 
		{
			int groundLayerMask = 1 << 9;
			RaycastHit hitTarget;
			Vector3 point = Camera.main.ScreenToWorldPoint (Input.mousePosition + new Vector3 (0, 0, 1));
		
			//	Make sure that only blocks with "Ground" layer can be selected by player
			if (Physics.Raycast (point, Camera.main.transform.forward, out hitTarget, Mathf.Infinity, groundLayerMask)) 
			{
				selectedVeggie = hitTarget.transform.parent.gameObject;

				//	Once selected, make sure that selected veggie is not already activating (lift is not moving)
				if (!selectedVeggie.GetComponent<TreeController4>().activatePlatform)
				{
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit)) 
					{	
						dragStart = hit.point;
					}
				
					dragging = true;
				}
			}
		}
	
		//	Dragging stays true once player puts down finger
		if (dragging) 
		{
			//	Once player lifts finger, dragging ends. Shoot a raycast to see where player lifted finger.
			if (Input.GetButtonUp ("Fire1")) 
			{
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) 
				{
					dragEnd = hit.point;
				}
				
				dragging = false;

				float dist = Vector3.Distance(dragStart, dragEnd);
				float direc = Mathf.Sign((dragEnd - dragStart).y);
				float dragDist = dist * direc;

				selectedVeggie.GetComponent<TreeController4>().DetermineFloor(dragDist);
			}
		}
	}
}
