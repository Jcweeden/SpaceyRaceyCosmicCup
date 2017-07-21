using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Respawns a vehicle when they fall off the course, and also the player if they press the respawn button.
/// </summary>
public class Respawn : MonoBehaviour {

	int checkpointReached;

	void OnTriggerEnter (Collider other) {	//when a car falls off the track and collides with the respawn collider trigger box

		if (other.name == "Player") { //if the car colliding with the respawn collider is the player
			checkpointReached = GameObject.Find ("Player").GetComponent<PlayerMovement> ().getcurrentCheckpointNo ();	//get the player's current checkpoint number

			if (checkpointReached >= 0 && checkpointReached <= 21){	//if between checkpoints 0-21 - respawn at checkpoint 7
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6200.19f, 6013.07f, 6001.1f);	//position the car at this checkpoint
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0,-90,0);	//and angle the car to face the correct direction

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6230.122f, 6128.074f, 6001.1f);	//also place the camera in the correct location
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.563f,-90f,0);			//and angle to face the correct direction
			}
			else if (checkpointReached >= 22 && checkpointReached <= 35){	//22-35 - 31
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (5840.4f, 6007f, 6001.1f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0,-90,0);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (5863.211f, 6113.68f, 5997.238f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.563f,-90f,0);
			}
			else if (checkpointReached >= 36 && checkpointReached <= 60){	//36-60 - 56
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (5771.3f, 5977.197f, 6073.427f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(11.558f,87.4f,-0.517f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (5802.307f, 6090.664f, 6077.837f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.561f,87.42101f,0);
			}
			else if (checkpointReached >= 60 && checkpointReached <= 100){	//60-100 - 72
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (5906.929f, 5966.611f, 6087.741f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0.014f,-0.736f,-0.076f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (5907.314f, 6081.611f, 6057.744f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.567f,-0.736f,0);
			}
			else if (checkpointReached >= 101 && checkpointReached <= 124){	//101-124 - 105
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (5917.322f, 5944.653f, 6412.344f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0.006f,86.50401f,-0.075f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (5887.377f, 6059.653f, 6420.514f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.567f,86.50401f,0);
			}
			else if (checkpointReached >= 125 && checkpointReached <= 143){	//125-143 - 134
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6233.5f, 5964.6f, 6435.9f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(14.361f,86.39301f,-0.896f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6202.032f, 6074.802f, 6432.693f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.567f,86.50401f,0);
			}
			else if (checkpointReached >= 144 && checkpointReached <= 169){	//144-169 - 149
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6401.6f, 5955.1f, 6440.6f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(14.361f,86.39301f,-0.896f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6372.494f, 6062.958f, 6439.019f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.567f,86.50401f,0);
			}
			else if (checkpointReached >= 170 && checkpointReached <= 183){	//170-183 - 169
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6622.1f, 5986.1f, 6434.8f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(14.361f,86.39301f,-0.896f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6592.494f, 6096.01f, 6432.878f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.567f,86.50401f,0);
			}
			else if (checkpointReached >= 184 && checkpointReached <= 209){	//184-209 - 190
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6854.748f, 5952.02f, 6442.38f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0.001f,-180.355f,-0.08f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6856.655f, 6067.02f, 6472.32f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(1.704f,0.306f,0.857f);
			}
			else if (checkpointReached >= 210 && checkpointReached <= 236){	//210-236 -	212 - set targetcheckpoint to 20
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6854.748f, 5952.02f, 6442.38f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0.001f,-180.355f,-0.08f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6847.1f, 6040.5f, 6254.9f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(1.704f,0.306f,0.857f);
			}
			else if (checkpointReached >= 237 && checkpointReached <= 281){	//237-281 - 241 - set targetcheckpoint to 23
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6841.9f, 5885.8f, 5863.6f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0.001f,-180.355f,-0.08f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6847.1f, 6040.5f, 6254.9f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(1.704f,0.306f,0.857f);
			}
			else if (checkpointReached >= 282 && checkpointReached <= 300){	//282-300 - 282 - set targetcheckpoint to 23
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6541.2f, 5963.6f, 5835.2f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(-8.768f,-17.802f,-2.802f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6847.1f, 6040.5f, 6254.9f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(1.704f,0.306f,0.857f);
			}
		
		}

		//ELSE, if the car is an AI do the same but do not move the camera.
		else if (other.name == "AI04" || other.name == "AI05" || other.name == "AI06") {

			GameObject Racer = GameObject.Find (other.name);
			checkpointReached = Racer.GetComponent<AIRacer2> ().getcurrentCheckpointNo ();

			if (checkpointReached >= 0 && checkpointReached <= 21){	//0-21 - 7
				Racer.transform.position = new Vector3 (6104.8f, 6014.45f, 6001.1f);
				Racer.transform.localEulerAngles = new Vector3(0,-90,0);
			}
			else if (checkpointReached >= 22 && checkpointReached <= 35){	//22-35 - 31
				Racer.transform.position = new Vector3 (5840.4f, 6007f, 6001.1f);
				Racer.transform.localEulerAngles = new Vector3(0,-90,0);
			}
			else if (checkpointReached >= 36 && checkpointReached <= 60){	//36-60 - 56
				Racer.transform.position = new Vector3 (5771.3f, 5977.197f, 6073.427f);
				Racer.transform.localEulerAngles = new Vector3(11.558f,87.4f,-0.517f);
			}
			else if (checkpointReached >= 60 && checkpointReached <= 100){	//60-100 - 72
				Racer.transform.position = new Vector3 (5906.929f, 5966.611f, 6087.741f);
				Racer.transform.localEulerAngles = new Vector3(0.014f,-0.736f,-0.076f);
			}
			else if (checkpointReached >= 101 && checkpointReached <= 124){	//101-124 - 105
				Racer.transform.position = new Vector3 (5917.322f, 5944.653f, 6412.344f);
				Racer.transform.localEulerAngles = new Vector3(0.006f,86.50401f,-0.075f);
			}
			else if (checkpointReached >= 125 && checkpointReached <= 143){	//125-143 - 134
				Racer.transform.position = new Vector3 (6233.5f, 5964.6f, 6435.9f);
				Racer.transform.localEulerAngles = new Vector3(14.361f,86.39301f,-0.896f);
			}
			else if (checkpointReached >= 144 && checkpointReached <= 169){	//144-169 - 149
				Racer.transform.position = new Vector3 (6401.6f, 5955.1f, 6440.6f);
				Racer.transform.localEulerAngles = new Vector3(14.361f,86.39301f,-0.896f);
			}
			else if (checkpointReached >= 170 && checkpointReached <= 183){	//170-183 - 169
				Racer.transform.position = new Vector3 (6622.1f, 5986.1f, 6434.8f);
				Racer.transform.localEulerAngles = new Vector3(14.361f,86.39301f,-0.896f);
			}
			else if (checkpointReached >= 184 && checkpointReached <= 209){	//184-209 - 190
				Racer.transform.position = new Vector3 (6854.748f, 5952.02f, 6442.38f);
				Racer.transform.localEulerAngles = new Vector3(0.001f,-180.355f,-0.08f);
			}
			else if (checkpointReached >= 210 && checkpointReached <= 236){	//210-236 -	212
				Racer.transform.position = new Vector3 (6854.748f, 5952.02f, 6442.38f);
				Racer.transform.localEulerAngles = new Vector3(0.001f,-180.355f,-0.08f);
				Racer.GetComponent<AIRacer2> ().setcurrentTargetNo (20);	//also reset the target checkpoint so the car drives in the correct direction
			}
			else if (checkpointReached >= 237 && checkpointReached <= 281){	//237-281 - 241
				Racer.transform.position = new Vector3 (6841.9f, 5885.8f, 5863.6f);
				Racer.transform.localEulerAngles = new Vector3(0.001f,-180.355f,-0.08f);
				Racer.GetComponent<AIRacer2> ().setcurrentTargetNo (24);
			}
			else if (checkpointReached >= 282 && checkpointReached <= 300){	//282-300 - 282
				Racer.transform.position = new Vector3 (6541.2f, 5963.6f, 5835.2f);
				Racer.transform.localEulerAngles = new Vector3(-8.768f,-17.802f,-2.802f);
				Racer.GetComponent<AIRacer2> ().setcurrentTargetNo (28);
			}

		}
	}


	/// <summary>
	/// Respawns the player when they press the space bar to respawn.
	/// </summary>
	public void RespawnPlayer () {	
			checkpointReached = GameObject.Find ("Player").GetComponent<PlayerMovement> ().getcurrentCheckpointNo ();

			if (checkpointReached >= 0 && checkpointReached <= 21){	//0-21 - 7
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6104.8f, 6014.45f, 6001.1f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0,-90,0);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6130.122f, 6128.074f, 6000.952f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.563f,-90f,0);
			}
			else if (checkpointReached >= 22 && checkpointReached <= 35){	//22-35 - 31
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (5840.4f, 6007f, 6001.1f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0,-90,0);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (5863.211f, 6113.68f, 5997.238f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.563f,-90f,0);
			}
			else if (checkpointReached >= 36 && checkpointReached <= 60){	//36-60 - 56
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (5771.3f, 5977.197f, 6073.427f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(11.558f,87.4f,-0.517f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (5802.307f, 6090.664f, 6077.837f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.561f,87.42101f,0);
			}
			else if (checkpointReached >= 60 && checkpointReached <= 100){	//60-100 - 72
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (5906.929f, 5966.611f, 6087.741f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0.014f,-0.736f,-0.076f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (5907.314f, 6081.611f, 6057.744f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.567f,-0.736f,0);
			}
			else if (checkpointReached >= 101 && checkpointReached <= 124){	//101-124 - 105
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (5917.322f, 5944.653f, 6412.344f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0.006f,86.50401f,-0.075f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (5887.377f, 6059.653f, 6420.514f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.567f,86.50401f,0);
			}
			else if (checkpointReached >= 125 && checkpointReached <= 143){	//125-143 - 134
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6233.5f, 5964.6f, 6435.9f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(14.361f,86.39301f,-0.896f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6202.032f, 6074.802f, 6432.693f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.567f,86.50401f,0);
			}
			else if (checkpointReached >= 144 && checkpointReached <= 169){	//144-169 - 149
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6401.6f, 5955.1f, 6440.6f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(14.361f,86.39301f,-0.896f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6372.494f, 6062.958f, 6439.019f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.567f,86.50401f,0);
			}
			else if (checkpointReached >= 170 && checkpointReached <= 183){	//170-183 - 169
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6622.1f, 5986.1f, 6434.8f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(14.361f,86.39301f,-0.896f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6592.494f, 6096.01f, 6432.878f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(26.567f,86.50401f,0);
			}
			else if (checkpointReached >= 184 && checkpointReached <= 209){	//184-209 - 190
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6854.748f, 5952.02f, 6442.38f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0.001f,-180.355f,-0.08f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6856.655f, 6067.02f, 6472.32f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(1.704f,0.306f,0.857f);
			}
			else if (checkpointReached >= 210 && checkpointReached <= 236){	//210-236 -	212 - set targetcheckpoint to 20
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6854.748f, 5952.02f, 6442.38f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0.001f,-180.355f,-0.08f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6847.1f, 6040.5f, 6254.9f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(1.704f,0.306f,0.857f);
			}
			else if (checkpointReached >= 237 && checkpointReached <= 281){	//237-281 - 241 - set targetcheckpoint to 23
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6841.9f, 5885.8f, 5863.6f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(0.001f,-180.355f,-0.08f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6847.1f, 6040.5f, 6254.9f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(1.704f,0.306f,0.857f);
			}
			else if (checkpointReached >= 282 && checkpointReached <= 300){	//282-300 - 282 - set targetcheckpoint to 23
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.position = new Vector3 (6541.2f, 5963.6f, 5835.2f);
				GameObject.Find ("Player").GetComponent<PlayerMovement> ().transform.localEulerAngles = new Vector3(-8.768f,-17.802f,-2.802f);

				GameObject.Find ("Main Camera").transform.position = new Vector3 (6847.1f, 6040.5f, 6254.9f);
				GameObject.Find ("Main Camera").transform.localEulerAngles = new Vector3(1.704f,0.306f,0.857f);
			}
}
}
