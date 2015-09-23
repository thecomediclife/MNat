using UnityEngine;
using System.Collections;

public class Level9GameController : MonoBehaviour {
    public Transform kid, icon1, icon2, crate1, crate2, path, door;

    public Vector3 spawnPoint1 = new Vector3(-6f, 8f, -5f);
    public Vector3 spawnPoint2 = new Vector3(0f, 13f, 0f);

    public bool complete = false;
    public bool button1Pressed, button2Pressed;

    private Vector3 buttonVec1 = new Vector3(-10f, 1f, 0f);
    private Vector3 buttonVec2 = new Vector3(3f, 11f, -5f);

    public Material redMat, tealMat;

	// Use this for initialization
	void Start () {
        kid = GameObject.Find("Kid").transform;
	}

    // Update is called once per frame
    void Update() {
        if (Vector3.Distance(crate1.position, buttonVec1) < 0.1f || Vector3.Distance(crate2.position, buttonVec1) < 0.1f || Vector3.Distance(kid.position, buttonVec1) < 0.1f)
        {
            button1Pressed = true;
        } else
        {
            button1Pressed = false;
        }

        if (Vector3.Distance(crate1.position, buttonVec2) < 0.1f || Vector3.Distance(crate2.position, buttonVec2) < 0.1f || Vector3.Distance(kid.position, buttonVec2) < 0.1f)
        {
            button2Pressed = true;
        }
        else
        {
            button2Pressed = false;
        }

        if (button1Pressed && button2Pressed)
        {
            complete = true;
        } else
        {
            complete = false;
        }

        if (button1Pressed)
        {
            icon1.GetComponent<Renderer>().material = tealMat;
        } else
        {
            icon1.GetComponent<Renderer>().material = redMat;
        }

        if (button2Pressed)
        {
            icon2.GetComponent<Renderer>().material = tealMat;
        } else
        {
            icon2.GetComponent<Renderer>().material = redMat;
        }

        if (complete || Input.GetKey(KeyCode.Z))
        {
            path.position = Vector3.MoveTowards(path.position, new Vector3(-10f, 4.46f, 0f), 2f * Time.deltaTime);
            door.position = Vector3.MoveTowards(door.position, new Vector3(-11f, 21f, 0f), 2f * Time.deltaTime);
        } else
        {
            path.position = Vector3.MoveTowards(path.position, new Vector3(-14f, 4.46f, 0f), 2f * Time.deltaTime);
            door.position = Vector3.MoveTowards(door.position, new Vector3(-11f, 17f, 0f), 2f * Time.deltaTime);
        }
    }
}
