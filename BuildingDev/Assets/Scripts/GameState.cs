using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	public float gameTime;
	public int playerHealth;
	public bool gameover;
	

	private bool hit;
	private float hitTimer;
	private float sunTimer;

	BuildingControl bc ;
	SilverLining sl;
	SpawnZombie sz;

	// Use this for initialization
	public void Start () {
	
		sunTimer = gameTime;
		playerHealth = 3;
		hit = false;
		gameover = false;
		bc = gameObject.GetComponent<BuildingControl>();

		sl = GameObject.Find ("_SilverLiningSky").GetComponent<SilverLining>();
		sl.minutes = 0;

		sz = gameObject.GetComponent<SpawnZombie>();

	}

	// Update is called once per frame
	void Update () {
		if(bc.go)
		{
			gameTime = Time.time;
			if (gameTime >= (sunTimer + 0.25f) && gameTime < 121)
			{
				//print(sunTimer);
				sl.minutes    += 1;
				sunTimer +=0.25f;
				if(sl.minutes >= 60)
				{
					sl.hour+=1;
					sl.minutes =0;
				}
			}
			//SET LIGHTS TO COME ON BEFORE IT GETS PITCH BLACK?
			if (gameTime == 115)
			{
				print("Street Lights");
			}
			if (gameTime > 30 && gameTime < 60)
			{
				//print("30");
				sz.minZombies = 5;
				sz.maxZombies = 6;
				sz.zombieSpeed = 4;
				sz.timeToSpawn = 3;
			}
			else if (gameTime > 60 && gameTime < 90)
			{
				//print("60");
				sz.minZombies = 5;
				sz.maxZombies = 7;
				sz.zombieSpeed = 4;
				sz.timeToSpawn = 3;
			}
			else if (gameTime > 90 && gameTime < 120)
			{
				//print("90");
				sz.minZombies = 6;
				sz.maxZombies = 7;
				sz.zombieSpeed = 6;
				sz.timeToSpawn = 2;
			}
			else if (gameTime > 120 && gameTime < 150)
			{
				//print("120");
				sz.minZombies = 7;
				sz.maxZombies = 9;
				sz.zombieSpeed = 6;
				sz.timeToSpawn = 2;
			}
			else if (gameTime > 150)
			{
				sz.minZombies = 7;
				sz.maxZombies = 10;
				sz.zombieSpeed = 7;
				sz.timeToSpawn = 2;

				//print("150");
			}
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
		gameover = true;
		//for(current buildings)
		//{
			//reverse building anmation
		//}
		//Show gameover GUI
			//Score == time alive
			//try again btn
	}
}
