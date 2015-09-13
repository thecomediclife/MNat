using UnityEngine;
using System.Collections;

public class BoulderSpawn : MonoBehaviour {
    public Transform[] boulderArray = new Transform[5];

    public float delay = 5.0f;
    private float delayTimer = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Time.time > delayTimer)
        {
            delayTimer = Time.time + delay;

            for (int i = 0; i < boulderArray.Length; i++)
            {
                if (boulderArray[i].position == Vector3.zero)
                {
                    boulderArray[i].position = this.transform.position;
                    boulderArray[i].GetComponent<Rigidbody>().useGravity = true;
                    boulderArray[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    break;
                } 
            }
        }
	}
}
