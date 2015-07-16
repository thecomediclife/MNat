using UnityEngine;
using System.Collections;

public class SpawnTree : MonoBehaviour {
	
	public GameObject tree;
	private GameObject instTree;
	float treeSizeY;
	private Rigidbody rb;
	public RaycastHit hit;
//	public float spawnSpeed = 15f;


	[HideInInspector]
	public Vector3 startPos;
	[HideInInspector]
	public Vector3 spawnPos;
	

	void Update() 	
	{
		int layerMask = 1 << 9;

		if (Input.GetButtonDown("Fire1")) 
		{
			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			//	Shoot raycast, ignore all layers except layer 8 (Ground)
			if (Physics.Raycast(point, Camera.main.transform.forward, out hit, Mathf.Infinity, layerMask))
			{
				if(hit.collider.tag == "Dirt") {

					float xPos = hit.collider.transform.position.x;
					float yPos = hit.collider.bounds.max.y;
					float zPos = hit.collider.transform.position.z;
					spawnPos = new Vector3(xPos,yPos,zPos);

//					float treeHeight = tree.transform.Find("TreeTop").transform.position.y;

//					startPos = spawnPos - new Vector3 (0, treeHeight, 0);
					instTree = Instantiate (tree, spawnPos, Quaternion.identity) as GameObject;

					instTree.GetComponent<TreeController>().activateLift = true;

//					instTree.GetComponent<TreeController>().spawnPos = spawnPos;
				}
				else {

				}
			}
		}
	}
}
