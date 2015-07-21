using UnityEngine;
using System.Collections;

public class TreeController3 : MonoBehaviour {

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	///	Process Flow: 
	/// 1. CalculateTouchDrag () function computes the distance from the player puts the finger down (dragging = true) 
	/// to when the player lifts the finger up (dragging = false). 
	/// 2. Lifting the finger is called only once and immediately calls the functions DetermineFloor() and ActivatePlatform().
	/// During this step, this script checks the boy's script to see if he is on or going towards the veggie node. If 
	/// this is true and platform is activating, then call function SnapTo(). SnapTo() is only called once. 
	/// 3a. DetermineFloor() calculates the drag distance and determines what floor (end position) the platform should move to. 
	/// 3b. ActivatePlatform() begins moving the platfrom from start position to end position, determined by function 
	/// DetermineFloor(). When platform reaches end position (activatePlatform = false), ActivatePlatform() is immediately 
	/// turned off. This is where we can tell Boy to Continue() moving assuming it was parented to the platform. 
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public Transform boy;
	public Transform platform;
	public Transform sparkles;
	Transform sparklesInst;
	
	private bool activatePlatform = false;
	private bool reachedEndPos = false;

	private bool dragging;
	private float dragStart;
	private float dragEnd;
	private float dragDist;
	public float dragLimit = 2;

	private float percent = 0;
	public float platformSpeed = 2f;
	
	public float[] floors; 
	private int floorNum = 0;

	private Vector3 groundPos;
	private Vector3 startPos;
	private Vector3 endPos;

	private Transform veggieNode;



	void Awake ()
	{
		veggieNode = transform.Find ("Leaves").transform.Find ("Node");
		boy = GameObject.Find ("Boy").transform;
		groundPos = platform.transform.position;
	}

	void Update () 
	{
		CalculateTouchDrag ();

		ActivatePlatform (startPos, endPos);
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	This function lerps from start position to end position.
	void ActivatePlatform (Vector3 start, Vector3 end) 
	{
		if (activatePlatform) {

			percent += platformSpeed * Time.deltaTime;
			platform.position = Vector3.Lerp (start, end, percent);

			if (Vector3.Distance (platform.position, end) < 0.05f) {
				platform.position = end;
				startPos = end;
				percent = 0;
				activatePlatform = false;
			
				if (boy.parent != null) 
				{
					boy.GetComponent<CharController6> ().Continue (false, null);
				}
			}
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

					sparklesInst = Instantiate (sparkles, hit.point, Quaternion.identity) as Transform;
				}

				dragging = true;
			}
		}

		//	Dragging is true once player puts down finger
		if (dragging) 
		{
			//	Raycast for sparkles. This raycast needs to continuously shoot to see where finger moves.
			Ray raySparkles = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitSparkles;
			if (Physics.Raycast(raySparkles, out hitSparkles))
			{	
				sparklesInst.position = hitSparkles.point;
			}

			//	Once player lifts finger, dragging ends. Shoot a raycast to see where player lifted finger.
			if (Input.GetButtonUp ("Fire1")) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					dragEnd = hit.point.y;
				}

				dragDist = dragEnd - dragStart;

				DetermineFloor();
				sparklesInst.GetComponent<SparklesController>().disappear = true;

				dragging = false;
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
			activatePlatform = true;

			SnapTo (); 

			//	Notes:
			//	Because arrays start from index 0, floorNum = 0 refers to array [0]. In the inspector
			//	array [0] is always 0, or basically ground level for the platform. Array[1] is the 
			//	first desired floor height relative to the ground level. 
		}
	}

	//	Called only during the frame that player lifts finger. This function checks if the boy at the moment is moving
	//	towards the veggie node or is already on the veggie node when the platform is activating. If platform is 
	//	activating, then snap the boy to the veggie node. 
	void SnapTo ()
	{
//		Transform nextNode = boy.GetComponent<CharController6> ().nextNode;
//		Transform currentNode = boy.GetComponent<CharController6> ().currentNode;
//		
//		if (activatePlatform && (nextNode == veggieNode || currentNode == veggieNode)) {
//			boy.GetComponent<CharController6> ().SnapTo(veggieNode);
//			boy.parent = platform.transform;
//		} else {
//			boy.parent = null;
//		}
	}




}
