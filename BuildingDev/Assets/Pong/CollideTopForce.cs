using UnityEngine;
using System.Collections;

public class CollideTopForce : MonoBehaviour {
	
	public float xforce = 0;
	public float yforce = 0;
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
