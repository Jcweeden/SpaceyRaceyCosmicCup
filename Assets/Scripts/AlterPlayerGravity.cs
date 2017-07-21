using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterPlayerGravity : MonoBehaviour {

	/// <summary>
	/// When the player passes through a trigger near the end of the track their gravity is altered to allow them to make the final jump.
	/// They must have passed through the gravity checkpoint beforehand, preventing them from reversing through this checkpoint and making
	/// them unable to make the next jump. This script triggers the method that alters their gravity value.
	/// </summary>
	void OnTriggerEnter (Collider other) {
		if (other.name == "Player") {	//if the vehicle passing through is the player
			PlayerMovement pmScript = other.GetComponent<PlayerMovement> ();	//run their setGravity script
			pmScript.setGravity ();
		}
	}
}
