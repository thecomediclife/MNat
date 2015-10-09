using UnityEngine;
using System.Collections;

public class Level57Blueprint : MonoBehaviour {
	public RangedEnemyAnimController rangedEnemy;

	public Transform lever, crate, enemy2, kid, rangedEnemyTransform;

	private bool enemykilled;

	// Use this for initialization
	void Start () {
		lever.GetComponent<Animator> ().speed = 0f;
		rangedEnemy.targetFound = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (crate.transform.position.y < 3f) {
			lever.GetComponent<Animator>().speed = 1f;
		}

		if (!enemy2.gameObject.activeInHierarchy && !enemykilled) {
			rangedEnemy.targetFound = false;
			enemykilled = true;
		}

		if (Mathf.Abs (kid.position.y - 4f) < 0.1f || Mathf.Abs (kid.position.y - 7f) < 0.1f) {
			rangedEnemy.targetFound = true;

			if (Mathf.Abs (kid.position.y - 7f) < 0.1f) {
				rangedEnemyTransform.forward = new Vector3 (0f, 0f, -1f);
			}
		} else if (enemykilled) {
			rangedEnemy.targetFound = false;
		}
	}
}
