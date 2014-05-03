using UnityEngine;
using System.Collections;

public class CollideSound : MonoBehaviour {
	
	public AudioClip audiosound;
	
	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter (Collision collision)
	{
		audio.PlayOneShot(audiosound);
	}

}
