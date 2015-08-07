using UnityEngine;
using System.Collections;

public class SunController1 : MonoBehaviour {
	
	public bool inSunLight = true;
	public float sunRaySize = 1;
	
	void Update () 
	{
		int treeLayerMask = 1 << 11;
		Vector3 p1 = transform.position + Vector3.up * 1000;
		Vector3 p2 = transform.position - Vector3.up * 1000;

		//	SphereCast
//		RaycastHit[] hits = Physics.SphereCastAll (transform.position, sunRaySize, transform.forward, Mathf.Infinity, treeLayerMask);

		//	CapsuleCast
		RaycastHit[] hits = Physics.CapsuleCastAll (p1, p2, sunRaySize, transform.forward, Mathf.Infinity, treeLayerMask);

		for (int i = 0; i < hits.Length; i++) 
		{
			hits[i].collider.GetComponent<TreeController5>().inSunLight = inSunLight;
		}
	}
}
