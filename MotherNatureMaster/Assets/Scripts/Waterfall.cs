using UnityEngine;
using System.Collections;

public class Waterfall : MonoBehaviour {

	private Transform RaycastEndPt;


	void Awake () {
		RaycastEndPt = transform.FindChild ("RaycastEndPt");
	}

	void Update () 
	{
		Vector3 direc = RaycastEndPt.position - transform.localPosition;
		RaycastHit hit;
		if (Physics.Raycast (transform.localPosition, direc, out hit)) {

			//	We can switch this in the future to hit's name, layer, etc.
			switch (hit.collider.tag)
			{
			case "Obstacle":
				break;
			case "Player":
				break;
			case "Tree":
				hit.collider.gameObject.GetComponent<TreeHealth>().takeDamage(1f * Time.deltaTime);
				break;
			default:
				break;
			}
		}
	}
}
