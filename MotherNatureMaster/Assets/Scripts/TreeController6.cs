using UnityEngine;
using System.Collections;

public class TreeController6 : MonoBehaviour {
	private TouchController4 mainCam;

	private Transform kid;
	private Transform platform;
	private Transform node;
	private Transform trunk;
	private Transform color;

	public bool chosen;
	public bool liftingHeavyObj;
	public bool inSunLight;
	public bool kidInRange;
	public bool kidAttached;
	
	[HideInInspector]
	public float deltaY;
	public float treeHeight = 6;	
	private float groundY;
	public float maxHeightY;
	
	private float timer;
	public float waitTime = 2;
	
	public bool activate;
	public bool dragging;
	
	public bool growing;
	public float growSpeed = 2f;
	
	public bool decaying;
	public float decaySpeed = 2f;
	
	private Transform touchCollider;

	public float lastInput;

    private BoxCollider boxCollider;

	void Awake()
	{
		trunk = transform.Find ("Trunk");
		platform = transform.Find ("Platform");
		node = platform.Find ("Node");
		color = transform.Find ("Color");
		kid = GameObject.Find ("Kid").transform;
		
		groundY = platform.localPosition.y;
		maxHeightY = groundY + treeHeight;
		
		//	Update the box collider to match the maximum tree height that player inputs
		boxCollider = GetComponent<BoxCollider> () as BoxCollider;
//  	boxCollider.size = new Vector3 (0.5f, treeHeight + 1f, 0.5f);
//		boxCollider.center = new Vector3 (0, (treeHeight - 1f) / 2, 0);
		
		//	TEMPORARY: Provides color box to show where player touch screen to move tree
		color.localScale = new Vector3 (0.5f, treeHeight + 1f, 0.5f);
		color.localPosition = new Vector3 (0, (treeHeight - 1f) / 2, 0);
		
		touchCollider = transform.Find ("Platform").transform.Find("TouchCollider");

		mainCam = Camera.main.transform.GetComponent<TouchController4>();
	}
	
	void Update ()
	{
		DetermineKidDistance ();

		if (mainCam.target == this.transform) {
			chosen = true;
			activate = true;
		} else {
			chosen = false;
		}

		//	TEMPORARY: Creates a tree that scales with the platform's position
		trunk.localScale = new Vector3 (trunk.localScale.x, platform.localPosition.y + 0.5f, trunk.localScale.z); 
		trunk.localPosition = new Vector3 (trunk.localPosition.x, platform.localPosition.y / 2, trunk.localPosition.z);
		
        boxCollider.size = new Vector3(trunk.localScale.x, platform.localPosition.y + 0.5f, trunk.localScale.z);
        boxCollider.center = new Vector3(trunk.localPosition.x, platform.localPosition.y / 2, trunk.localPosition.z);

        //  Disable node while tree is moving
        if (Mathf.Abs (platform.localPosition.y - groundY) < 0.15f || Mathf.Abs (platform.localPosition.y - maxHeightY) < 0.15f) {
			node.gameObject.SetActive (true);
		} else {
			node.gameObject.SetActive (false);
		}
		
		FaceCollider ();
	}
	
	void FixedUpdate ()
	{
		if (inSunLight) {

			if (activate) {
				//Tree is chosen by touchcontroller
				if (chosen) {

					//Save last input.
					if (mainCam.deltaY != 0f) {
						lastInput = mainCam.deltaY;
					}

					if (mainCam.deltaY > 0f) {

						//If lifting heavy object, but player is dragging upwards, then tree doesn't grow
						if (liftingHeavyObj) {
							//Do nothing
						} else {
							//If not lifting heavy object, and player is dragging upwards, then grow.
							Grow ();
						}
					} else if (mainCam.deltaY < 0f) {
						//If player is dragging down, decay. Regardless of heavy object.
						Decay ();
					} else if (mainCam.deltaY == 0.0f) {
						//Player isn't moving finger, don't grow or decay

						//If lifting heavy object, and player isn't moving finger, then decay.
						if (liftingHeavyObj) {
							Decay ();
						}
					}

				} else {
					//Player has let go of screen. Tree follows last input.
					if (lastInput > 0f) {
						Grow ();
					} else if (lastInput < 0f) {
						Decay ();
					} else if (lastInput == 0f) {
						Debug.Log ("last input of tree is 0f. This shouldn't be possible. Error");
						Decay ();
					}

				}
			} else {
				//Tree is inactive. Should only be false when tree is at max Y or min Y
			}
		} else {

			//Tree is not in sunlight
			Decay ();
		}
	}
	
	void Grow ()
	{
		platform.GetComponent<Rigidbody> ().MovePosition (platform.position + transform.up * Time.deltaTime * growSpeed);
		
		if ((Mathf.Abs (groundY - platform.localPosition.y) < 0.05f) || (platform.localPosition.y < groundY)) {
			AttachKid ();
		}
		
		if ((Mathf.Abs (maxHeightY - platform.localPosition.y) < 0.05f) || (platform.localPosition.y > maxHeightY)) 
		{
			platform.localPosition = new Vector3 (platform.localPosition.x, maxHeightY, platform.localPosition.z);
//			growing = false;
//			decaying = true;
//			timer = Time.time;
			activate = false;
			
//			//Activate Grab attention at maxHeightY
//			if (!kidAttached) {
//				GetComponent<GrabAttention>().FindClosestPath();
//			}
			
			DetachKid();
		}
	}
	
	void Decay ()
	{
		platform.GetComponent<Rigidbody> ().MovePosition (platform.position - transform.up * Time.deltaTime * decaySpeed);
		
		if ((Mathf.Abs (maxHeightY - platform.localPosition.y) < 0.05f) || (platform.localPosition.y > maxHeightY)) {
			AttachKid ();
		}
		
		if ((Mathf.Abs (groundY - platform.localPosition.y) < 0.05f) || (platform.localPosition.y < groundY)) 
		{
			platform.localPosition = new Vector3 (platform.localPosition.x, groundY, platform.localPosition.z);
//			decaying = false;
//			growing = false;
			activate = false;
			
			DetachKid();
		}
	}
	
	void SunlightDecay() 
	{
		platform.GetComponent<Rigidbody> ().MovePosition (platform.position - transform.up * Time.deltaTime * decaySpeed);
		
		if ((Mathf.Abs (groundY - platform.localPosition.y) < 0.05f) || (platform.localPosition.y < groundY)) 
		{
			platform.localPosition = new Vector3 (platform.localPosition.x, groundY, platform.localPosition.z);
//			decaying = false;
//			growing = false;
			activate = false;
			
			DetachKid();
		}
	}
	
	void DetermineKidDistance() {
		//	Debug.Log (Vector3.Distance (kid.transform.position, transform.position) + " " + transform.name);
		
		if (Mathf.Abs(node.position.y - kid.transform.position.y) < 0.1f && Vector3.Distance (kid.transform.position, node.position) < 1.0f) {
			kidInRange = true;
		} else {
			kidInRange = false;
		}
	}
	
	void AttachKid() {
		if (kidInRange && !kidAttached) {
			kid.GetComponent<CharController6> ().Pause (node, false, null);
			kid.transform.parent = platform.transform;
			kidAttached = true;
		}
	}
	
	void DetachKid() {
		if (kidAttached) {
			kid.GetComponent<CharController6> ().Continue (false, null);
			kid.transform.parent = null;
			kidAttached = false;
		}
	}
	
	void FaceCollider() {
		Vector3 camForward = Camera.main.transform.forward;
		camForward = new Vector3 (camForward.x, 0f, camForward.z);
		camForward *= -1f;
		
		touchCollider.transform.forward = camForward;
	}
}
