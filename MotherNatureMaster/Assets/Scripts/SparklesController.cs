using UnityEngine;
using System.Collections;

public class SparklesController : MonoBehaviour {
	
	public float deathTime = 2;
	public Transform sparkles;
	private Transform sparklesInst;
	private bool dragging;

	void Start () 
	{
		sparklesInst = Instantiate (sparkles, transform.position, Quaternion.identity) as Transform;
		sparklesInst.gameObject.SetActive (false);
	}

	void Update () 
	{
		if (Input.GetButtonDown ("Fire1")) 
		{
			int groundLayerMask = 1 << 9;
			RaycastHit hitDirt;
			Vector3 point = Camera.main.ScreenToWorldPoint (Input.mousePosition + new Vector3 (0, 0, 1));
			
			if (Physics.Raycast (point, Camera.main.transform.forward, out hitDirt, Mathf.Infinity, groundLayerMask)) 
			{	
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) 
				{	
					sparklesInst.gameObject.SetActive (true);
					sparklesInst.transform.position = hit.point;
				}

				dragging = true;

			}
		}

		if (dragging) 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) 
			{	
				sparklesInst.transform.position = hit.point;
			}
			
			if (Input.GetButtonUp ("Fire1")) 
			{				
				dragging = false;
				sparklesInst.gameObject.SetActive (false);
			}
		}
	}
}
