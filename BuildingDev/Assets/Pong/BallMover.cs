using UnityEngine;
using System.Collections;

public class BallMover : MonoBehaviour {
	
	private const float velocityIncrement = 1.0016f; //1.003
	private const float angleIncrement = 0.03f;  //0.15
	private bool bServe = false, bAngle = false;
	private bool countdown = false;
	private bool checkPlayerOnBoard = true;
	private int counter = 5;
	private float lastXVel = 0;
	private int fullAddX = 0;
	private int fullAddY = 0;
	private bool paused = false;
	private Vector3 saveVelocity;
	private Vector3 saveAngularVelocity;
	public float minVelocity = 5;
	private int particleStart = 10; 
	public float maxVelocity = 20;
	public bool won = false;
	private UDPSend send;
	private UDPReceive recieve;
	private Settings settings;
	
	// Use this for initialization
	void Start () {
		//check for jump maybe not cause of 2 people
		//if(player.JumpDetect() == "Landed")
		recieve = GameObject.Find("Smart Floor").GetComponent<UDPReceive>();
		settings = GameObject.Find("Smart Floor").GetComponent<Settings>();
		
		if(Screen.fullScreen)	
		{
			fullAddX = 100;
			fullAddY = 70;
		}
		
		AutoConnect();
		StartCoroutine(WaitCountdown(8.5f));
	}
	
	void AutoConnect()
	{
		send = GameObject.Find("Smart Floor").GetComponent<UDPSend>();
		//send.init();
		StartCoroutine(SendMessageStart());
	}
	
	void OnGUI()
	{
		if(countdown)
		{
			float win=Screen.width*0.6f;
			float w1=win*0.50f;
			
			GUIStyle styleCount = new GUIStyle();
			styleCount.alignment = TextAnchor.UpperLeft;
			styleCount.normal.textColor = Color.red;
			styleCount.fontSize = 200;
			styleCount.fontStyle = FontStyle.Bold;
			
			GUILayout.BeginArea(new Rect(335 + fullAddX, 175 + fullAddY, 500, 200));
			GUILayout.BeginHorizontal();
			GUILayout.Label(counter.ToString(), styleCount, GUILayout.Width(w1));
			GUILayout.EndHorizontal(); 
			GUILayout.EndArea();
		}
	}
	
	void OnTriggerEnter()
	{
		StartPosition();
		StartCoroutine(Respawn(1.5f));
	}
	
	public void StartPosition()
	{
		bServe = false;
		bAngle = false;
		//rigidbody.freezeRotation = true;
		//check who last scored and serve to opposite side
		rigidbody.isKinematic = true;
		rigidbody.position = new Vector3(0, 8.5f, 0);
	}
	
	public void Reset()
	{
		bServe = true;
		bAngle = false;
		countdown = false;
		rigidbody.isKinematic = false;
		//rigidbody.freezeRotation = true;
		rigidbody.position = new Vector3(0, 8.5f, 0);
		//check who last scored and serve to opposite side
		rigidbody.velocity = new Vector3(minVelocity,0,0);
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !won && !countdown) 
		{
	        // toggle pause state
	        paused = !paused;
	
	        if (paused) {
				bServe = false;
	            saveVelocity = rigidbody.velocity;
	            saveAngularVelocity = rigidbody.angularVelocity;
	            rigidbody.isKinematic = true;
	        } else {
	            // un-paused
	            rigidbody.isKinematic = false;
	            rigidbody.velocity = saveVelocity;
	            rigidbody.angularVelocity = saveAngularVelocity;
	            rigidbody.WakeUp();
				bServe = true;
	        }
    	}
		
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit();
		
		if (Input.GetKeyDown(KeyCode.F5)) 
			StartCoroutine(ZeroBoard());
		
		//check if someone is on the board before counting down
		if (checkPlayerOnBoard)
		{
			int allAboard = 0;
			for (int i=0; i < settings.noOfPlayers; i++)
			{
				if(recieve.resultData[i].force_N > 100)
					allAboard++;
			}
			
			if(allAboard == settings.noOfPlayers)
			{
				StartCoroutine(Countdown());
				checkPlayerOnBoard = false;
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {	
		if (rigidbody.velocity.x > particleStart || rigidbody.velocity.y > particleStart 
		    || rigidbody.velocity.x < -particleStart || rigidbody.velocity.y < -particleStart)
		{
			particleEmitter.enabled = true;
		}
		else
		{
			particleEmitter.enabled = false;
			particleEmitter.ClearParticles();
		}
	
		if (bServe)
		{
			rigidbody.velocity *= velocityIncrement;
			
			if(bAngle)
			{
				if (rigidbody.velocity.x < 0 && rigidbody.velocity.x > -6)
				{
					rigidbody.velocity = new Vector3(rigidbody.velocity.x - angleIncrement, rigidbody.velocity.y, 0);
				}
				else if (rigidbody.velocity.x > 0 && rigidbody.velocity.x < 6)
				{
					rigidbody.velocity = new Vector3(rigidbody.velocity.x + angleIncrement, rigidbody.velocity.y, 0);
				}
				
				if (rigidbody.velocity.y == 0 && lastXVel < 0)
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y + angleIncrement, 0);
				
				if (rigidbody.velocity.y == 0 && lastXVel > 0)
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y - angleIncrement, 0);
				
				if (rigidbody.velocity.y < 0 && rigidbody.velocity.y > -6)
				{
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y - angleIncrement, 0);
				}
				else if (rigidbody.velocity.y > 0 && rigidbody.velocity.y < 6)
				{
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y + angleIncrement, 0);
				}
				
				//Make sure we stay between the MAX and MIN speed.
		        float totalVelocity = Vector3.Magnitude(rigidbody.velocity);
		        if(totalVelocity>maxVelocity)
				{
		            float tooHard = totalVelocity / maxVelocity;
		            rigidbody.velocity /= tooHard;
		        } else if (totalVelocity < minVelocity) {
		            float tooSlowRate = totalVelocity / minVelocity;
		            rigidbody.velocity /= tooSlowRate;
		        }
				
				lastXVel = rigidbody.velocity.x;
			}
		}
	}
	
	IEnumerator Respawn(float wait)
	{
		yield return new WaitForSeconds(wait);
		if(!won)
		{
			Reset();
			yield return new WaitForSeconds(1.2f);
			bAngle = true;
		}
		else
		{
			StartCoroutine(WaitCountdown(8.5f));
		}
	}
	
	IEnumerator ZeroBoard()
	{
		yield return new WaitForSeconds(0.5f);
		send.sendMessage("StopSmart");
		yield return new WaitForSeconds(0.5f);
		send.sendMessage("Zero");
		yield return new WaitForSeconds(0.5f);
		send.sendMessage("StartSmart");
	}
	
	IEnumerator SendMessageStart()
	{
		yield return new WaitForSeconds(0.5f);
		send.sendMessage("Connect");
		yield return new WaitForSeconds(0.5f);
		send.sendMessage("Zero");
		yield return new WaitForSeconds(0.5f);
		send.sendMessage("StartSmart");
	}
	
	IEnumerator WaitCountdown(float wait)
	{
		yield return new WaitForSeconds(wait);
		won = false;
		checkPlayerOnBoard = true;
	}
	
	IEnumerator Countdown()
	{
		countdown = true;
		audio.Play();
		yield return new WaitForSeconds(1);
		counter--;
		yield return new WaitForSeconds(1);
		counter--;
		yield return new WaitForSeconds(1);
		counter--;
		yield return new WaitForSeconds(1);
		counter--;
		yield return new WaitForSeconds(1);
		counter--;
		Reset();
		yield return new WaitForSeconds(1.2f);
		bAngle = true;
		counter = 5;
	}
}