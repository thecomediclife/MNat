using UnityEngine;
using System.Collections;

public class ObstacleSight : MonoBehaviour {

	public bool sightCanBeBlocked;
	public bool turnsToTarget;
	public Vector3 targetPos;
	public bool kidInSight;	
	private GameObject kid;
	private SphereCollider col;
	
	void Awake ()
	{
		col = gameObject.GetComponent<SphereCollider>();
		kid = GameObject.Find ("Kid");
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject == kid) 
		{
			switch (turnsToTarget) 
			{
			case true:
				targetPos = other.transform.position;
				break;
			case false:
				break;
			}

			switch (sightCanBeBlocked)
			{
			case true:
				kidInSight = false;
				Vector3 direction = other.transform.position - transform.position;
				RaycastHit hit;
				if(Physics.Raycast(transform.position, direction.normalized, out hit, col.radius)) {
					if(hit.collider.gameObject == kid) {
						kidInSight = true;
					}
				}
				break;
			case false:
				kidInSight = true;
				break;
			}

//			//	KidInSight is true as long as kid is in collider. Obstacle's sight cannot be blocked.
//			if (!sightCanBeBlocked) {
//				kidInSight = true;
//
//			//	KidInSight is false if there is something blocking the obstacle's vision of kid.
//			} else {
//				kidInSight = false;
//				Vector3 direction = other.transform.position - transform.position;
//				RaycastHit hit;
//				if(Physics.Raycast(transform.position, direction.normalized, out hit, col.radius))
//				{
//					if(hit.collider.gameObject == kid)
//					{
//						kidInSight = true;
//					}
//				} 
//			}
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		if (other.gameObject == kid) 
		{
			kidInSight = false;
		}
	}
}
