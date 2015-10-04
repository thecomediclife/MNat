using UnityEngine;
using System.Collections;

public class AudioTest : MonoBehaviour {

    public Transform child1;

    public bool played;

    public float timestamp = 12.53f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (this.GetComponent<AudioSource>().time > timestamp && !played)
        {
            child1.GetComponent<AudioSource>().Play();
            child1.GetComponent<MusicTest2>().StartCounter();
            played = true;
        }

        //Debug.Log(this.GetComponent<AudioSource>().time);
	}
}
