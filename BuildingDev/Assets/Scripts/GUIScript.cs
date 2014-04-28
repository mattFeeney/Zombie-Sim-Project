using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

	//GUI IMAGES
	public Texture2D heart;
	private int heartPos;
	//

	private bool playing, gameOver;
	private int noLives;	

	//Ref Scripts
	GameState gs;
	BuildingControl bc;

	// Use this for initialization
	void Start () {
		playing = gameOver = false;

		//Link scripts
		gs = gameObject.GetComponent<GameState>();
		bc = gameObject.GetComponent<BuildingControl>();


		//grab player health
		noLives = gs.playerHealth;
		heartPos = 20;
	}

	// Update is called once per frame
	void Update () {
		noLives = gs.playerHealth;
		playing = bc.go;
	}

	void OnGUI ()
	{
		if(playing)
		{
			GUI.Label (new Rect (heartPos, 20, 32, 32), heart);
			for(int i = 0; i < noLives; i++)
			{
				//GUI.Label (new Rect (heartPos, 20, 32, 32), heart);
				//heartPos += 52;
			}
			//loop number of lives images to screen
				// gui image pos = top left
				// top left + image & padding
		}
		else if (gameOver)
		{
			//SHOW GAME OVER IMAGES
			//SHOW SCORE
		}
	}
}
