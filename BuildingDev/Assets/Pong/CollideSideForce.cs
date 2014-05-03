using UnityEngine;
using System.Collections;

public class CollideSideForce : MonoBehaviour {
	
	public float xforce = 0;
	private const float yforce = 0;
	private const float zforce = 0;
	private int counter = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter (Collision collision)
	{
		//set flag to say it had a hit and set the other paddles flag to false
		//so need to first know what object this is and make a condition for whichever it is
		//topforce scipt will need to check for last
		if(counter <= 5)
		{
			StartCoroutine(WaitCount());
		
		    if (collision.gameObject.tag == "pong")
		    {
				collision.gameObject.rigidbody.AddForce (xforce, yforce ,zforce);	
		    }
		}
	}
	
	IEnumerator WaitCount()
	{
		yield return new WaitForSeconds(2f);
		counter++;
	}
}
