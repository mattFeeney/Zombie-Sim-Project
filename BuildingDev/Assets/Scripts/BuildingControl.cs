using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingControl : MonoBehaviour {

	
	public List<GameObject> buildings;//List of buildings game can spawn
	public List<GameObject> currentbuildings;//Current on screen buildings
	public GameObject newBuilding;//Holder for a new building
	
	public int lastSpawn;//Stores last building picked
	public int pickBuilding;//For math.random
	
	//Timers
	public float Timer;
	public float labelTimer;
	
	//Buildings//
	public GameObject building1;
	public GameObject building2;
	public GameObject building3;
	//--------// 
	
	//Zombies//
	public GameObject zombie1;
	public GameObject zombie2;
	public GameObject zombie3;
	public GameObject zombie4;
	//-------//
	
	//Gui stuff
	public Vector2 startLabel;
	public string startLabelText;
	
	
	
	public bool gameStarted; //Has player pressed space?
	public bool go; //GamePlaing
	
	public double animLoop;
	public bool animFinished;
	
	public int noBuildings;
	public int scrollSpeed = 3;
	
	public Vector3 spawnLocation;
	
	// Use this for initialization
	void Start () {
	
		animLoop = 0.5;
		lastSpawn = 50;//Set last spawn so there is a compare value
		animFinished = false;
		go = false;
		startLabelText = "Press space to start...";
		startLabel = new Vector2(Screen.width/2-50, Screen.height/2-25);
		
		//Add Building Objects to List//
		buildings.Add(building1);
		buildings.Add(building2);
		buildings.Add(building3);
		buildings.Add(building1);
		buildings.Add(building2);
		buildings.Add(building3);
		//----------------------------// 
		
		noBuildings = buildings.Count;
		spawnLocation = new Vector3 (-1000,0,1000);
		
	}
    //<-------END START-------> //
	
	// Update is called once per frame
	void Update () {
		//Start Timer
		Timer = Time.time;
		if (Timer > animLoop  && !animFinished)
		{
			spawnLocation.x = -1000;
			animBuildings();
			animLoop +=0.5;
			if(animLoop == 3)
			{
				animFinished = true;
				//print("FINSIHED!");
			}
		}
		//
		if (Input.GetKey (KeyCode.Space) && gameStarted == false && animFinished)
		{
			gameStarted = go = true;	
			//Hide Lable
			startLabel = new Vector2(-1000,-1000);
		}
		//Pause Game
		if (Input.GetKeyDown (KeyCode.P) && gameStarted)
		{
			go = !go;	
			startLabel = new Vector2(-1000,-1000);
			if (!go)
			{
				startLabelText = "Game Paused";
				//Show Label
				startLabel = new Vector2(Screen.width/2-50, Screen.height/2-25);
			}
		}
		if(Input.GetKeyDown(KeyCode.S))
		{
			print("SwitchCon");
			labelTimer = Timer + 2;
			startLabelText = "Controller Switched";
			startLabel = new Vector2(Screen.width/2-50, Screen.height/2-25);
			if(Timer > labelTimer)
			{
				print("if");
				startLabel = new Vector2(-1000,-1000);		
			}
		}
		//If Game Running
		if(go)
		{
			//print(noBuildings);
			noBuildings = buildings.Count;
			for(int i = 0; i < ((noBuildings+3)*2); i++)
			{
				//If building is off screen
				if(currentbuildings[i].transform.position.z < -200)
				{
					float xPos = currentbuildings[i].transform.position.x;
					//Remove Buildings	
					GameObject temp = currentbuildings[i];
					currentbuildings.Remove(currentbuildings[i]);
					Destroy(temp);
					
					//Spawn new build - pass in it's position in the list and x position (which side it's on)
					SpawnBuild(i, xPos);
				}
				//Move buildings towards the camera
				else
				{
					currentbuildings[i].transform.Translate(0,-scrollSpeed,0);
				}
			}
		}
		//Game started - Player has Paused
		if(!go && gameStarted)
		{
			print("Game Paused");	
		}
	}
	//<---END UPDATE---> //
	void OnGUI () 
	{
		if(animFinished)
		{
			GUI.Label (new Rect (startLabel.x, startLabel.y, 130, 60), startLabelText);	
		}
		
	}
	//<--- END GUI --->//
	
	void animBuildings()
	{
		for(int i = 0; i < 2; i++)
		{
			while (pickBuilding == lastSpawn)
			{
				pickBuilding = Random.Range(0, noBuildings);
			}
			//Replace last picked building
			lastSpawn = pickBuilding;
					
				
			newBuilding  = Instantiate (buildings[pickBuilding], spawnLocation, Quaternion.Euler(90, 0, 0)) as GameObject;
			currentbuildings.Add(newBuilding);
			//Add Second building to other side
			spawnLocation.x += 2000;
		}
		//Change Spawn Location
		spawnLocation.z += 1500;
	}
	
	void SpawnBuild (int listIndex, float xPos) {
		
		//Building Spawn Location
		spawnLocation = new Vector3(xPos,0,8250);	//CHANGE Z VALUE SO ITS CORRECT!
		
		//Random pick Building
		int pickBuilding = Random.Range(0, noBuildings);
		
		//Instantiate Building
		newBuilding  = Instantiate (buildings[pickBuilding], spawnLocation, Quaternion.Euler(90, 0, 0)) as GameObject;
		currentbuildings.Add(newBuilding);
	}
	//<---END SPAWNBUILD--->
}
