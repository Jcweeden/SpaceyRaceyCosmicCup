using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The class that controls the AI, movement, pathfinding, particle effects, of all AI racers.
/// </summary>
public class AIRacer2 : MonoBehaviour {

	int currentTargetNo;					//the checkpoint the car is moving towards

	public List<GameObject> checkpoints;	//the 30 checkpoints the AI will head towards to navigate around the track

	GameObject AIRacerCar;					//the car itself - the gameObject the script is attached to
	Rigidbody AIRigidbody;					//the rigidbody of the car we add force to so we can move it

	public int lap;							//the current lap the racer is on	
	public string carIdentifier;			//a unique identifer for each car - e.g. 'AI01', or 'AI02'

	public float gravity;					//the gravity value applied to the car
	public int movementSpeed;				//how quick the car's max speed is
	public float rotationSpeed;				//how quickly the car rotates to face its next checkpoint
	public int distanceToChangeCheckPoint;	//how far away the next checkpoint is - used for position system


	public bool grounded;					//whether vehicle is falling, or on the ground - decided using a raycast from underneath the car
	private Vector3 posCur;					//the position of the vehicle should it be aligned on the surface of the road
	private Quaternion rotCur;				//the rotation of the vehicle should it be aligned on the surface of the road

	public bool boosting;					//bool as to whether the car is currently boosting - turns on the boost particle effects
	public int boostVal;					//set to a value when boost is enabled and then decrements. Shows how much time is left on the car's boost

	private float acceleration;				//the acceleration of the car that increases as the car gets faster. Capped at 1.

	//AI04 / AI06 ParticleSystems
	ParticleSystem jetMiddle;
	ParticleSystem jetRight;
	ParticleSystem jetLeft;
	Light backLightAura;

	//AI05 ParticleSystems
	ParticleSystem AI05JetRight1;
	ParticleSystem AI05JetRight2;
	ParticleSystem AI05JetRight3;
	ParticleSystem AI05JetLeft1;
	ParticleSystem AI05JetLeft2;
	ParticleSystem AI05JetLeft3;

	GameObject currentTarget;		//node that AI is travelling towards

	public int currentCheckpoint;	//the last checkpoint that the car passed

	public bool movementDisabled;	//turned on when the race begins, and disabled once past the finish line


	// Use this for initialization
	void Start () {
		movementDisabled = true;	//disable movement until the race starts

		currentTargetNo = 0;		//the current checkpoint the car is moving towards

		currentCheckpoint = 0;		//set the last checkpoint the car has passed to 0
		lap = 1;					//the current lap the car is on

		AIRacerCar = gameObject; 								//declares the gameobject the script is attached to as the AIRacerCar
		AIRigidbody = AIRacerCar.GetComponent<Rigidbody> ();	//get the rigidbody of the car so we can add force to move it


		//save up the particle systems on the car according to which car it is for future reference
		if (carIdentifier == "AI04") {
			backLightAura = GameObject.Find ("AI04BackLightAura").GetComponent<Light> ();
			jetLeft = GameObject.Find ("AI04JetLeft").GetComponent<ParticleSystem> ();
			jetMiddle = GameObject.Find ("AI04JetMiddle").GetComponent<ParticleSystem> ();
			jetRight = GameObject.Find ("AI04JetRight").GetComponent<ParticleSystem> ();
		}

		if (carIdentifier == "AI05") {
			backLightAura = GameObject.Find ("AI05BackLightAura").GetComponent<Light> ();

			AI05JetRight1 = GameObject.Find ("AI05JetRight1").GetComponent<ParticleSystem> ();
			AI05JetRight2 = GameObject.Find ("AI05JetRight2").GetComponent<ParticleSystem> ();
			AI05JetRight3 = GameObject.Find ("AI05JetRight3").GetComponent<ParticleSystem> ();

			AI05JetLeft1 = GameObject.Find ("AI05JetLeft1").GetComponent<ParticleSystem> ();
			AI05JetLeft2 = GameObject.Find ("AI05JetLeft2").GetComponent<ParticleSystem> ();
			AI05JetLeft3 = GameObject.Find ("AI05JetLeft3").GetComponent<ParticleSystem> ();
		}

		if (carIdentifier == "AI06") {
			backLightAura = GameObject.Find ("AI06BackLightAura").GetComponent<Light> ();
			jetLeft = GameObject.Find ("AI06JetLeft").GetComponent<ParticleSystem> ();
			jetRight = GameObject.Find ("AI06JetRight").GetComponent<ParticleSystem> ();
		}
	}

	void Update() {
		Ray ray = new Ray(transform.position, -transform.up);		//declare a new Ray. Direction is straight down from the car

		RaycastHit hit; //raycastHit that will contain information from casting the ray below the car

		//cast the ray. 'out hit' fills the hit variable with information
		//The maximum distance the ray will go is 5.3f - keeping the vehicle hovering at a small distance above the ground - above this gravity will take effect
		if (Physics.Raycast (ray, out hit, 5.3f) == true) {	//if the vehicle raycast reaches the floor
			//draw a Debug line of the raycast so we can see the ray in the scene view
			Debug.DrawLine(transform.position, hit.point, Color.green);
			//store the roation and position as they would be aligned on the surface
			rotCur = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
			posCur = new Vector3(transform.position.x, hit.point.y + 3f, transform.position.z);
			grounded = true;	//and declare the vehicle is touching the ground (Actually floating above it)

		}
		else { 		//if raycast didn't hit anything, the vehicle ise in the air and not grounded
			grounded = false;
		}


		if(grounded == true) {	//if the car is floating 'on' the ground
			//smoothly rotate and move the car until it's aligned to the road. The '5' multiplier controls how fast the changes the car aligns
			transform.position = Vector3.Lerp(transform.position, posCur, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotCur, Time.deltaTime * 5);
		}

		else /*if (grounded == false)*/{	//if the car is in the air, apply downward force to simulate gravity and pull it back on to the track
			transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up * gravity, Time.deltaTime * 15);
		}

	}

	// Update is called once per frame
	void FixedUpdate () {

		//calculate the distance to the next checkpoint
		float distance = Vector3.Distance (checkpoints[currentTargetNo].transform.transform.position, this.transform.position);

		//if within a certain distance to the next checkpoint, then move up one checkpoint - so the car will continue on its path around the track
		if (distance < distanceToChangeCheckPoint) {
			if (currentTargetNo < checkpoints.Count-1) {	//if the checkpoint is before the end of the lap
				
				currentTargetNo++;	//iterate by 1
			}
			else {					//otherwise reset the target back to the first checkpoint so the car begins another lap
				currentTargetNo = 0;
			}
		}


		grounded = true;	//reset grounded to true so it can be set to false again.

		if (movementDisabled == false) {	//if movement is enabled then
			moveTowardsTarget ();			//move the car towards the target
		}
	}



	/// <summary>
	/// Moves the car towards its next target checkpoint. Rotates the vehicle and applies force, more is boost is enabled
	/// </summary>
	void moveTowardsTarget() {

		if (acceleration < 1) {	//if the current acceleration is below 1 then slowly increase it
			acceleration = acceleration + 0.05f;
		}
		//TO IMPLEMENT //bank the car, rotating it as it turns


		//rotate the car to face the next target
		var lookPos = checkpoints[currentTargetNo].transform.position - transform.position;
		lookPos.y = 0;	//keep the y axis 0 so the car does not roll over
		var rotation = Quaternion.LookRotation(lookPos);

		//lerp from the current rotation of the car to the rotation that faces the next checkpoint
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

		//get direction facing 
		Vector3 movement = transform.rotation * Vector3.forward;	//calculate movement direction by rotation


		//emit particles according to which car it is
		if (carIdentifier == "AI04") {
			AI04StandardSpeedParticleEmissions ();
		}
		else if (carIdentifier == "AI05") {
			AI05StandardSpeedParticleEmissions ();
		}
		else if (carIdentifier == "AI06") {
			AI06StandardSpeedParticleEmissions ();
		}



		if (boosting == false) {	//if not boosting
			
			if (grounded == true) {	//and is on the ground
				AIRigidbody.AddForce (acceleration * (movementSpeed * Time.deltaTime) * movement, ForceMode.Impulse);	//then accelerate the car
			} 

			else /*if (grounded == false)*/{ //else the car is flying, make the car move slightly slower when in the air
				AIRigidbody.AddForce (acceleration * ((movementSpeed/2) * Time.deltaTime) * movement, ForceMode.Impulse);
			}
		}

	else {	//boost is enabled
			DecrementBoost ();	//lower the boost value

			//add the additional force of the boost on to the movement speed
			AIRigidbody.AddForce (1 * ((movementSpeed+boostVal) * Time.deltaTime) * movement, ForceMode.Impulse);	//propulsion
		}
	}

	/// <summary>
	/// Sets the boost value to true. Run from the Boost.cs script when hit a boost collider is triggered
	/// </summary>
	public void SetBoost(bool isBoosting) {

		//if not currently boosting, and boost has been triggered
		if (boosting != true && isBoosting == true) {
			boostVal = 1000;	//+1000 speed when boosting
			if ( carIdentifier == "AI04") {	//trigger the boost particle effects dependent on which car it is.
				AI04BoostSpeedParticleEmissions();	//SET particles to boost speed Colours
			}
			if ( carIdentifier == "AI06") {
				AI06BoostSpeedParticleEmissions();	//SET particles to boost speed Colours
			}
			if ( carIdentifier == "AI05") {
				AI05BoostSpeedParticleEmissions();	//SET particles to boost speed Colours
			}
		}
		boosting = isBoosting;	//update the boosting value of the car  to reflect whether boosting or not
	}

	/// <summary>
	/// Sets the AI04 boost speed particle emissions to have a yellow effect on the central emitter.
	/// </summary>
	public void AI04BoostSpeedParticleEmissions() {
		var col = jetMiddle.colorOverLifetime;
		col.enabled = true;

		Gradient grad = new Gradient();
		grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.yellow, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 0.1f), new GradientAlphaKey(0.0f, 1.0f) } );

		col.color = grad;
	}

	/// <summary>
	/// Sets the AI05 boost speed particle emissions to have a yellow effect on all emitters.
	/// </summary>
	public void AI05BoostSpeedParticleEmissions() {
		var jetR1 = AI05JetRight1.colorOverLifetime;
		var jetR2 = AI05JetRight2.colorOverLifetime;
		var jetR3 = AI05JetRight3.colorOverLifetime;
		var jetL1 = AI05JetLeft1.colorOverLifetime;
		var jetL2 = AI05JetLeft2.colorOverLifetime;
		var jetL3 = AI05JetLeft3.colorOverLifetime;



		Gradient grad = new Gradient();
		grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 0.1f), new GradientAlphaKey(0.0f, 1.0f) } );

		jetR1.color = grad;
		jetR2.color = grad;
		jetR3.color = grad;
		jetL1.color = grad;
		jetL2.color = grad;
		jetL3.color = grad;

	}

	/// <summary>
	/// Sets the AI06 boost speed particle emissions to have a yellow effect on the left/right emitters.
	/// </summary>
	public void AI06BoostSpeedParticleEmissions() {
		var col = jetLeft.colorOverLifetime;
		col.enabled = true;

		Gradient grad = new Gradient();
		grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.yellow, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 0.1f), new GradientAlphaKey(0.0f, 1.0f) } );

		col.color = grad;

		var colR = jetRight.colorOverLifetime;
		colR.enabled = true;

		Gradient gradR = new Gradient();
		gradR.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.yellow, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 0.1f), new GradientAlphaKey(0.0f, 1.0f) } );

		colR.color = gradR;
	}


	/// <summary>
	/// Decrements the boost value.
	/// </summary>
	public void DecrementBoost() {
		
		if (boostVal > 0) {		//if still boosting decrement it
			boostVal = boostVal - 20;
		} 

		else {	//boost is over - reset back to normal values and disable boost
			boosting = false;

			//reset PARTICLE COLOURS for each car
			if (carIdentifier == "AI04") {
				
				var col = jetMiddle.colorOverLifetime;
				Gradient grad = new Gradient ();
				grad.SetKeys (new GradientColorKey[] {
					new GradientColorKey (new Color32 (255, 0, 79, 255), 0.0f),
					new GradientColorKey (new Color32 (255, 0, 79, 255), 1.0f)
				}, new GradientAlphaKey[] {
					new GradientAlphaKey (0.0f, 0.0f),
					new GradientAlphaKey (1.0f, 0.1f),
					new GradientAlphaKey (0.0f, 1.0f)
				});
				col.color = grad;
			}
			else if (carIdentifier == "AI06") {

				var col = jetLeft.colorOverLifetime;
				var colR = jetRight.colorOverLifetime;

				Gradient grad = new Gradient ();
				grad.SetKeys (new GradientColorKey[] {
					new GradientColorKey (new Color32 (255, 174, 0, 255), 0.0f),
					new GradientColorKey (new Color32 (255, 174, 0, 255), 1.0f)
				}, new GradientAlphaKey[] {
					new GradientAlphaKey (0.0f, 0.0f),
					new GradientAlphaKey (1.0f, 0.1f),
					new GradientAlphaKey (0.0f, 1.0f)
				});
				col.color = grad;
				colR.color = grad;
			}

			else if (carIdentifier == "AI05") {

				var jetR1 = AI05JetRight1.colorOverLifetime;
				var jetR2 = AI05JetRight2.colorOverLifetime;
				var jetR3 = AI05JetRight3.colorOverLifetime;
				var jetL1 = AI05JetLeft1.colorOverLifetime;
				var jetL2 = AI05JetLeft2.colorOverLifetime;
				var jetL3 = AI05JetLeft3.colorOverLifetime;

				Gradient grad = new Gradient ();
				grad.SetKeys (new GradientColorKey[] {
					new GradientColorKey (new Color32 (220, 255, 0, 255), 0.0f),
					new GradientColorKey (new Color32 (220, 255, 0, 255), 1.0f)
				}, new GradientAlphaKey[] {
					new GradientAlphaKey (0.0f, 0.0f),
					new GradientAlphaKey (1.0f, 0.1f),
					new GradientAlphaKey (0.0f, 1.0f)
				});
				jetR1.color = grad;
				jetR2.color = grad;
				jetR3.color = grad;
				jetL1.color = grad;
				jetL2.color = grad;
				jetL3.color = grad;
			}
		}
	}

	public void AI04StandardSpeedParticleEmissions() {	//adjust particle effects according to speed

		//make aura glow more when moving faster
		if (acceleration >= 0.18) {
			backLightAura.intensity = (acceleration * 100f) * 0.0279f;
		} else {	//else keep the glow at a base number
			backLightAura.intensity = 0.5f;
		}
			
		//change force of emissions particles
		if (acceleration > 0) {
			var forceMiddle = jetMiddle.forceOverLifetime;		//middle jet
			forceMiddle.zMultiplier = (acceleration * 100f) * 1f;	

			var forceLeft = jetLeft.forceOverLifetime;		//middle jet
			forceLeft.zMultiplier = (acceleration * 100f) * 0.4f;	

			var forceRight = jetRight.forceOverLifetime;		//middle jet
			forceRight.zMultiplier = (acceleration * 100f) * 0.4f;	
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

	public void AI06StandardSpeedParticleEmissions() {	//adjust particle effects according to speed

		//make aura glow more when moving faster
		if (acceleration >= 0.18) {
			backLightAura.intensity = (acceleration * 100f) * 0.0279f;
		} else { //else keep the glow at a base number
			backLightAura.intensity = 0.5f;
		}

		//change force of emissions particles
		if (acceleration > 0) {
			var forceLeft = jetLeft.forceOverLifetime;		//middle jet
			forceLeft.zMultiplier = (acceleration * 100f) * 0.8f;	

			var forceRight = jetRight.forceOverLifetime;		//middle jet
			forceRight.zMultiplier = (acceleration * 100f) * 0.8f;	
		}
		else {
			var forceRight = jetRight.forceOverLifetime;
			forceRight.zMultiplier = 0f;
			var forceLeft = jetLeft.forceOverLifetime;
			forceLeft.zMultiplier = 0f;
		}
	}

	public void AI05StandardSpeedParticleEmissions() {	//adjust particle effects according to speed

		//make aura glow more when moving faster
		if (acceleration >= 0.18) {
			backLightAura.intensity = (acceleration * 100f) * 0.0279f;
		} else { //else keep the glow at a base number
			backLightAura.intensity = 0.5f;
		}
			
		//change force of emissions particles
		if (acceleration > 0) {

			var forceJetRight1 = AI05JetRight1.forceOverLifetime;		//middle jet
			forceJetRight1.zMultiplier = (acceleration * 100f) * 1f;	

			var forceJetRight2 = AI05JetRight2.forceOverLifetime;		//middle jet
			forceJetRight2.zMultiplier = (acceleration * 100f) * 0.4f;	

			var forceJetRight3 = AI05JetRight3.forceOverLifetime;		//middle jet
			forceJetRight3.zMultiplier = (acceleration * 100f) * 0.4f;	

			var forceJetLeft1 = AI05JetLeft1.forceOverLifetime;		//middle jet
			forceJetLeft1.zMultiplier = (acceleration * 100f) * 1f;	

			var forceJetLeft2 = AI05JetLeft2.forceOverLifetime;		//middle jet
			forceJetLeft2.zMultiplier = (acceleration * 100f) * 0.4f;	

			var forceJetLeft3 = AI05JetLeft3.forceOverLifetime;		//middle jet
			forceJetLeft3.zMultiplier = (acceleration * 100f) * 0.4f;		
		}
		else {
			var forceJetRight1 = AI05JetRight1.forceOverLifetime;		//middle jet
			forceJetRight1.zMultiplier = 0f;
			var forceJetRight2 = AI05JetRight2.forceOverLifetime;		//middle jet
			forceJetRight2.zMultiplier = 0f;
			var forceJetRight3 = AI05JetRight3.forceOverLifetime;		//middle jet
			forceJetRight3.zMultiplier = 0f;

			var forceJetLeft1 = AI05JetLeft1.forceOverLifetime;		//middle jet
			forceJetLeft1.zMultiplier = 0f;
			var forceJetLeft2 = AI05JetLeft2.forceOverLifetime;		//middle jet
			forceJetLeft2.zMultiplier = 0f;
			var forceJetLeft3 = AI05JetLeft3.forceOverLifetime;		//middle jet
			forceJetLeft3.zMultiplier = 0f;
		}
	}
		
	/// <summary>
	/// Set the last checkpoint number the AI has passed.
	/// </summary>
	public void setcurrentCheckpointNo(int checkpointNo) {

		currentCheckpoint = checkpointNo;
	}

	/// <summary>
	/// Returns the last checkpoint number the AI has passed.
	/// </summary>
	public int getcurrentCheckpointNo() {

		return currentCheckpoint;
	}

	/// <summary>
	/// Increments the lap so long as it is less than the total number of laps in the race.
	/// </summary>
	public void incrementLap() {
		if (lap < GameObject.Find("Player").GetComponent<PlayerMovement>().getRaceNumOfLaps()) {	//if less than total number of laps in the race
			lap++;
		} else {	//else the AI has finished its final lap
			setMovementDisabled (true);	//disable the vehicle's movement
			GameObject.Find ("GUICanvas").GetComponent<GUITimer> ().racerFinished (gameObject.name);	//update the GUI to say the vehicle has finished
		}	
	}

	/// <summary>
	/// Set the current checkpoint number the AI is heading towards.
	/// </summary>
	public void setcurrentTargetNo(int targetNo) {
		
		currentTargetNo = targetNo;
	}

	/// <summary>
	/// Sets bool indicating whether the car can move.
	/// </summary>
	public void setMovementDisabled(bool value) {
		movementDisabled = value;
	}
}
