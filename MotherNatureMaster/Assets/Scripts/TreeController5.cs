using UnityEngine;
using System.Collections;

public class TreeController5 : MonoBehaviour {

	private Transform kid;
	private Transform platform;
	private Transform node;
	private Transform trunk;
	private Transform color;

	public float deltaY;
	public float treeHeight = 6;	
	public float groundY;
	public float maxHeightY;

	public float growSpeed = 2f;
	public float decaySpeed = 2f;

	private float timer;
	public float waitTime = 2;

	public bool activate;
	public bool dragging;
	public bool growing;
	public bool decaying;
	public bool liftingHeavyObj;

	void Awake()
	{
		trunk = transform.Find ("Trunk");
		platform = transform.Find ("Platform");
		node = platform.Find ("Node");
		color = transform.Find ("Color");
//		kid = GameObject.Find ("Kid").transform;

		groundY = platform.localPosition.y;
		maxHeightY = groundY + treeHeight;

		//	Update the box collider to match the maximum tree height that player inputs
		BoxCollider boxCollider = GetComponent<BoxCollider> () as BoxCollider;
		boxCollider.size = new Vector3 (1, treeHeight, 1);
		boxCollider.center = new Vector3 (0, treeHeight / 2, 0);

		//	TEMPORARY: Provides color box to show where player touch screen to move tree
		color.localScale = new Vector3 (1, treeHeight, 1);
		color.localPosition = new Vector3 (0, treeHeight / 2, 0);
	}

	void Update ()
	{
		//	TEMPORARY: Creates a tree that scales with the platform's position
		trunk.localScale = new Vector3 (trunk.localScale.x, platform.localPosition.y + 0.5f, trunk.localScale.z); 
		trunk.localPosition = new Vector3 (trunk.localPosition.x, platform.localPosition.y / 2, trunk.localPosition.z);

		//	Disable node while tree is moving
		if (platform.localPosition.y == groundY || platform.localPosition.y == maxHeightY)
			node.gameObject.SetActive (true); 
		else
			node.gameObject.SetActive (false);

		print (liftingHeavyObj);
	}

	void FixedUpdate ()
	{
		if (activate) {
			
			switch (dragging) {
				
			//	If player is touching the screen
			case true:

				growing = false; 
				decaying = false;

				if (deltaY > 0 && platform.localPosition.y <= maxHeightY)
				{
					platform.GetComponent<Rigidbody>().MovePosition(platform.localPosition + transform.up * Time.deltaTime);
				}
				break;
				
			//	If player is no longer touching the screen
			case false:
				
				switch (liftingHeavyObj) 
				{
					
				//	If tree is lifting nothing or a light object
				case (false):
					switch (growing) {
					case true:
						Grow ();
						break;
					case false:
						if (decaying)
						{
							if (Time.time > timer + waitTime)
								Decay ();
						}
						break;
					}
					break;
					
				//	If tree is lifting a heavy object
				case (true):
					switch (growing) {
					case true:
						growing = false;
						decaying = true;
						//	Because tree is lifting a heavy object, tree cannot grow and therefore decays immediately
						break;
					case false:
						if (decaying)
						{
							Decay ();
						}
						break;
					}
					break;
				}
				break;
			}
		}
	}

	void Grow ()
	{
		platform.GetComponent<Rigidbody> ().MovePosition (platform.localPosition + transform.up * Time.deltaTime * growSpeed);

		if ((Mathf.Abs (maxHeightY - platform.localPosition.y) < 0.05f) || (platform.localPosition.y > maxHeightY)) 
		{
			platform.localPosition = new Vector3 (platform.localPosition.x, maxHeightY, platform.localPosition.z);
			growing = false;
			decaying = true;
			timer = Time.time;
		}
	}

	void Decay ()
	{
		platform.GetComponent<Rigidbody> ().MovePosition (platform.localPosition - transform.up * Time.deltaTime * decaySpeed);

		if ((Mathf.Abs (groundY - platform.localPosition.y) < 0.05f) || (platform.localPosition.y < groundY)) 
		{
			platform.localPosition = new Vector3 (platform.localPosition.x, groundY, platform.localPosition.z);
			decaying = false;
			activate = false;
		}
	}
}
