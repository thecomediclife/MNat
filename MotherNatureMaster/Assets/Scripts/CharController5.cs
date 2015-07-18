using UnityEngine;
using System.Collections;

public class CharController5 : MonoBehaviour {

	public bool moving;
	public int moveAction;
	public int pauseAction;
	


	void Update () 
	{
		if (moving) 
		{

			switch (moveAction)
			{
			case 1:
				break;
			case 2:
				break;
			default:
				break;
			}

		} 
		else //	(!moving)
		{
			PauseAndSnap ();

			switch (pauseAction)
			{
			case 1:
				break;
			case 2:
				break;
			default:
				//	Do nothing
				break;
			}
		}
	}

	void PauseAndSnap () 
	{

	}

}
