using UnityEngine;
using System.Collections;

public class Waterfall : MonoBehaviour {

	private Transform RaycastEndPt;
	public float damage = 2f;

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
			case "Kid":
				break;
			case "Tree":
				hit.collider.gameObject.GetComponentInParent<TreeController4> ().takeDamage(damage * Time.deltaTime);
				break;
			default:
				break;
			}
		}
	}
}
