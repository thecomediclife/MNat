using UnityEngine;
using System.Collections;

public class MusicTest2 : MonoBehaviour {

    public Transform other;

    private float counter;

    public bool play;

    public float timestamp = 30.83f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (play)
        {
            if (this.GetComponent<AudioSource>().time > timestamp)
            {
                other.GetComponent<AudioSource>().Play();
                play = false;
                other.GetComponent<MusicTest2>().StartCounter();
            }
        }
	}

    public void StartCounter()
    {
        play = true;
    }
}
