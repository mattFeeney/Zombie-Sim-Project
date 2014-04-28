using UnityEngine;
using System.Collections;

public class PlayMove : MonoBehaviour {

	// Use this for initialization
	
	
	private bool isKeyboard;
	private bool isSFloor;
	public int movementScore;
	public Vector2 startLabel;

	//Camera Shake Stuff
	public bool Shaking; 
	private float ShakeDecay;
	private float ShakeIntensity;   
	private Vector3 OriginalPos;
	private Quaternion OriginalRot;
	//
	
	GameState gs;
	BuildingControl bc ;
	
	void Start () {
		
		isKeyboard = true;
		isSFloor = false;
		movementScore = 0;

		Shaking = false; 

		//Link Game State script	
		gs = GameObject.Find ("GameController").GetComponent<GameState>();
		bc = GameObject.Find ("GameController").GetComponent<BuildingControl>();


	}

	// Update is called once per frame
	void Update () {
		if(bc.go)
		{	
			//<---USING KEYBOARD--->//
			if(isKeyboard)
			{
				if(Input.GetKey(KeyCode.LeftArrow) && this.transform.position.x > -150)
				{
					this.transform.Translate(-10,0,0);
					movementScore +=1;
				}
				else if(Input.GetKey(KeyCode.RightArrow) && this.transform.position.x < 150)
				{
					this.transform.Translate(10,0,0);
					movementScore +=1;
				}
			}
			//<---USING SMARTFLOOR--->//
			else if (isSFloor)
			{
				//movementScore += movement on floor.round();
			}
			//-----------------//
			if(Input.GetKeyDown(KeyCode.S))
			{
				isKeyboard = !isKeyboard;
				isSFloor = !isSFloor;
			}
			if(ShakeIntensity > 0)
			{
				transform.position = OriginalPos + Random.insideUnitSphere * ShakeIntensity;
				transform.rotation = new Quaternion(OriginalRot.x + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
				                                    OriginalRot.y + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
				                                    OriginalRot.z + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
				                                    OriginalRot.w + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f);
				
				ShakeIntensity -= ShakeDecay;
			}
			else if (Shaking)
			{
				Shaking = false;  
			}
		}
	}

	void OnCollisionEnter (Collision col)
	{
		//If player hit zombie
		if(col.gameObject.name == "Zombie(Clone)")
		{
			//Move zombie behind camera and SZ will destory it
			col.gameObject.transform.Translate(0, -50, 0);
			gs.playerHit();
			DoShake();
		}
	}

	public void DoShake()
	{
		OriginalPos = transform.position;
		OriginalRot = transform.rotation;
		
		ShakeIntensity = 0.3f;
		ShakeDecay = 0.02f;
		Shaking = true;
	}   
}
