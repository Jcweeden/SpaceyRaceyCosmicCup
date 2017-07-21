using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applied to box colliders on boost pads. Gets the car that passes through and initiates their boost.
/// </summary>
public class Boost : MonoBehaviour {

	void OnTriggerEnter (Collider other) {	//when pass through Boost box collider
		if (other.tag == "Player") {		//if the vehicle is the player
			PlayerMovement pmScript = other.GetComponent<PlayerMovement>();	 
			pmScript.SetBoost(true);//set the player boost to true
		}
		else {
			AIRacer2 AIScript = other.GetComponent<AIRacer2>(); //if the vehicle is AI
			AIScript.SetBoost(true);//set the AI boost to true
		}
	}
}
