using UnityEngine;
using System.Collections;

public class Level6Blueprint : MonoBehaviour {
	//THIS SCRIPT IS ONLY FOR LEVEL 6, ONLY WORKS FOR LEVEL 6, AND LEVEL 6 CAN'T WORK WITHOUT IT. Level blueprint

	public Transform goal1, goal2, goal3;

	public Transform icon1, icon2, icon3;

	public Transform door;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (goal1.GetComponent<Level6GoalTrigger> ().triggered) {
			icon1.GetComponent<Renderer>().material.color = Color.cyan;
		}

		if (goal2.GetComponent<Level6GoalTrigger> ().triggered) {
			icon2.GetComponent<Renderer>().material.color = Color.cyan;
		}

		if (goal3.GetComponent<Level6GoalTrigger> ().triggered) {
			icon3.GetComponent<Renderer>().material.color = Color.cyan;
		}

		if (goal1.GetComponent<Level6GoalTrigger> ().triggered && goal2.GetComponent<Level6GoalTrigger> ().triggered && goal3.GetComponent<Level6GoalTrigger> ().triggered) {
			door.transform.localPosition = Vector3.MoveTowards(door.transform.localPosition, new Vector3(-6f,-1f,3f), 1f * Time.deltaTime);
		}
	}
}
