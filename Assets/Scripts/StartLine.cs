using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Collision detection to detect when the player crosses the start line - used to start the race.
/// </summary>
public class StartLine : MonoBehaviour {
	
	bool RaceStarted;	//starts the laptimer when the start line is crossed.

	void OnTriggerEnter (Collider other) {	//when the player passes through Startline
		if (other.name == "Player") {
			RaceStarted = true;	//if first time crossing line begin clock
		}
	}

	// Use this for initialization
	void Start () {
		RaceStarted = false;
	}

	public bool getRaceStarted(){
		return RaceStarted;
	}
}
