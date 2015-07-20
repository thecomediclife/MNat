using UnityEngine;
using System.Collections;

public class TreeControllerTest1 : MonoBehaviour {

	public Transform boy;
	public Transform platform;
	public bool boyOnPlatform;
	public bool activating = false;
	public float percent = 0;
	public float platformSpeed = 1f;

	private Vector3 groundPos;
	private Vector3 startPos;
	private Vector3 endPos;

	private float timer;
	public float interval = 2;

	void Start ()
	{
		groundPos = platform.position;
		endPos = platform.position + new Vector3 (0, 3, 0);

		activating = true;
	}

	void Update () 
	{
		if (boyOnPlatform && activating) {
			boy.parent = platform.transform;
		} else
			boy.parent = null;





		if (activating) 
		{
			percent += platformSpeed * Time.deltaTime;
			platform.position = Vector3.Lerp (groundPos, endPos, percent);
			
			if (Vector3.Distance(platform.position, endPos) < 0.1f)
			{
				platform.position = groundPos;
				percent = 0;
				activating = false;
				StartCoroutine(WaitWhat(4));
			}
		}
	}



//	void ActivatePlatform () 
//	{
//		activating = true;
//		
//		percent += platformSpeed * Time.deltaTime;
//		platform.position = Vector3.Lerp (groundPos, endPos, percent);
//		
//		if (Vector3.Distance(platform.position, endPos) < 0.01f)
//		{
//			platform.position = groundPos;
//			percent = 0;
//			activating = false;
//		}
//	}
	

	IEnumerator WaitWhat (float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		activating = true;
	}
}
