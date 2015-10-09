using UnityEngine;
using System.Collections;

public class Level13Blueprint : MonoBehaviour {
	public Transform layer1, layer2, layer3;

	public bool layer1Activate, layer2Activate, layer3Activate;

	public BombScript3 bomb;

	public Transform node1,node2,node3;

	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown (KeyCode.Z)) {
//			layer1Activate = true;
//		}
//
//		if (Input.GetKeyDown (KeyCode.X)) {
//			layer2Activate = true;
//		}
//
//		if (Input.GetKeyDown (KeyCode.C)) {
////			layer3Activate = true;
//		}

		if (layer1Activate) {
			for (int i = 0; i < layer1.childCount; i++) {
				layer1.transform.GetChild(i).position = Vector3.MoveTowards(layer1.transform.GetChild(i).position, new Vector3(layer1.transform.GetChild(i).position.x, 0f, layer1.transform.GetChild(i).position.z), Random.Range(1f, 5f) * Time.deltaTime);
			}
		}

		if (layer2Activate) {
			for (int i = 0; i < layer2.childCount; i++) {
				layer2.transform.GetChild(i).position = Vector3.MoveTowards(layer2.transform.GetChild(i).position, new Vector3(layer2.transform.GetChild(i).position.x, 0f, layer2.transform.GetChild(i).position.z), Random.Range(1f, 5f) * Time.deltaTime);
			}

		}

		if (layer3Activate) {
			for (int i = 0; i < layer3.childCount; i++) {
				layer3.transform.GetChild(i).position = Vector3.MoveTowards(layer3.transform.GetChild(i).position, Vector3.zero, Random.Range(1f, 5f) * Time.deltaTime);
			}
		}

		if (Mathf.Abs (bomb.transform.position.y - 17f) < 0.5f) {
			bomb.SetActive();
		}

		if (Mathf.Abs (bomb.transform.position.y - 16f) < 0.5f && bomb.detonate) {
			layer1Activate = true;
		}

		if (Mathf.Abs (bomb.transform.position.y - 12f) < 0.5f && bomb.detonate) {
			layer2Activate = true;
		}

		if (Mathf.Abs (bomb.transform.position.y - 9f) < 0.5f && bomb.detonate) {
//			layer3Activate = true;
		}

		if (bomb.detonate && bomb.activate) {
			bomb.Reset(new Vector3(0f,25f,-2f));
//			bomb.transform.position = new Vector3(0f,23f,-2f);
		}
	}
}
