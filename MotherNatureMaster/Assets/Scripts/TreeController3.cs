using UnityEngine;
using System.Collections;

public class TreeController3 : MonoBehaviour {

	private Transform boy;
	private Transform platform;
	private bool boyOnPlatform;

	public bool dragging;
	private float dragStart;
	private float dragEnd;
	private float dragDist;
	public float dragLimit = 2;

	private float percent = 0;
	public float platformSpeed = 2f;

	public int floorNum = 0;
	public float[] floors; 



	void Start () 
	{
		platform = transform.FindChild ("Leaves");
	}

	void Update () 
	{
		//	Check if boy is within Platform's trigger collider & dragging is true
		if (boyOnPlatform && dragging)
			boy.transform.parent = transform;
		else if (boy != null)
			boy.transform.parent = null;


		CalculateDrag ();


//		if (!dragging) 
//		{
			DetermineFloor ();
//		}
	}


	void ActivatePlatform (Vector3 startPos, Vector3 endPos) 
	{
		bool activating = true;

		//	Growing/Rotting function
		while (activating) 
		{
			percent += platformSpeed * Time.deltaTime;
			platform.position = Vector3.Lerp (startPos, endPos, percent);

			if (Vector3.Distance(platform.position, endPos) < 0.01f)
			{
				activating = false;
			}
		}
	}

	void DetermineFloor () 
	{
		bool counting = true;

		while (counting) {
//		if (floors != null) 
//		{
			//	Big flick up
			if (dragDist >= dragLimit) {
				floorNum += 2;
				counting = false;
			}
			//	Small flick up
			else if (0 < dragDist && dragDist < dragLimit) {
				floorNum += 1;
				counting = false;
			}

			//	Small flick down
			else if (-dragLimit < dragDist && dragDist < 0) {
				floorNum -= 1;
				counting = false;
			}
			//	Big flick down
			else if (dragDist <= -dragLimit) {
				floorNum -= 2;
				counting = false;
			} 

			else {
				counting = false;
			}
			print (floorNum);
		}

//			if (floorNum < 0)
//				floorNum = 0;
//			if (floorNum > floors.Length)
//				floorNum = floors.Length;


//		}
	}


	void OnTriggerEnter (Collider other) 
	{
		boyOnPlatform = true;
	}

	void OnTriggerExit (Collider other)
	{
		boyOnPlatform = false;
	}



	void CalculateDrag ()
	{
		if (Input.GetButtonDown ("Fire1")) 
		{
			int groundLayerMask = 1 << 9;
			RaycastHit hitDirt;
			Vector3 point = Camera.main.ScreenToWorldPoint (Input.mousePosition + new Vector3(0,0,1));

			if (Physics.Raycast (point, Camera.main.transform.forward, out hitDirt, Mathf.Infinity, groundLayerMask)) 
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{	
					dragStart = hit.point.y;
				}

				dragging = true;
			}
		}

		if (dragging) 
		{
			if (Input.GetButtonUp ("Fire1")) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					dragEnd = hit.point.y;
				}

				dragDist = dragEnd - dragStart;
				dragging = false;

				print ("Drag distance is  " + dragDist);
			}
		}
	}
}
