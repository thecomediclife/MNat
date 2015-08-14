using UnityEngine;
using System.Collections;

public class Level3Gate : MonoBehaviour {
	public bool open, key1, key2 = false;

	private Transform door1;
	private Transform door2;

	// Use this for initialization
	void Start () {
		door1 = transform.Find ("Door1");
		door2 = transform.Find ("Door2");
	}
	
	// Update is called once per frame
	void Update () {
		if (key1 && key2)
			open = true;

		if (open)
			OpenDoors ();

	}

	void OpenDoors() {
		if (door2.transform.rotation.eulerAngles.y < 45f) {
			door1.transform.Rotate (new Vector3 (0, -5f, 0) * Time.deltaTime);
			door2.transform.Rotate (new Vector3 (0, 5f, 0) * Time.deltaTime);
		}
	}
}
