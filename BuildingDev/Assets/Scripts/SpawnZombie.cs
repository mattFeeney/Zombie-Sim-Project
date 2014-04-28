using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnZombie : MonoBehaviour {
	
	private float zombieTimer;
	private float zombieSpawn = 0; //Time till next wave is spawned
	private List<GameObject> zombies;//Zombies objects to use when spawning
	private List<GameObject> currentZombies;
	private List<Vector3>  spawnLocations;
	private List <int> zombieWave;//Holder for zombie locations in current wave
	public GameObject newZombie;//Holder for a new zomzbie
	public int pickZombie;
	public int noZombies;
	public int location;
	public float zombieSpeed;
	public Vector3 spawnLocation;


	//Zombies//
	public GameObject zombie1;
	public GameObject zombie2;
	public GameObject zombie3;
	public GameObject zombie4;
	//-------//
	
	BuildingControl bc ;
	
	// Use this for initialization
	void Start() {    
		//Set high to be out of pickable range
		location = 0;
		//
		zombieSpawn = 3;
		zombieSpeed = 3;
		
		//Add Zombies to List
		zombies = new List<GameObject>();
		zombies.Add(zombie1);
		zombies.Add(zombie2);
		zombies.Add(zombie3);
		zombies.Add(zombie4);
		//
		spawnLocations = new List<Vector3>();
		//Store locations in list
		for (int x = -150; x < 180; x += 30) {
			spawnLocations.Add(new Vector3(x, 0, 1500));
		}
		//LINK BUILDING SCRIPT
		bc = gameObject.GetComponent<BuildingControl>();
		currentZombies = new List<GameObject>();
		zombieWave = new List<int>();
	}
	
	// Update is called once per frame
	void Update() {       
		//zombieSpeed = bc.scrollSpeed * 2;
		
		zombieTimer = Time.time;
		
		//UPDATE NUMBER OF ZOMBIES
		noZombies = currentZombies.Count;
		//print(noZombies);
		
		if (bc.go) {
			if (zombieTimer > zombieSpawn) {
				// (Number of zombies, Time added till next spawn)
				spawnZombies(8, 2);
			} else {
				int count = 0;
				while (count < currentZombies.Count) {
					//If zombie is off screen
					if (currentZombies [count].transform.position.z < -200) {
						//Remove Zombie  
						GameObject temp = currentZombies [count];
						currentZombies.Remove(temp);
						Destroy(temp);
						print("Zombie Destroyed");
						//break;
					}
					//Move zombie towards the camera
					else {
						currentZombies [count].transform.Translate(0, -zombieSpeed, 0);
						count += 1;
					}
				}  // end while...
			} // end else zombieTimer>zombieSpawn
		}
	}
	
	void spawnZombies(int noOfZombies, int zombieTimer) {
		zombieWave.Clear(); // empty the zombieWave
		// generate locations to spawn...
		zombieWave.Add(Random.Range(0, 11)); // add first location....       
		for (int i = 1; i < noOfZombies; i++) { // for the remaining number of zombies
			location = Random.Range(0, 11); // generate a random location
			
			while (zombieWave.IndexOf(location) >= 0) {  // check if that value is already on the list
				location = Random.Range(0, 11);  // if it is, try again....
			}
			zombieWave.Add(location); // add the new location to the zombie wave...
		} 
		// generated locations, now setup zombies at those locations...        
		
		for (int i = 0; i < noOfZombies; i++) {
			//pickZombie = 0;
			pickZombie = Random.Range(0, 4);
			Vector3 loc = spawnLocations[zombieWave[i]];
			newZombie = Instantiate(zombies [pickZombie], loc, Quaternion.Euler(270, 0, 180)) as GameObject;
			currentZombies.Add(newZombie);
		}
		// reset zombieSpawn time....
		zombieSpawn = Time.time + zombieTimer;
		//prevLocation = 999;
	} // end spawnZombies+++++++++++
} // end class
