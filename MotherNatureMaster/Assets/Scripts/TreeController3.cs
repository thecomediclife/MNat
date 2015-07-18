using UnityEngine;
using System.Collections;

public class TreeController3 : MonoBehaviour {

	private Transform boy;
	public Transform platform;
	private bool boyOnPlatform;

	public bool buttonDown;
	public bool touchDrag;
	public bool activating = false;
	public bool platformHeightReached = false;

	private float percent = 0;
	public float platformSpeed = 2f;
	private Vector3 direction;
	public Vector3 startPos;
	public Vector3 endPos;


	void Start () 
	{
	
	}

	void Update () 
	{
		//	Check if boy is within Platform's trigger collider & buttonDown is true
		if (boyOnPlatform && buttonDown)
			boy.transform.parent = transform;
//		else
//			boy.transform.parent = null;

		//	Activate platform only when buttondown is true 
		if (buttonDown) {
			//	Check if platform is at highest point (true) or at lowest point (false)
			if (!platformHeightReached) {
				//	If platform is at lowest point, grow
				ActivatePlatform (true);
			} else {
				//	If platform is at highest point, rot
				ActivatePlatform (false);
			}
		}


	}


	void ActivatePlatform (bool grow) 
	{
		activating = true;

		//	Change startPos and endPos depending on if growing or rotting
		switch (grow) 
		{
		//	Growing
		case true:		
			//	startPos = ;
			//	endPos = ;
			break;
		//	Rotting
		case false:				
			//	startPos = ;
			//	endPos = ;
			break;
		}

		//	Growing/Rotting function
		while (activating) 
		{
			percent += platformSpeed * Time.deltaTime;
			platform.position = Vector3.Lerp (startPos, endPos, percent);

			if (Vector3.Distance(platform.position, endPos) < 0.01f)
			{
				activating = false;
				platformHeightReached = !platformHeightReached;
			}
		}
	}


	void OnTriggerEnter (Collider other) 
	{
		boyOnPlatform = true;
	}

	void OnTriggerExit (Collider other)
	{
		boyOnPlatform = false;
	}



	void OnMouseDown ()
	{


		print (Input.mousePosition);

//		int groundLayerMask = 1 << 9;
//
//		RaycastHit hit;
//		if (Input.GetButtonDown ("Fire1")) {
//			Vector3 point = Camera.main.ScreenToWorldPoint (Input.mousePosition);
//			
//			//	Shoot raycast, ignore all layers except layer 10 (Trigger)
//			/*			if (Physics.Raycast (point, Camera.main.transform.forward, out hit, Mathf.Infinity, triggerLayerMask)) {
//					hit.transform.GetComponent<TreeController2>().buttonDown = true;
//			}*/
//			
//			if (Physics.Raycast (point, Camera.main.transform.forward, out hit, Mathf.Infinity, groundLayerMask)) {
////				if (hit.transform.parent != null)
////					hit.transform.parent.transform.GetComponent<TreeController2>().buttonDown = true;
//			}
//			
//		}
	}

	void OnMouseUp ()
	{

	}
}
