using UnityEngine;
using System;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	
	public float batSpeed = 15;
	private const int resetOffset = 2;
	private const float pOffset = 0.2f;
	private const int batMax = 17;
	private const float halfway = batMax/2f;
	private const int zero = 0;
	private float unitInUnity = 27.75f;
	public int playerNumber = 1;
	private UDPReceive receive;
	private Settings settings;
	
	// Use this for initialization
	void Start () {
		receive = GameObject.Find("Smart Floor").GetComponent<UDPReceive>();
		settings = GameObject.Find("Smart Floor").GetComponent<Settings>();
		playerNumber = playerNumber -1;
		float tempUnit = settings.Units / 3;
		
		if(tempUnit != 0)
			unitInUnity = unitInUnity * tempUnit; //depending on how many panels it will be calculated here
	}
	
	// Update is called once per frame
	void Update () {
		float batPos = transform.position.y; 
		float playerPos = receive.resultData[playerNumber].y_cop_mm / unitInUnity; 
		float overflow = Math.Abs(halfway - playerPos);
		
		//print("Player Position: " + playerPos + " and bat position: " + batPos);
	
		//The way that the board is setup is back to front so the positions have to be worked out this way
		//the commented code below shows the way when the boards 0,0 is the other way around
		if (batPos > 0 && playerPos > 0 && playerPos != 0
		     && batPos <= batMax && playerPos <= batMax )
		{
			if ((playerPos - pOffset) > halfway && (batPos - pOffset) > (halfway - overflow) 
			    || (playerPos + pOffset) < halfway && (batPos - pOffset) > (halfway + overflow))
				transform.Translate (0, -batSpeed * Time.deltaTime,0);
			
			else if ((playerPos - pOffset) > halfway && (batPos + pOffset) < (halfway - overflow) 
			    || (playerPos + pOffset) < halfway && (batPos + pOffset) < (halfway + overflow))
				transform.Translate (0, batSpeed * Time.deltaTime,0);
		}
		
		//just in case it goes over
		if(batPos > batMax)
			transform.Translate (0, -resetOffset * Time.deltaTime,0);
		if(batPos < 0)
			transform.Translate (0, resetOffset * Time.deltaTime,0);
		
		//This is hardcoding the keyboard controls if there is 2 players 
		//if more are needed then add additional 
		if(playerNumber == 0)
		{
			if (Input.GetKey ("q")) 
				transform.Translate (0, batSpeed * Time.deltaTime,0);
			if (Input.GetKey ("a")) 
				transform.Translate (0, -batSpeed * Time.deltaTime,0);
		}
		
		if (playerNumber == 1)
		{
			if (Input.GetKey ("o")) 
				transform.Translate (0, batSpeed * Time.deltaTime,0);
			if (Input.GetKey ("m")) 
				transform.Translate (0, -batSpeed * Time.deltaTime,0);
		}
		
		/*if (paddlePos > 0 && paddlePos <= paddleMax && playerPos > 0 && 
		    playerPos <= paddleMax && playerPos != 0)
		{
			if(paddlePos > playerPos + playerOffset)
				transform.Translate (0, -paddleSpeed * Time.deltaTime,0);
			else if ((paddlePos < playerPos - playerOffset))
				transform.Translate (0, paddleSpeed * Time.deltaTime,0);
		}*/
	}
}