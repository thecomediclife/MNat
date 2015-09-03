using UnityEngine;
using System.Collections;

public class BlockRandomizer : MonoBehaviour {

	public int minimum = 1;
	public int maximum = 10;
	public Texture2D[] decals;

	void Start () 
	{
		//	Random y position
		int rando1 = Random.Range (minimum, maximum);
		if (rando1 == 5) {
			float yPos = transform.position.y;
			yPos += Random.Range (-0.1f, 0.1f);
			transform.position = new Vector3 (transform.position.x, yPos, transform.position.z);
		}

		//	Random potation
		int rando2 = Random.Range (minimum, maximum);
		if (rando2 == 5 || rando2 == 5)
			transform.Rotate (Random.Range (-5, 5), Random.Range (-10, 10), Random.Range (-5, 5), Space.World);

		//	Random texture
		int rando3 = Random.Range (minimum, maximum);
		if (rando3%5 == 0)
			GetComponent<Renderer>().material.SetTexture ("_MainTex", decals[Random.Range(0, decals.Length)]);

		print (rando3);
	}
}
