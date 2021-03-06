using UnityEngine;
using System;
using System.Collections;

public class PlayerActions : MonoBehaviour {
	
	private const int framesLimit = 10;
	private const int averageOffset = 30;
	private const int jumpOffset = 100;
	private const float pOffset = 0.2f;
	private const int maxOccurrences = 3;
	
	
	private bool[] bCrouch, bCheckJump, bLanding;  //checking for diff states
	private float[] lastXcord, lastYcord, lastForce;					 //need to see last frames cords
	private static string[] strHorMovement, strVerMovement; 
	
	private int[] countFrames, countOccurrences;
	
	private float averageForce = 0;
	private int playerNo = 0;
	private UDPReceive receive;
	
	public bool displayDebug = false, calculateJump = false;			 //these are set in the inspector
	public string[] strCurrentForce;
	
	// Use this for initialization
	void Start () {
		int noOfPlayers = GetComponent<Settings>().noOfPlayers;
		receive = GameObject.Find("SmartFloor Prefab").GetComponent<UDPReceive>();
		
		//the following need to be arrays due to multiple players
		lastForce = new float[noOfPlayers];
		lastXcord  = new float[noOfPlayers];
		lastYcord  = new float[noOfPlayers];
		strHorMovement = new string[noOfPlayers];
		strVerMovement = new string[noOfPlayers];
		strCurrentForce = new string[noOfPlayers];
		countFrames = new int[noOfPlayers];
		countOccurrences = new int[noOfPlayers];
		bCrouch = new bool[noOfPlayers];
		bCheckJump = new bool[noOfPlayers];
		bLanding = new bool[noOfPlayers];
	}
	
	void OnGUI()
	{
		int directionY = 510;
		
		if(displayDebug)
		{
			Rect rectMoveObj=new Rect(10, directionY, 500,700);
			GUIStyle styleMove = new GUIStyle();
			styleMove.alignment = TextAnchor.UpperLeft;
			styleMove.normal.textColor = Color.white;
			//GUI.Box(rectMoveObj, "P1 Horizontal Direction: " + strHorMovement + "\nP1 Vertical Direction:   " 
			  //      + strVerMovement + "\n" + strCurrentForce, styleMove);
			
			for (int player=0; player < strHorMovement.Length; player++)
			{
				rectMoveObj = new Rect(10, directionY ,500,700);
				GUI.Box(rectMoveObj, "Player " + player + " Horizontal Direction: " + strHorMovement[player] 
				        + "\nPlayer " + player + " Vertical Direction:   " 
				        + strVerMovement[player] + "\n" + strCurrentForce[player], styleMove);
				directionY += 30;
			}
		}
	}
	
	public string JumpDetect(int currPlayer)
    {	
		if(calculateJump)   //if player's jump detection is required enable in the inspector
		{
			playerNo = currPlayer;
			averageForce = UDPReceive.averageForce[playerNo];
			if(StandingStillCheck())
			{
				//standing still now see if came from a jump to land
				if(bLanding[playerNo])
				{
					bLanding[playerNo] = false;
					print("Landed");
					strCurrentForce[playerNo] = "Landed";
				}
			} 
			else if (!bLanding[playerNo])
			{
				//our own global counter here separate from the current frames
				countFrames[playerNo]++;
				
				if (CrouchCheck())
					if (LiftOffCheck())
						AirOrLandCheck();
			}
			
			//to check for a slight miss jump off board
			if(bLanding[playerNo] && receive.resultData[playerNo].force_N == 0 && bCheckJump[playerNo])
			{
				print("Currently off the board");
				strCurrentForce[playerNo] = "Off Board";
				ResetFlags();
			}
			
			//need this to check for consistency
			lastForce[playerNo] = receive.resultData[playerNo].force_N;
		}
		return strCurrentForce[playerNo];
	}
	
	private void ResetFlags()
	{
		bCrouch[playerNo] = false;
		bCheckJump[playerNo] = false;
		countFrames[playerNo] = 0;
		countOccurrences[playerNo] = 0;
	}
	
	private bool StandingStillCheck()
	{
		if(averageForce != 0)
		{
			//check if average movement might need to check a few frames and make sure its consistant
			if(receive.resultData[playerNo].force_N >= (averageForce - averageOffset)
			   && receive.resultData[playerNo].force_N <= (averageForce + averageOffset) 
			   && lastForce[playerNo] >= (averageForce - averageOffset) 
			   && lastForce[playerNo] <= (averageForce + averageOffset))
			{
				strCurrentForce[playerNo] = "Standing";
				ResetFlags();
				return true;
			}
		}
		return false;
	}
	
	private bool CrouchCheck()
	{		
		if(countFrames[playerNo] <= framesLimit && !bCrouch[playerNo] && averageForce != 0)
		{
			//needs to be crouching for a little while
			if(receive.resultData[playerNo].force_N < averageForce - jumpOffset)
				countOccurrences[playerNo]++;
			
			//print("counter = " + countOccurrences + " & max = " + maxOccurrences);
			if(countOccurrences[playerNo] >= maxOccurrences)
			{
				print("Crouching");
				strCurrentForce[playerNo] = "Crouching";
				bCrouch[playerNo] = true;
				countOccurrences[playerNo] = 0;
				return bCrouch[playerNo];
			}
		}
		return bCrouch[playerNo];
	}
	
	private bool LiftOffCheck()
	{	
		//check for increase for lift off
		if (countFrames[playerNo] <= framesLimit * 2 && bCrouch[playerNo] && !bCheckJump[playerNo])
		{
			if(receive.resultData[playerNo].force_N > averageForce + jumpOffset)
				countOccurrences[playerNo]++;
			
			if(countOccurrences[playerNo] >= maxOccurrences)
			{
				print("Lift off");
				strCurrentForce[playerNo] = "Lift Off";
				bCheckJump[playerNo] = true;
				countOccurrences[playerNo] = 0;
				return bCheckJump[playerNo];
			}
			
			//check in case
			if(receive.resultData[playerNo].force_N == 0)
			{
				countOccurrences[playerNo] = 0;
				bCheckJump[playerNo] = true;
				print("Jumping in air");
				strCurrentForce[playerNo] = "Jumping in air";
				countOccurrences[playerNo]++;
				return bCheckJump[playerNo];
			}
		}
		return bCheckJump[playerNo];
	}
	
	private void AirOrLandCheck()
	{
		if (bCrouch[playerNo] && bCheckJump[playerNo])
		{
			if(receive.resultData[playerNo].force_N == 0)
			{
				print("Jumping in air");
				strCurrentForce[playerNo] = "Jumping in air";
				countOccurrences[playerNo]++;
			}
			
			//most prob jumped off board mid jump
			if(countOccurrences[playerNo] >= framesLimit)
			{
				print("Currently off the board");
				strCurrentForce[playerNo] = "Off Board";
				ResetFlags();
				bLanding[playerNo] = true;
			}
	
			//landing
			if(receive.resultData[playerNo].force_N > 0 && countOccurrences[playerNo] >= 1)
			{
				print("Landing");
				strCurrentForce[playerNo] = "Landing";
				bLanding[playerNo] = true;
			}
		}
	}
	
    public void CurrentDirection(int currPlayer)
    {
		//print("current X " + resultData.x_cop_mm + " lastResults " + lastXcord);
		//print("current Force " + resultData.force_N);
		
		if(displayDebug)
		{
			float currX = receive.resultData[currPlayer].x_cop_mm;
			float currY = receive.resultData[currPlayer].y_cop_mm;
			
			if(currX < (lastXcord[currPlayer] - pOffset))
				strHorMovement[currPlayer] = "Player " + currPlayer + " is moving Right";
			else if (currX > (lastXcord[currPlayer] + pOffset))
				strHorMovement[currPlayer] = "Player " + currPlayer + " is moving Left";
			else if(currX < (lastXcord[currPlayer] + pOffset) && currX > (lastXcord[currPlayer] - pOffset))
				strHorMovement[currPlayer] = "Player " + currPlayer + " is Stationary";
			
			//check for Vertical movement
			if(currY < lastYcord[currPlayer])
				strVerMovement[currPlayer] = "Player " + currPlayer + " is moving Forwards";
			else if(currY > lastYcord[currPlayer])
				strVerMovement[currPlayer] = "Player " + currPlayer + " is moving Backwards";
			else
				strVerMovement[currPlayer] = "Player " + currPlayer + " is Stationary";
			
			lastXcord[currPlayer] = currX;
			lastYcord[currPlayer] = currY;
		}
    }
}
