using UnityEngine;
using System.Collections;

public class FishController : MonoBehaviour {

	public Vector3 sizeOfSphere;
	private Vector3 nextDest;
	private Vector3 dir;
	private float speed;

	void Start () 
	{
		nextDest = calcDest ();
		speed = 1;
	}

	void Update () 
	{
		transform.localPosition = Vector3.MoveTowards (transform.localPosition, nextDest, speed * Time.deltaTime);

		transform.LookAt (nextDest + transform.parent.transform.position);


		if (Vector3.Distance (transform.localPosition, nextDest) < 0.05) {
			transform.localPosition = nextDest;
			nextDest = calcDest();

			print (nextDest);

			speed = Random.Range(0.2f, 1.5f);
		}
	}

	public Vector3 calcDest() 
	{
		Vector3 newDest = Random.insideUnitSphere;
		float newDestX = newDest.x * sizeOfSphere.x;
		float newDestY = newDest.y * sizeOfSphere.y;
		float newDestZ = newDest.z * sizeOfSphere.z;
		newDest = new Vector3 (newDestX, newDestY, newDestZ);

		return newDest;

	}
}
