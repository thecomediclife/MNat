using UnityEngine;
using System.Collections;

public class Level13Blueprint : MonoBehaviour {
	public Transform layer1, layer2, layer3;

	public bool layer1Activate, layer2Activate, layer3Activate;

	public BombScript bomb;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			layer1Activate = true;
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			layer2Activate = true;
		}

		if (Input.GetKeyDown (KeyCode.C)) {
			layer3Activate = true;
		}

		if (layer1Activate) {
			for (int i = 0; i < layer1.childCount; i++) {
				layer1.transform.GetChild(i).position = Vector3.MoveTowards(layer1.transform.GetChild(i).position, Vector3.zero, Random.Range(1f, 5f) * Time.deltaTime);
			}
		}

		if (layer2Activate) {
			for (int i = 0; i < layer2.childCount; i++) {
				layer2.transform.GetChild(i).position = Vector3.MoveTowards(layer2.transform.GetChild(i).position, Vector3.zero, Random.Range(1f, 5f) * Time.deltaTime);
			}
		}

		if (layer3Activate) {
			for (int i = 0; i < layer3.childCount; i++) {
				layer3.transform.GetChild(i).position = Vector3.MoveTowards(layer3.transform.GetChild(i).position, Vector3.zero, Random.Range(1f, 5f) * Time.deltaTime);
			}
		}

		if (Mathf.Abs (bomb.transform.position.y - 19f) < 0.5f) {
			bomb.SetActive();
		}

		if (Mathf.Abs (bomb.transform.position.y - 18f) < 0.5f && bomb.detonate) {
			layer1Activate = true;
		}

		if (Mathf.Abs (bomb.transform.position.y - 12f) < 0.5f && bomb.detonate) {
			layer2Activate = true;
		}

		if (Mathf.Abs (bomb.transform.position.y - 9f) < 0.5f && bomb.detonate) {
			layer3Activate = true;
		}

		if (bomb.detonate) {
			bomb.Reset();
			bomb.transform.position = new Vector3(-1f,23f,-2f);
		}
	}
}
