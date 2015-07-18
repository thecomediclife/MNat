using UnityEngine;
using System.Collections;

public class TreeController3 : MonoBehaviour {

	private Transform boy;
	private Transform platform;
	private bool boyOnPlatform;

	private bool dragging;
	private float dragStart;
	private float dragEnd;
	private float dragDist;
	public float dragLimit = 2;

	private float percent = 0;
	public float platformSpeed = 2f;

	public int floorNum = 0;
	public float[] floors; 

	private Vector3 groundPos;
	private Vector3 startPos;
	private Vector3 endPos;

	void Awake ()
	{
		platform = transform.FindChild ("Leaves");
	}

	void Start () 
	{
		groundPos = platform.transform.position;
	}

	void Update () 
	{
//		//	Check if boy is within Platform's trigger collider & dragging is true
//		if (boyOnPlatform && dragging)
//			boy.transform.parent = transform;
//		else if (boy != null)
//			boy.transform.parent = null;

		CalculateTouchDrag ();

		ActivatePlatform (startPos, endPos);
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	This function lerps from start position to end position.
	void ActivatePlatform (Vector3 start, Vector3 end) 
	{
		percent += platformSpeed * Time.deltaTime;
		platform.position = Vector3.Lerp (start, end, percent);
		
		if (Vector3.Distance(platform.position, end) < 0.01f)
		{
			percent = 0;
			startPos = end;
		}
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	This function calculates the Y distance from when the player touched the screen to when the player
	//	lifted his finger from the screen. Once player lifts finger and the distance is calculated, go to 
	//	DetermineFloor() function to see how high the tree should grow depending on the drag distance. 
	void CalculateTouchDrag ()
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

				DetermineFloor();
			}
		}
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	This function is called at the end of CalculateTouchDrag(). Depending on how far the player dragged 
	//	his finger, DetermineFloor() will calculate how high the tree grows or rots--calculates end position 
	//	for ActivatePlatform(). 
	void DetermineFloor () 
	{
		if (floors != null) {
				
			//	Big flick up
			if (dragDist >= dragLimit) {
				floorNum += 2;
			}
			//	Small flick up
			else if (0 < dragDist && dragDist < dragLimit) {
				floorNum += 1;
			}
			//	Small flick down
			else if (-dragLimit < dragDist && dragDist < 0) {
				floorNum -= 1;
			}
			//	Big flick down
			else if (dragDist <= -dragLimit) {
				floorNum -= 2;
			} else {
			}

			floorNum = Mathf.Clamp (floorNum, 0, floors.Length - 1);
			endPos = groundPos + new Vector3 (0, floors [floorNum], 0);

			//	Notes:
			//	Because arrays start from index 0, floorNum = 0 refers to array [0]. In the inspector
			//	array [0] is always 0, or basically ground level for the platform. Array[1] is the 
			//	first desired floor height relative to the ground level. 
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
}
