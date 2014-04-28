using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	public float gameTime;
	public int playerHealth;

	private bool hit;
	private float hitTimer;


	BuildingControl bc ;

	// Use this for initialization
	public void Start () {
	
		playerHealth = 3;
		hit = false;
		bc = gameObject.GetComponent<BuildingControl>();

	}
	
	// Update is called once per frame
	void Update () {

		if(bc.go)
		{
			gameTime = Time.time;
		}

		if(gameTime > 180)
		{
			gameOver();
		}
		if(hit)//Stops multiple lives lost at once
		{
			if(gameTime > hitTimer + 1)	hit = false;
		}
	}

	public void playerHit()
	{
		if(!hit)//Stops multiple lives lost at once
		{
			//print("GAME STATE - Player Hit()");
			playerHealth --;
			hit = true;
			hitTimer = gameTime;

			//HIT ANIMATION GUI AND CAMERA SHAKE!
		}
		if (playerHealth == 0)
		{
			gameOver();
		}
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
