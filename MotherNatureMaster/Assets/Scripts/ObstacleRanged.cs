using UnityEngine;
using System.Collections;

public class ObstacleRanged : MonoBehaviour {

	public bool useSightController;
	bool inSight;

	public bool includeGravity;
	public Rigidbody projectile;
	float nextShot = 0.0f;
	public float interval = 1.5f;
	public float shotSpeed = 3.0f;
	public float shotForce = 100f;
	
	void Update () 
	{
		//	Rotate on the XZ place to face the target
		if (GetComponent<ObstacleSight> ().turnsToTarget == true) {
			Vector3 targetPos = GetComponent<ObstacleSight> ().targetPos;
			transform.LookAt (new Vector3 (targetPos.x, transform.position.y, targetPos.z));
		}

		//	Determine if ranged obstacle shoots only when kid is in trigger collider
		if (useSightController) {
			//	Use trigger collider to detect kid. Therefore only shoots when kid is in collider
			inSight = GetComponent<ObstacleSight> ().kidInSight;
		} else {
			//	Don't use trigger collider to detect kid. Therefore continuously shoots
			inSight = true;
		}

		//	Shoot projectile
		if (inSight) {
			if (Time.time > nextShot) {
				nextShot = Time.time + interval;

				Rigidbody shot = Instantiate (projectile, transform.position + transform.forward, Quaternion.identity) as Rigidbody;
				Physics.IgnoreCollision (shot.GetComponent<Collider> (), GetComponent<Collider> ());

				if (includeGravity) {
					//	Projectile motion
					shot.useGravity = true;
				} else {
					//	Straight line
					shot.useGravity = false;
				}

				shot.AddForce (transform.forward * shotForce);
			}
		} else {
		}
	}
}
