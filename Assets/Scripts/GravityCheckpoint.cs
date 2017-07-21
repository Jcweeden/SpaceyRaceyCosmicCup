using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCheckpoint : MonoBehaviour {

	/// <summary>
	/// When the player passes through a trigger near the end of the track their gravity is altered to allow them to make the final jump.
	/// This script triggers the method that says the player has passed through the checkpoint that allows their gravity value to be changed
	/// upon passing through the next checkpoint. This checkpoint is a necessary measure before the second checkpoint that actually alters the
	/// gravity levels of the player. This prevent the player simply reversing through the checkpoint and altering their gravity again to the 
	/// incorrect level to continue racing -  they cannot do this with the use of two checkpoints as there is a jump inbetween that cannot be 
	/// passed back through.
	/// </summary>
	void OnTriggerEnter (Collider other) {	//when pass through Startline
		if (other.name == "Player") {
			PlayerMovement pmScript = other.GetComponent<PlayerMovement> ();
			pmScript.setGravityCheckpoint ();
		}
	}
}
