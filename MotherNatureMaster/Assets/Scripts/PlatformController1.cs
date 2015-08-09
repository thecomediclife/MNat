using UnityEngine;
using System.Collections;

public class PlatformController1 : MonoBehaviour {

	private Transform tree;

	void Awake () 
	{
		tree = transform.parent;
	}

	void OnCollisionEnter (Collision other)
	{
		if (other.collider.tag == "Heavy") 
		{
			tree.GetComponent<TreeController5>().liftingHeavyObj = true;
		}
	}
	
	void OnCollisionStay (Collision other)
	{
		if (other.collider.tag == "Heavy") 
		{
			tree.GetComponent<TreeController5>().liftingHeavyObj = true;
		}
	}
	
	void OnCollisionExit (Collision other)
	{
		if (other.collider.tag == "Heavy") 
		{
			tree.GetComponent<TreeController5>().liftingHeavyObj = false;
		}
	}
}
