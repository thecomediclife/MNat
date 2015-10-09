using UnityEngine;
using System.Collections;

public class RangedEnemyAnimController : MonoBehaviour {
	//Also spawns projectiles

	public Animator anim;

	public float delay = 1.0f;
	private float timer = 0.0f;

	public bool targetFound;
	public Transform kid;

	public Transform projectile;

	public bool thrown;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
		kid = GameObject.Find ("Kid").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (targetFound) {
			if (Time.time > timer) {
				anim.SetBool ("Throw", true);

				if (thrown && projectile.GetComponent<ProjectileScript> ().reset) {
					timer = Time.time + delay;
					thrown = false;
					anim.SetBool ("Throw", false);
				}
			}
		} else {
			thrown = false;
			anim.SetBool ("Throw", false);
			timer = 0.0f;
		}
	}

	void Throw() {
		projectile.position = transform.position + new Vector3 (0.5f, 1f, 0.23f);
		projectile.GetComponent<Renderer> ().enabled = true;
		projectile.GetComponent<Rigidbody> ().useGravity = true;
		projectile.GetComponent<Rigidbody> ().AddForce (transform.forward * 160f);
		projectile.GetComponent<ProjectileScript> ().reset = false;

		thrown = true;
	}
}
