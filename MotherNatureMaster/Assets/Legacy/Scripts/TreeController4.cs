using UnityEngine;
using System.Collections;

public class TreeController4 : MonoBehaviour {

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	///	Process Flow: 
	/// 1. The camera has the script for calculating how much the player drags the fingers (dragDist). dragDist is then sent 
	/// here to DetermineFloor() where the floor for the tree is calculated. 
	/// 2. Lifting the finger is called only once and immediately calls the functions DetermineFloor() and ActivatePlatform().
	/// During this step, this script checks the kid's script to see if he is on or going towards the veggie node. If 
	/// this is true and platform is activating, then call function SnapTo(). SnapTo() is only called once. 
	/// 3a. DetermineFloor() calculates the drag distance and determines what floor (end position) the platform should move to. 
	/// 3b. ActivatePlatform() begins moving the platfrom from start position to end position, determined by function 
	/// DetermineFloor(). When platform reaches end position (activatePlatform = false), ActivatePlatform() is immediately 
	/// turned off. This is where we can tell kid to Continue() moving assuming it was parented to the platform. 
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public Transform kid;
	private Transform platform;

	private float percent = 0;
	public float platformSpeed = 2f;
	public bool activatePlatform = false;
	private bool reachedEndPos = false;

	private bool dragging;
	public float dragLimit = 2;

	private int floorNum = 0;
	public float[] floors;

	private Vector3 groundPos;
	private Vector3 startPos;
	private Vector3 endPos;

	private Transform veggieNode;

	public float maxHealth = 20;
	public float currentHealth;

	void Awake ()
	{
		platform = transform.Find ("Leaves");
		veggieNode = transform.Find ("Leaves").transform.Find ("Node");
		kid = GameObject.Find ("Kid").transform;
		groundPos = platform.transform.position;
		endPos = platform.transform.position;
		startPos = platform.transform.position;

		currentHealth = maxHealth;
	}

	void Update () 
	{
		ActivatePlatform (startPos, endPos);

		if (Input.GetButtonDown ("Jump"))
			takeDamage (1);

		if (currentHealth <= 0) 
		{
			DetermineFloor(-100);
			currentHealth = maxHealth;
		}
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	This function lerps from start position to end position.
	void ActivatePlatform (Vector3 start, Vector3 end) 
	{
		if (activatePlatform) 
		{

			percent += platformSpeed * Time.deltaTime;
			platform.position = Vector3.Lerp (start, end, percent);

			if (Vector3.Distance (platform.position, end) < 0.05f) 
			{
				platform.position = end;
				startPos = end;
				percent = 0;
				activatePlatform = false;
			
				if (kid.parent != null) 
				{
					kid.GetComponent<CharController6> ().Continue (false, null);
					kid.parent = null;
				}
			}
		}
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	This function calculates the Y distance from when the player touched the screen to when the player
	//	lifted his finger from the screen. Once player lifts finger and the distance is calculated, go to 
	//	DetermineFloor() function to see how high the tree should grow depending on the drag distance. 

	/////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	This function is called at the end of CalculateTouchDrag(). Depending on how far the player dragged 
	//	his finger, DetermineFloor() will calculate how high the tree grows or rots--calculates end position 
	//	for ActivatePlatform(). 
	public void DetermineFloor (float dragDist) 
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

	//	Called only during the frame that player lifts finger. This function checks if the kid at the moment is moving
	//	towards the veggie node or is already on the veggie node when the platform is activating. If platform is 
	//	activating, then snap the kid to the veggie node. 
	void SnapTo ()
	{
		Transform nextNode = kid.GetComponent<CharController6> ().nextNode;
		Transform currentNode = kid.GetComponent<CharController6> ().currentNode;

		if (activatePlatform && (nextNode == veggieNode || currentNode == veggieNode)) {
			kid.GetComponent<CharController6> ().SnapTo(veggieNode, true);
			kid.parent = platform.transform;
		} else {
			kid.parent = null;
		}
	}

	public void takeDamage (float i) {
		currentHealth -= i;
	}
}
