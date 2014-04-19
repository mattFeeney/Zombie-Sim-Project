using UnityEngine;
using System.Collections;

public class PlayMove : MonoBehaviour {

	// Use this for initialization
	
	
	private bool isKeyboard;
	private bool isSFloor;
	
	public Vector2 startLabel;
	
	GameState gs;
	
	void Start () {
		
		isKeyboard = true;
		isSFloor = true;
		startLabel = new Vector2(Screen.width/2-50, Screen.height/2-25);

		//Link Game State script	
		gs = GameObject.Find ("GameController").GetComponent<GameState>();


	}

	// Update is called once per frame
	void Update () {
	//---CONTROLLERS---//
		//<---USING KEYBOARD--->//

		if(isKeyboard)
		{
			if(Input.GetKey(KeyCode.LeftArrow) && this.transform.position.x > -150)
			{
				this.transform.Translate(-10,0,0);
			}
			else if(Input.GetKey(KeyCode.RightArrow) && this.transform.position.x < 150)
			{
				this.transform.Translate(10,0,0);
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

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name == "Zombie(Clone)")
		{
			//Move zombie behind camera and SZ will destory it
			col.gameObject.transform.Translate(0, -50, 0);
			gs.playerHit();
		}
	}
}
