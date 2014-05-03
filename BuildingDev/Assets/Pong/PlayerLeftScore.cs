using UnityEngine;
using System.Collections;

public class PlayerLeftScore : MonoBehaviour {
	
	public int leftScore = 0;
	private bool winner = false;
	private int fullAddX = 0;
	private int resultLeftPos = 250;
	public AudioClip cheer;
	
	// Use this for initialization
	void Start () 
	{
		if(Screen.fullScreen)	
			fullAddX = 110;
	}
	
	void OnGUI()
	{
		float win=Screen.width*0.6f;
		float w1=win*0.50f;
		
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.UpperLeft;
		style.normal.textColor = Color.white;
		style.fontSize = 50;
		
		GUILayout.BeginArea(new Rect(resultLeftPos + fullAddX, 50, 500, 100));
		GUILayout.BeginHorizontal();
		if(winner)
			GUILayout.Label("P1 WINS", style, GUILayout.Width(w1));
		else
			GUILayout.Label(leftScore.ToString(), style, GUILayout.Width(w1));
		GUILayout.EndHorizontal(); 
		GUILayout.EndArea();
	}
	
	void Update()
	{
		if(leftScore == 5)
		{
			BallMover ball = GameObject.Find("Pong").GetComponent<BallMover>();
			PlayerRightScore otherScored = GameObject.Find("OutOfBoundsLeft").GetComponent<PlayerRightScore>();
			otherScored.rightScore = 0;
			ball.won = true;
			ball.StartPosition();
			leftScore = 0;
			audio.PlayOneShot(cheer);
			resultLeftPos = 150;
			winner = true;
			StartCoroutine(WaitCount(10));
		}
	}
	void OnTriggerEnter()
	{
		leftScore++;
		//have positive min velocity
		BallMover ball = GameObject.Find("Pong").GetComponent<BallMover>();
		ball.minVelocity = 7;
	}
	
	IEnumerator WaitCount(int wait)
	{
		yield return new WaitForSeconds(wait);
		resultLeftPos = 200;
		winner = false;
	}
}
