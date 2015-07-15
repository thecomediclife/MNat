using UnityEngine;
using System.Collections;

public class FloatingPlatformInverseChild : MonoBehaviour {

	public Transform player;
	public GameObject button;
	Vector3 startPos;

	private float perc;

	void Start () {
		startPos = transform.localPosition;
	}

	void Update () {
		if (!button.GetComponent<ButtonPermanent> ().isClicked) {
			if (player.position.z == 4f && player.position.y == 13f) {
				perc = Mathf.InverseLerp (1, -4, player.position.x);
				transform.localPosition = new Vector3 (transform.localPosition.x, Mathf.Lerp (5, -1, perc), transform.localPosition.z);
			} else {
				transform.localPosition = startPos;
			}
		} else {
			GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
		}


	}
}
