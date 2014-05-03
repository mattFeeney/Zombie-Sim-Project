using UnityEngine;
using System.Collections;

public class RespawnBall : MonoBehaviour {
	
	//private float tChange = 0; // force new direction in the first Update
	private float randomX;
	private float randomY;
    private const float moveSpeed = 3;
	
	void OnTriggerEnter (Collider other) {
		other.transform.position = new Vector3(0, 0, 0);
	//	other.transform.Translate(new Vector3(randomX,randomY,0) * moveSpeed * Time.deltaTime);
		//other.transform.TransformDirection(Random.value , Random.value ,0);
	}
	
	/*void Update () {
	    // change to random direction at random intervals
	    if (Time.time >= tChange){
	        randomX = Random.Range(-2.0f,2.0f); // with float parameters, a random float
	        randomY = Random.Range(-2.0f,2.0f); //  between -2.0 and 2.0 is returned
	        // set a random interval between 0.5 and 1.5
	        tChange = Time.time + Random.Range(0.5f,1.5f);
	    }
	}*/
	
	/*void OnTriggerExit (Collider other) {
		other.transform.position = new Vector3(0, 0, 0);
		other.transform.TransformDirection(Random.value , Random.value ,0);
	}*/
}



