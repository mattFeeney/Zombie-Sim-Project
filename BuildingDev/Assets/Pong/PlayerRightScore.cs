using UnityEngine;
using System.Collections;

public class PlayerRightScore : MonoBehaviour {
	
	public int rightScore = 0;
	private bool winner = false;
	private int fullAddX = 0;
	private int resultRightPos = 500;
	public AudioClip cheer;
	
	// Use this for initialization
	void Start () 
	{
		if(Screen.fullScreen)	
			fullAddX = 130;
	}
	
	void OnGUI()
	{
		float win=Screen.width*0.6f;
		float w1=win*0.50f;
		
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.UpperLeft;
		style.normal.textColor = Color.white;
		style.fontSize = 50;
		
		GUILayout.BeginArea(new Rect(resultRightPos + fullAddX, 50, 500, 100));
		GUILayout.BeginHorizontal();
		if(winner)
			GUILayout.Label("P2 WINS", style, GUILayout.Width(w1));
		else
			GUILayout.Label(rightScore.ToString(), style, GUILayout.Width(w1));
		GUILayout.EndHorizontal(); 
		GUILayout.EndArea();
	}
	
	void Update()
	{
		if(rightScore == 5)
		{
			BallMover ball = GameObject.Find("Pong").GetComponent<BallMover>();
			PlayerLeftScore otherScored = GameObject.Find("OutOfBoundsRight").GetComponent<PlayerLeftScore>();
			otherScored.leftScore = 0;
			ball.won = true;
			ball.StartPosition();
			rightScore = 0;
			audio.PlayOneShot(cheer);
			resultRightPos = 450;
			winner = true;
			StartCoroutine(WaitCount(10));
		}
	}
	
	void OnTriggerEnter()
	{
		rightScore++;
		//have positive min velocity
		BallMover ball = GameObject.Find("Pong").GetComponent<BallMover>();
		ball.minVelocity = -7;
	}
	
	IEnumerator WaitCount(int wait)
	{
		yield return new WaitForSeconds(wait);
		resultRightPos = 500;
		winner = false;
	}
}
