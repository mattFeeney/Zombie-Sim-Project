using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingControl : MonoBehaviour {

	//List of buildings game can spawn
	public List<GameObject> buildings;
	//Current on screen buildings
	public List<GameObject> currentbuildings;

	//Holder for a new building
	public GameObject newBuilding;
	
	//Buildings//
	public GameObject building1;
	public GameObject building2;
	public GameObject building3;
	// /// /// // 
	
	
	public int noBuildings;
	public int scrollSpeed = 3;
	
	public Vector3 spawnLocation;
	
	// Use this for initialization
	void Start () {
	
		//Add Building Objects to List
		buildings.Add(building1);
		buildings.Add(building2);
		buildings.Add(building3);
		buildings.Add(building1);
		buildings.Add(building2);
		buildings.Add(building3);
		// // // // // // // // // // 
		
		noBuildings = buildings.Count;
		//noBuildings = 30;
		
		spawnLocation = new Vector3 (-1000,500,1000);
		
		//Generate Buildings
		for(int i = 0; i < 2; i++)
		{
			for(int j = 0; j < noBuildings; j++)
			{
				int pickBuilding = Random.Range(0, noBuildings);
				
				//print(pickBuilding);
				
				spawnLocation.z += 1500;
				
				newBuilding  = Instantiate (buildings[pickBuilding], spawnLocation, Quaternion.identity) as GameObject;
				currentbuildings.Add(newBuilding);
			}
			spawnLocation = new Vector3 (1000,500,1000);
			
		}
		
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		//Update number of buildings
		noBuildings = buildings.Count;
		for(int i = 0; i < (noBuildings*2); i++)
		{
			//If building is off screen
			if(currentbuildings[i].transform.position.z < -200)
			{
				//Respawn	
				GameObject temp = currentbuildings[i];
				currentbuildings.Remove(currentbuildings[i]);
				Destroy(temp);
				//Spawn new build, pass in it's position in the list
				SpawnBuild(i);
			}
			//Move buildings towards the camera
			else
			{
				currentbuildings[i].transform.Translate(0,0,-scrollSpeed);
			}
		}
	
	}
	
	void SpawnBuild (int listIndex) {
		
		spawnLocation = new Vector3(-1000,500,8500);	
		
		//Two loops - one for each side
		for(int i = 0; i < 2; i++)
		{		
			int pickBuilding = Random.Range(0, noBuildings);
			
			newBuilding  = Instantiate (buildings[pickBuilding], spawnLocation, Quaternion.identity) as GameObject;
			currentbuildings.Insert(listIndex, newBuilding);
			
			//Switch Spawn sides
			spawnLocation.x += 2000;	
		}
		print(currentbuildings.Count);
	}
}
