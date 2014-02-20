using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingControl : MonoBehaviour {

	
	public List<GameObject> buildings;//List of buildings game can spawn
	public List<GameObject> currentbuildings;//Current on screen buildings
	public GameObject newBuilding;//Holder for a new building
	
	public int lastSpawn;
	public int pickBuilding;
	
	
	public float Timer;
	
	
	//Buildings//
	public GameObject building1;
	public GameObject building2;
	public GameObject building3;
	//--------// 
	
	public Vector2 startLable;
	public string startLableText;
	
	public bool gameStarted; //Has player pressed space?
	public bool go; //GamePlaing
	
	public float animLoop;
	public bool animFinished;
	
	public int noBuildings;
	public int scrollSpeed = 3;
	
	public Vector3 spawnLocation;
	
	// Use this for initialization
	void Start () {
	
		animLoop = 2;
		lastSpawn = 50;//Set last spawn so there is a compare value
		animFinished = false;
		go = false;
		startLableText = "Press space to start...";
		startLable = new Vector2(Screen.width/2-50, Screen.height/2-25);
		
		//Add Building Objects to List//
		buildings.Add(building1);
		buildings.Add(building2);
		buildings.Add(building3);
		buildings.Add(building1);
		buildings.Add(building2);
		buildings.Add(building3);
		//----------------------------// 
		
		noBuildings = buildings.Count;
		spawnLocation = new Vector3 (-1000,500,1000);
		
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
			animLoop +=2;
			if(animLoop == 20)
			{
				animFinished = true;
				print("FINSIHED!");
			}
		}
		//
		if (Input.GetKey (KeyCode.Space) && gameStarted == false && animFinished)
		{
			gameStarted = go = true;	
			//Hide Lable
			startLable = new Vector2(-1000,-1000);
		}
		//Pause Game
		if (Input.GetKeyDown (KeyCode.P) && gameStarted)
		{
			go = !go;	
			startLable = new Vector2(-1000,-1000);
			if (!go)
			{
				startLableText = "Game Paused";
				//Show Label
				startLable = new Vector2(Screen.width/2-50, Screen.height/2-25);
			}
		}
		//If Game Running
		if(go)
		{
			print(noBuildings);
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
					currentbuildings[i].transform.Translate(0,0,-scrollSpeed);
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
		GUI.Label (new Rect (startLable.x, startLable.y, 130, 60), startLableText);	
		}
	}
	//<--- END GUI --->//
	
	void animBuildings()
	{
		print("animBuilding");
		for(int i = 0; i < 2; i++)
		{
			print(spawnLocation.z);
			while (pickBuilding == lastSpawn)
			{
				pickBuilding = Random.Range(0, noBuildings);
			}
			//Replace last picked building
			lastSpawn = pickBuilding;
					
				
			newBuilding  = Instantiate (buildings[pickBuilding], spawnLocation, Quaternion.identity) as GameObject;
			currentbuildings.Add(newBuilding);
			
			spawnLocation.x += 2000;
		}
		//Change Spawn Location
		spawnLocation.z += 1500;
	}
	
	void SpawnBuild (int listIndex, float xPos) {
		
		//Building Spawn Location
		spawnLocation = new Vector3(xPos,500,13250);	
		
		//Random pick Building
		int pickBuilding = Random.Range(0, noBuildings);
		
		//Instantiate Building
		newBuilding  = Instantiate (buildings[pickBuilding], spawnLocation, Quaternion.identity) as GameObject;
		currentbuildings.Add(newBuilding);
	}
	//<---END SPAWNBUILD--->
}
