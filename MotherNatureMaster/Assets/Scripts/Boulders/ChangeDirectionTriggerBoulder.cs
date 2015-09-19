using UnityEngine;
using System.Collections;

public class ChangeDirectionTriggerBoulder : MonoBehaviour {
    public Vector3 newDirection;
	public bool newAwayDir;
	public Vector3 newAwayDirection;

	void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BoulderScript1>() != null)
        {
			other.GetComponent<BoulderScript1>().ChangeDirection(newDirection, newAwayDir, newAwayDirection);
        }
    }
}
