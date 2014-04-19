using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

	private bool playing, gameOver;

	// Use this for initialization
	void Start () {
		playing = gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onGUI ()
	{
		if(playing)
		{
			//loop number of lives images to screen
				// gui image pos = top left
				// top left + image & padding
		}
		else if (gameOver)
		{

		}
	}
}
