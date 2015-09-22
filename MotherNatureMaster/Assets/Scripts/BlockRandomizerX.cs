using UnityEngine;
using System.Collections;

public class BlockRandomizerX : MonoBehaviour {

	public int minimum = 1;
	public int maximum = 10;
	public Texture2D[] decals;

	void Start () 
	{
		//	Random y position
		int rando1 = Random.Range (minimum, maximum);
		if (rando1%2 == 0) {
			float xPos = transform.position.x;
			xPos += Random.Range (-0.1f, 0.2f);
			transform.position = new Vector3 (xPos,transform.position.y, transform.position.z);
		}

		//	Random rotation
//		int rando2 = Random.Range (minimum, maximum);
//		if (rando2%2 == 0)
//			transform.Rotate (Random.Range (-1, 1), Random.Range (-5, 5), Random.Range (-1, 1), Space.World);

		//	Random texture
//		int rando3 = Random.Range (minimum, maximum);
//		if (rando3%2 == 0)
			GetComponent<Renderer>().material.SetTexture ("_MainTex", decals[Random.Range(0, decals.Length)]);
	}
}
