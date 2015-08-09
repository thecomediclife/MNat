﻿using UnityEngine;
using System.Collections;

public class TreeController5 : MonoBehaviour {

	private Transform kid;
	private Transform platform;
	private Transform node;
	private Transform trunk;
	private Transform color;

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
		BoxCollider boxCollider = GetComponent<BoxCollider> () as BoxCollider;
		boxCollider.size = new Vector3 (1, treeHeight, 1);
		boxCollider.center = new Vector3 (0, treeHeight / 2, 0);

		//	TEMPORARY: Provides color box to show where player touch screen to move tree
		color.localScale = new Vector3 (1, treeHeight, 1);
		color.localPosition = new Vector3 (0, treeHeight / 2, 0);
	}

	void Update ()
	{
		DetermineKidDistance ();

		//	TEMPORARY: Creates a tree that scales with the platform's position
		trunk.localScale = new Vector3 (trunk.localScale.x, platform.localPosition.y + 0.5f, trunk.localScale.z); 
		trunk.localPosition = new Vector3 (trunk.localPosition.x, platform.localPosition.y / 2, trunk.localPosition.z);

		//  Disable node while tree is moving
		if (Mathf.Abs (platform.localPosition.y - groundY) < 0.05 || Mathf.Abs (platform.localPosition.y - maxHeightY) < 0.05) {
			node.gameObject.SetActive (true);
		} else {
			node.gameObject.SetActive (false);
		}

	}

	void FixedUpdate ()
	{
		if (inSunLight) {

			if (activate) {

//				if (kidInRange && !kidAttached) {
//					AttachKid();
//				}

				switch (dragging) {
			
				//	If player is touching the screen
				case true:

					growing = false; 
					decaying = false;

					if (deltaY > 0 && platform.localPosition.y <= maxHeightY) {
						platform.GetComponent<Rigidbody> ().MovePosition (platform.position + transform.up * Time.deltaTime * growSpeed);
					}

					if (kidInRange && !kidAttached) {
						AttachKid();
					}

					break;
			
				//	If player is no longer touching the screen
				case false:
			
					switch (liftingHeavyObj) {
				
					//	If tree is lifting nothing or a light object
					case (false):

						////////////////////////////
						switch (growing) {
						case true:
							Grow ();
							break;
						case false:
							if (decaying) {
								if (Time.time > timer + waitTime) {
									Decay ();
//									if (kidInRange && !kidAttached)
//										AttachKid();
								}
							}
							break;
						}
						break;
						///////////////////////////

					//	If tree is lifting a heavy object
					case (true):
						switch (growing) {
						case true:
							growing = false;
							decaying = true;
					//	Because tree is lifting a heavy object, tree cannot grow and therefore decays immediately
							break;
						case false:
							if (decaying) {
								Decay ();
							}
							break;
						}
						break;
					}
					break;
				}
			}
		} else {
//			if (activate || growing || decaying)
//			{
//				//	Need somethign that overrides all variables and sets decaying to true and starts Decay ().
//				//	Dragging can be true or false (depending on whether player is dragging or not or trying to drag), the tree
//				//	will decay regardless.
//			}
//
//			if (decaying)
//				Decay ();
		}
	}

	void Grow ()
	{
		platform.GetComponent<Rigidbody> ().MovePosition (platform.position + transform.up * Time.deltaTime * growSpeed);

		if (kidInRange && !kidAttached) {
			AttachKid();
		}

		if ((Mathf.Abs (maxHeightY - platform.localPosition.y) < 0.05f) || (platform.localPosition.y > maxHeightY)) 
		{
			platform.localPosition = new Vector3 (platform.localPosition.x, maxHeightY, platform.localPosition.z);
			growing = false;
			decaying = true;
			timer = Time.time;

			//Activate Grab attention at maxHeightY
			if (!kidAttached) {
				GetComponent<GrabAttention>().FindClosestPath();
			}

			if (kidAttached)
				DetachKid();
		}
	}

	void Decay ()
	{
		platform.GetComponent<Rigidbody> ().MovePosition (platform.position - transform.up * Time.deltaTime * decaySpeed);

		if (kidInRange && !kidAttached) {
			AttachKid();
		}

		if ((Mathf.Abs (groundY - platform.localPosition.y) < 0.05f) || (platform.localPosition.y < groundY)) 
		{
			platform.localPosition = new Vector3 (platform.localPosition.x, groundY, platform.localPosition.z);
			decaying = false;
			growing = false;
			activate = false;

			if (kidAttached)
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
		kid.GetComponent<CharController6> ().Pause (node, false, null);
		kid.transform.parent = platform.transform;
		kidAttached = true;
	}

	void DetachKid() {
		kid.GetComponent<CharController6> ().Continue (false, null);
		kid.transform.parent = null;
		kidAttached = false;
	}

//	void OnTriggerEnter(Collider other) {
//		Debug.Log (other.name);
//	}
}
