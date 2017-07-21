using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Checkpoint trigger. This is attached to every checkpoint node in the game.
/// </summary>
public class CheckpointTrigger : MonoBehaviour {

	public int checkpointNumber;
	private PositionTracker positionTracker;

	/// <summary>
	/// Iterate the vehicle's checkpoint number when it passes through the checkpoint
	/// </summary>
	void OnTriggerEnter (Collider other) {					//when a vehicle passes through the checkpoint
		GameObject Racer = GameObject.Find(other.name);		//get the agent who entered the checkpoint's name

		//iterate the vehicle's checkpoint number
		if (other.name == "Player") {														//if the vehicle is the player
			PlayerMovement PlayerScript = Racer.GetComponent<PlayerMovement> ();
			if (checkpointNumber == 0 && PlayerScript.getcurrentCheckpointNo() == 300) {	//if completed a lap
				PlayerScript.incrementLap ();												//increment the lap
				positionTracker.updateRacerLap (other.name);								//and update it in the position tracker
				GameObject.Find("GUICanvas").GetComponent<GUITimer>().ResetTime();			//and reset the laptime
			}

			PlayerScript.setcurrentCheckpointNo (checkpointNumber);							//then update the checkpoint number
			UpdateCheckpointPosition(Racer, checkpointNumber);								//update the position tracker with the new checkpoint
		}

			else {	//else it is not player but a racer
			AIRacer2 RacerScript = Racer.GetComponent<AIRacer2> ();

				if (checkpointNumber == 0 && RacerScript.getcurrentCheckpointNo() == 300) {
					RacerScript.incrementLap ();
					positionTracker.updateRacerLap(other.name);
				}
			RacerScript.setcurrentCheckpointNo (checkpointNumber);
			UpdateCheckpointPosition(Racer, checkpointNumber);
		}
	}
		
	/// <summary>
	/// Updates the position tracker with the new checkpoint.
	/// </summary>
	public void UpdateCheckpointPosition(GameObject Racer, int checkpointNumber) {
		GameObject.Find ("GUICanvas").GetComponent<PositionTracker> ().updateRacerCheckpoint(Racer.name, checkpointNumber);
	}
		
	/// <summary>
	/// Gets the instance of the position tracker.
	/// </summary>
	void Start () {
		positionTracker = GameObject.Find ("GUICanvas").GetComponent<PositionTracker> ();
	}
}
