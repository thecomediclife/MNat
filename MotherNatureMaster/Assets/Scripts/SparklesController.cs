using UnityEngine;
using System.Collections;

public class SparklesController : MonoBehaviour {

	public bool disappear;
	public float deathTime = 2;

	void Start () {
		disappear = false;
	}

	void Update () 
	{
		print (disappear);

		if (disappear) 
		{
			StartCoroutine(Death (deathTime));
			disappear = false;
		}
	}

	IEnumerator Death(float time)
	{
		yield return new WaitForSeconds (time);
		Destroy (gameObject);
	}


}
