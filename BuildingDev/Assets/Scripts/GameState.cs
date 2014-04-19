using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	public float gameTime;
	public int playerHealth;


	BuildingControl bc ;

	// Use this for initialization
	public void Start () {
	
		playerHealth = 3;
		playing = false;

		bc = gameObject.GetComponent<BuildingControl>();

	}
	
	// Update is called once per frame
	void Update () {

		if(bc.go)
		{
			gameTime = Time.time;
		}

	}

	public void playerHit()
	{
		print("GAME STATE - Player Hit()");
		//Timer for 1 second to avoid double hit?
		playerHealth --;
		//player.health --
		if (playerHealth == 0)
		{
			gameOver();
		}
		//else
		//{
			//hit animations a go go!
		//}

	}

	void gameOver()
	{
		bc.go = false;
		//for(current buildings)
		//{
			//reverse building anmation
		//}
		//Show gameover GUI
			//Score == time alive
			//try again btn
	}
}
