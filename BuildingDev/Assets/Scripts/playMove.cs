using UnityEngine;
using System.Collections;

public class playMove : MonoBehaviour {

	// Use this for initialization
	
	
	public bool isKeyboard;
	public bool isSFloor;
	
	public Vector2 startLabel;
	
	void Start () {
		
		isKeyboard = true;
		isSFloor = true;
		startLabel = new Vector2(Screen.width/2-50, Screen.height/2-25);
		//Grab floor values, translate the player
		// |
		// |
		// V
		//Convert the floor value to game space? 
		//(using on the X for game)
		
		//Use Y value to pause the game???	
		//Maybe if player leaves the floor - zero input???
		
	}
	
	// Update is called once per frame
	void Update () {
	//---CONTROLLERS---//
		//<---USING KEYBOARD--->//
		if(isKeyboard)
		{
			if(Input.GetKey(KeyCode.LeftArrow) && this.transform.position.x > -150)
			{
				this.transform.Translate(-1,0,0);
			}
			else if(Input.GetKey(KeyCode.RightArrow) && this.transform.position.x < 150)
			{
				this.transform.Translate(1,0,0);
			}
		}
		//<---USING SMARTFLOOR--->//
		else if (isSFloor)
		{
			
		}
	//-----------------//
		if(Input.GetKeyDown(KeyCode.S))
		{
			isKeyboard = !isKeyboard;
			isSFloor = !isSFloor;
		}
	}

	//ADD GUI TO SHOW CONTROLLER SWITCH
}
