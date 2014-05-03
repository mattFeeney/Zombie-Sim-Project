using UnityEngine;
using System.Collections;

public class PlayMove : MonoBehaviour {

	// Use this for initialization
	
	
	private bool isKeyboard;
	private bool isSFloor;
	private float PlayerX;
	private float PlayerY;

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
	UDPReceive receive;
	Settings settings;
	
	void Start () {


		Screen.SetResolution(1600, 2400, false);

		isKeyboard = true;
		isSFloor = false;
		movementScore = 0;

		PlayerX = 0;

		Shaking = false; 

		//Link Game State script	
		gs = GameObject.Find ("GameController").GetComponent<GameState>();
		bc = GameObject.Find ("GameController").GetComponent<BuildingControl>();
		receive = GameObject.Find ("SmartFloor Prefab").GetComponent<UDPReceive>();
		settings = GameObject.Find("SmartFloor Prefab").GetComponent<Settings>();




	}

	// Update is called once per frame
	void Update () {
		if(bc.go)
		{	
			PlayerX = receive.resultData[0].x_cop_mm;
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

				this.transform.Translate(-receive.resultData[0].x_cop_mm,0,0);
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

	void OnGUI()
	{
		GUI.Label(new Rect(10,10, 500,100), PlayerX.ToString());
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
