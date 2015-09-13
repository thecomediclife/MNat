using UnityEngine;
using System.Collections;

public class ChangeDirectionTriggerBoulder : MonoBehaviour {
    public Vector3 newDirection;

	void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BoulderScript1>() != null)
        {
            other.GetComponent<BoulderScript1>().ChangeDirection(newDirection);
        }
    }
}
