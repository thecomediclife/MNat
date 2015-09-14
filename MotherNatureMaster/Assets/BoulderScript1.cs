using UnityEngine;
using System.Collections;

public class BoulderScript1 : MonoBehaviour {

    public Vector3 direction = new Vector3(8f, 0f, 0f);
    public Vector3 originalDirection;

    public Vector3 awayDirection = new Vector3(8f, 0f, 0f);

	// Use this for initialization
	void Start () {
        originalDirection = direction;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 direc = direction + new Vector3(0f, -100f, 0f);

        GetComponent<Rigidbody>().AddForce(direc);

        if (transform.position.y < -20f)
        {
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            transform.position = Vector3.zero;
            direction = originalDirection;
        }
    }

    //Changes direction of the boulder rolling. Function is called outside of the script.
    public void ChangeDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    void OnCollisionEnter(Collision collision)
    {
        //When boulder collides with tree, it changes direction to the other way.
        if (collision.transform.tag == "Tree")
        {
            Debug.Log("true");
            direction = awayDirection;
        }
    }
}
