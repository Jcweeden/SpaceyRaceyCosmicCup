using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// PlayerMovement contains the controls for the player, taking the keyboard inputs and applying velocity and rotations to the player's vehicle.
/// </summary>
public class PlayerMovement : MonoBehaviour {

	GameObject player; 						//the car itself - the gameObject the script is attached to
	Rigidbody playerRigidBody;				//the rigidbody of the car we add force to so we can move it


	float moveHorizontal;					//float between -1 and 1 to indicate key press of forwards and backwards movement buttons
	float moveVertical;						//float between -1 and 1 to indicate key press of rotate left and right movement buttons

	public float movingSpeed;				//how quick the car's max speed is
	public float rotatingSpeed;				//how quickly the car rotates when rotation keys are pressed
	public float gravity;					//the gravity value applied to the car

	public bool grounded;					//whether vehicle is falling, or on the ground - decided using a raycast from underneath the car
	private Vector3 posCur;					//the position of the vehicle should it be aligned on the surface of the road
	private Quaternion rotCur;				//the rotation of the vehicle should it be aligned on the surface of the road

	public bool boosting;					//bool as to whether the car is currently boosting - turns on the boost particle effects
	public int boostVal;					//set to a value when boost is enabled and then decrements. Shows how much time is left on the car's boost

	//Player ParticleSystems & light on the front of the vehicle
	ParticleSystem jetMiddle;
	ParticleSystem jetRight;
	ParticleSystem jetLeft;
	Light backLightAura;

	public bool gravityCheckpoint;				//for certain jumps, a change in gravity is required for the player so they can make the jump - when true the player has more gravity assigned

	public int currentCheckpoint;				//the last checkpoint that the car passed
	public int lap;								//the current lap the racer is on
	public int raceNumOfLaps; 					//number of laps in the race

	public bool movementDisabled;				//turned on when the race begins, and disabled once past the finish line


	void Awake() {

		Application.targetFrameRate = 60;		//sets the game to run at 60fps
	}

	// Use this for initialization
	void Start () {

		movementDisabled = true;						//disable movement until countdown timer finishes
		player = GameObject.Find ("Player");			//gets the gameobject the script is attached to as the Player (could do player = gameObject;)
		playerRigidBody = player.GetComponent<Rigidbody> (); //get the rigidbody of the car so we can add force to move it

		//save the particle systems on the car for future reference
		backLightAura = GameObject.Find ("PlayerBackLightAura").GetComponent<Light> ();
		jetLeft = GameObject.Find ("PlayerJetLeft").GetComponent<ParticleSystem> ();
		jetMiddle = GameObject.Find ("PlayerJetMiddle").GetComponent<ParticleSystem> ();
		jetRight = GameObject.Find ("PlayerJetRight").GetComponent<ParticleSystem> ();

		currentCheckpoint = 0;	//set the last checkpoint the car has passed to 0
		boosting = false;		//set the car to not be boosting upon startup

		GameObject.Find ("GUICanvas").GetComponent<GUITimer> ().UpdateLapsElapsedText (lap, raceNumOfLaps);
	}

	void Update() {


		Ray ray = new Ray(transform.position, -transform.up); 	//declare a new Ray. Direction is straight down from the car
		RaycastHit hit;											//raycastHit that will contain information from casting the ray below the car

		//The maximum distance the ray will go is 4.3f - keeping the vehicle hovering at a small distance above the ground - above this gravity will take effect
		if(Physics.Raycast(ray, out hit, 4.3f) == true) {						//if the vehicle raycast reaches the floor
			Debug.DrawLine(transform.position, hit.point, Color.green);			//draw a Debug line of the raycast so we can see the ray in the scene view

			//store the roation and position as they would be aligned on the surface
			rotCur = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
			posCur = new Vector3 (transform.position.x, hit.point.y + 2.0f, transform.position.z);
			grounded = true;	//and declare the vehicle is touching the ground (Actually floating above it)

		}			
		else {			//if raycast didn't hit anything, the vehicle is in the air and not grounded
			grounded = false;
		}
			
		if(grounded == true) {		//if the car is floating 'on' the ground
			//smoothly rotate and move the car until it's aligned to the road. The '5' multiplier controls how fast the changes the car aligns
			transform.position = Vector3.Lerp(transform.position, posCur, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotCur, Time.deltaTime * 5);
		}
		else /*if (grounded == false)*/{	//if the car is falling, slowly tilt its front end downwards to simulate falling
			transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up * gravity, Time.deltaTime * 15/*5*/);
		}


		//DEBUGGING - resets race
		/*if (Input.GetKeyDown (KeyCode.Return)) { 

			SceneManager.LoadScene ("1"); 
		} */
	}


	// Update is called once per frame
	void FixedUpdate () {


		if (movementDisabled == false) {						//if movement is enabled then
			moveHorizontal = Input.GetAxis ("Horizontal");		//get float between -1 and 1 to indicate key press of forwards and backwards movement buttons
			moveVertical = Input.GetAxis ("Vertical");			//float between -1 and 1 to indicate key press of rotate left and right movement buttons


			Vector3 movement = transform.rotation * Vector3.forward;	//calculate movement direction by rotation


			if (boosting == false) {	//if no boost enabled - move normally
				StandardSpeedParticleEmissions ();	//emit normal particles

				if (grounded == true) {				//and is on the ground
					//then accelerate the car according to moveVertical, which is indicated by button press
					playerRigidBody.AddForce (moveVertical * ((movingSpeed + boostVal) * Time.deltaTime) * movement, ForceMode.Impulse);	
				} else /*if (grounded == false)*/{ //else the car is flying, make the car move slightly slower when in the air
					playerRigidBody.AddForce ((moveVertical) * ((((movingSpeed / 100) * 70) + boostVal) * Time.deltaTime) * movement, ForceMode.Impulse);	//propulsion
				}


			} else {	//boost is enabled

				BoostSpeedParticleEmissions ();	//emit coloured particle emissions

				DecrementBoost ();	//lower the boost value

				//add the additional force of the boost on to the movement speed
				playerRigidBody.AddForce (1 * ((movingSpeed + boostVal) * Time.deltaTime) * movement, ForceMode.Impulse);	//propulsion
			}

		 
			transform.Rotate (Vector3.up, moveHorizontal * Time.deltaTime * rotatingSpeed);	//steering
			//Debug.Log(movingSpeed+boostVal);


			//rotate Z axis when steering to roll the car

			if (grounded == true) {
				//then rotate the car on its y axis (roll the car)according to moveHorizontal, which is indicated by button press
				transform.Rotate (0, (moveHorizontal * Time.deltaTime), 0);

				//if the car is already rotated on its y axis then slowly rotate it back to a normal position
				if (moveHorizontal > 0f) {
					transform.Rotate (0, 0, -2.4f);
				}

				if (moveHorizontal < 0f && transform.localEulerAngles.z < 10) {
					transform.Rotate (0, 0, 2.4f);
				}
			}

			//press space respawns the car
			if (Input.GetKey (KeyCode.Space)) {		
				GameObject.Find("RespawnCollider").GetComponent<Respawn>().RespawnPlayer();
			}

			//press r to restart the race
			if (Input.GetKey (KeyCode.R)) {		
				SceneManager.LoadScene ("a"); 

			}

			/*if (Input.GetKey (KeyCode.E)) {		//SET TO REROTATE CAR
				//transform.Translate (0, 10f, 0);
				playerRigidBody.AddExplosionForce(10f, transform.position, 0f, 3f);
			}*/

			//if the car is falling, slowly tilt its front end downwards to simulate falling
			if (grounded == false && transform.rotation.x < 0.65) {	//tilt downwards when falling
				//Debug.Log (transform.rotation.x);
				transform.Rotate (0.4f, 0, 0);

			}
		}
	}

	/// <summary>
	/// Sets the Player boost speed particle emissions to have a yellow effect on the central emitter.
	/// </summary>
	public void BoostSpeedParticleEmissions() {
		var col = jetMiddle.colorOverLifetime;
		col.enabled = true;

		Gradient grad = new Gradient();
		grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.yellow, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 0.1f), new GradientAlphaKey(0.0f, 1.0f) } );

		col.color = grad;
	}

	/// <summary>
	/// Emits Player boost speed particle emissions with their standard colours, and according to the speed the player is moving
	/// </summary>
	public void StandardSpeedParticleEmissions() {	//adjust particle effects according to speed

		//make light aura glow more when moving faster
		if (moveVertical >= 0.18) {
			backLightAura.intensity = (moveVertical * 100f) * 0.0279f;
		} else {
			backLightAura.intensity = 0.5f;
		}


		var forceOverLifeTime = jetLeft.forceOverLifetime;

		//change force of emissions particles
		if (moveVertical > 0) {
			var forceMiddle = jetMiddle.forceOverLifetime;		//middle jet
			forceMiddle.zMultiplier = (moveVertical * 100f) * 1f;	

			var forceLeft = jetLeft.forceOverLifetime;		//left jet
			forceLeft.zMultiplier = (moveVertical * 100f) * 0.4f;	

			var forceRight = jetRight.forceOverLifetime;		//right jet
			forceRight.zMultiplier = (moveVertical * 100f) * 0.4f;	
		}
		else {
			var forceMiddle = jetMiddle.forceOverLifetime;
			forceMiddle.zMultiplier = 0f;
			var forceRight = jetRight.forceOverLifetime;
			forceRight.zMultiplier = 0f;
			var forceLeft = jetLeft.forceOverLifetime;
			forceLeft.zMultiplier = 0f;
		}
	}

	/// <summary>
	/// Sets the boost value to true. Run from the Boost.cs script when hit a boost collider is triggered
	/// </summary>
	public void SetBoost(bool isBoosting) {
		
		//if not currently boosting, and boost has been triggered
		if (boosting != true && isBoosting == true) {
			boostVal = 825;	//+1000 speed

			BoostSpeedParticleEmissions();	//SET particles to boost speed Colours
		}
		boosting = isBoosting;	//update the boosting value of the car to reflect whether boosting or not
	}

	/// <summary>
	/// Decrements the boost value.
	/// </summary>
	public void DecrementBoost() {
		if (boostVal > 0) {			//if still boosting decrement it
			boostVal = boostVal - 20;
		} 

		else {	//boost is over - reset back to normal values and disable boost
			boosting = false;

			//reset PARTICLE COLOURS back to normal
			var col = jetMiddle.colorOverLifetime;
			//col.enabled = true;
			Gradient grad = new Gradient();
			grad.SetKeys( new GradientColorKey[] { new GradientColorKey(new Color32(40,0,225,100), 0.0f), new GradientColorKey(new Color32(40,0,225,100), 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 0.1f), new GradientAlphaKey(0.0f, 1.0f) } );
			col.color = grad;
		}
	}

	/// <summary>
	/// Changes the gravity value applied to the player when not grounded. For certain jumps a change in gravity is required for the player so they
	/// can make the jump. If the player has passed a gravity checkpoint then this method will increase gravity to allow the jump to be made. 
	/// Otherwise it will return it to normal. Run from AlterPlayerGravity.cs
	/// </summary>
	public void setGravity() {
		if (gravity == 4.4f) {
			gravity = 2.5f;
		} else if (gravityCheckpoint){
			gravity = 4.4f;
		}
	}

	/// <summary>
	/// Run when GravityCheckpoint.cs is triggered. Alternates the state of the gravity checkpoint variable between true and false.
	/// </summary>
	public void setGravityCheckpoint() {
		if (gravityCheckpoint == true) {
			gravityCheckpoint = false;
		} else {
			gravityCheckpoint = true;
		}
	}

	/// <summary>
	/// Set the last checkpoint number the Player has passed.
	/// </summary>
	public void setcurrentCheckpointNo(int checkpointNo) {

		currentCheckpoint = checkpointNo;
	}

	/// <summary>
	/// Returns the last checkpoint number the Player has passed.
	/// </summary>
	public int getcurrentCheckpointNo() {

		return currentCheckpoint;
	}

	public void incrementLap() {
		if (lap < raceNumOfLaps) {	//if not on the final lap
			lap++;	//increment lap
			GameObject.Find ("GUICanvas").GetComponent<GUITimer> ().UpdateLapsElapsedText (lap, raceNumOfLaps);	//update the GUI to display lap counter
		} else {	//else player has completed their final lap
			setMovementDisabled (true);	//disabled movement as player has finished

			//call GUI to display position and lap times
			GameObject.Find ("GUICanvas").GetComponent<GUITimer> ().completedRace ();
			GameObject.Find ("GUICanvas").GetComponent<GUITimer> ().racerFinished (gameObject.name);
		}
	}

	/// <summary>
	/// Sets bool indicating whether the car can move.
	/// </summary>
	public void setMovementDisabled(bool isDisabled) {
		movementDisabled = isDisabled;
	}

	/// <summary>
	/// Returns the number of laps in the race.
	/// </summary>
	public int getRaceNumOfLaps(){
		return raceNumOfLaps;
	}
}
