using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// GUI timer controls all the UI that is displayed on screen during the race, and the stats after, excluding the positioning system.
/// </summary>
public class GUITimer : MonoBehaviour {


	public GameObject[] GUIElements;				//arraylist containing all GUI elements

	public Text CurrentLapTimerLabel;				//Text that shows current lap time

	public Text CurrentLapTimerLabelMinutes;		//Text that shows current lap minutes counter
	public Text CurrentLapTimerLabelSeconds;		//Text that shows current lap seconds counter
	public Text CurrentLapTimerLabelMS;				//Text that shows current lap millisecs counter


	public Text BestLapTimerLabel;					//Text that shows time of best lap
	public Text LapCounterText;						//Text that shows number of laps completed
	public Text PrevLapText;						//Text that appears to show previous lap time

	public Text countdownText;						//countdown at the start of the race
	int countdownVal;								//int storing the value of the countdown timer

	public Text RaceCompleteText;					//Text that shows time of best lap
	public Text PromptText;							//Text that indicates the user can press enter to restart the game
	public Text PlayerRaceTime;						//Text label that says 'Lap:'
	public Text PlayerRacePosition;					//Text that shows the current postion out of 4
	public GameObject PlayerSprite;					//The image of the player's vehicle that displays upon completing the race



	public List<float> lapTimes;					//a list of all laptimes - used to create total racetime		
	public float bestLapTime;						//the best lap time - shown on screen

	private float time;								//variable delta time is added to to act as a time counter

	private bool raceComplete;						//true if race is complete

	private List<string> finishingPositions;		//list of the finishing order of the racers. A vehicle is added when it finishes

	public int playerPosition;						//stores players position when finishing

	void Start() {
		PlayerSprite.SetActive (false);
		raceComplete = false;
		lapTimes = new List<float> ();
		finishingPositions = new List<string> ();
		bestLapTime = 0f;
		countdownVal = 3;							//number that is counted down from at the start of the race
		playerPosition = 0;

		initStartCountdown ();						//init the countdown to begin the race
	}

	/// <summary>
	/// Run when the scene is loaded. Inits the start countdown before the race starts, and then enables movement on all racers.
	/// </summary>
	public void initStartCountdown() {
		if (countdownVal > 0) {
			countdownText.text = countdownVal.ToString();
			countdownVal--;
			Invoke ("initStartCountdown", 1f);
		} else if (countdownVal == 0) {	//if countdownVal == 0 then the race has begun
			countdownText.text = "START";	//display the GO text
			countdownVal--;
			//enable vehicle movement
			GameObject.Find ("Player").GetComponent<PlayerMovement>().setMovementDisabled(false);
			GameObject.Find ("AI04").GetComponent<AIRacer2>().setMovementDisabled(false);
			GameObject.Find ("AI05").GetComponent<AIRacer2>().setMovementDisabled(false);
			GameObject.Find ("AI06").GetComponent<AIRacer2>().setMovementDisabled(false);


			Invoke ("initStartCountdown", 1f);
		} else if (countdownVal == -1) {
			countdownText.text = "";	//after the counter has run out set it to empty to hide it
		}
	}

	/// <summary>
	/// Updates this instance to check if the player is pressing return to restart the game once it has finished. Also runs the timer counting lap times
	/// and updating these values on the GUI.
	/// </summary>
	void Update() {
		if (raceComplete == true) {						//if the race is complete
			if (Input.GetKeyDown (KeyCode.Return)) { 	//allow the player to press return to restart
				SceneManager.LoadScene ("a"); 
			} 
		}

		//if the race has started but has not been completed
		if (GameObject.Find("StartLine").GetComponent<StartLine>().getRaceStarted() && raceComplete == false) {

			time += Time.deltaTime;		//increment the timer by adding delta time


			//update the current lap timer
			var minutes = time / 120; 			//divide the guiTime by 60 to get minutes
			var seconds = time % 60; 			//modulo division for seconds
			var fraction = (time * 100) % 100;	//get the remaining millisecs

			CurrentLapTimerLabelMinutes.text = string.Format ("{0:00}", minutes);
			CurrentLapTimerLabelSeconds.text = string.Format ("{0:00}", seconds);
			CurrentLapTimerLabelMS.text = string.Format ("{0:00}", fraction);
		}
	}


	/// <summary>
	/// Run when a lap is completed. Resets the timer and updates the best lap time if applicable.
	/// </summary>
	public void ResetTime() {
		if (time != 0f) {				//if the timer does not equal 0 then it means a lap has been completed
			lapTimes.Add(time); 		//add to laptimes
			DisplayPreviousLapTime();	//and show the previous lap time on screen

			if (bestLapTime == 0 || time < bestLapTime) {	//if the first lap time, or the best laptime
				bestLapTime = time;							//store it as the best lap time

				//display this value on screen as the best lap
				var bestMinutes = bestLapTime / 120; 			//divide the guiTime by 60 to get minutes
				var bestSeconds = bestLapTime % 60; 			//modulo division for seconds
				var bestFraction = (bestLapTime * 100) % 100;	//get the remaining millisecs

				BestLapTimerLabel.text = string.Format ("Best Lap: {0:00}:{1:00}:{2:00}",bestMinutes,bestSeconds,bestFraction);
			}
			time = 0f;				//reset the timer for next lap
		}
	}

	/// <summary>
	/// Displayes the previous lap time on screen for 5 seconds.
	/// </summary>
	public void DisplayPreviousLapTime() {

		//uses the last added laptime to work out mins, sec, ms
		var bestMinutes = lapTimes [lapTimes.Count - 1] / 120; 			//divide the guiTime by 60 to get minutes
		var bestSeconds = lapTimes [lapTimes.Count - 1] % 60; 			//modulo division for seconds
		var bestFraction = (lapTimes [lapTimes.Count - 1] * 100) % 100;	//get the remaining millisecs

		//displays this time on screen
		PrevLapText.text = "Prev Lap: " + string.Format ("{0:00}:{1:00}:{2:00}", bestMinutes, bestSeconds, bestFraction);

		Invoke ("HidePrevLapTimeText", 5f);	//and then hides the previous lap text after 5 seconds of being displayed
	}

	/// <summary>
	/// Hides the previous lap time text.
	/// </summary>
	public void HidePrevLapTimeText() {
		PrevLapText.text = "";
	}

	/// <summary>
	/// Updates the amount of laps completed text.
	/// </summary>
	public void UpdateLapsElapsedText(int lap, int raceNumOfLaps) {
		LapCounterText.text = "Lap " + lap + " / " + raceNumOfLaps;
	}

	/// <summary>
	/// Adds a racers to the list of racers who have finished all laps.
	/// </summary>
	public void racerFinished(string racerID) {
		finishingPositions.Add (racerID);
	}


	/// <summary>
	/// Invoked upon completing the race. Disables all GUI elements so the final stats can be displayed on a clean screen.
	/// </summary>
	public void completedRace(){
		raceComplete = true;
		for (int i = 0; i < GUIElements.Length; i++) {	//disable every GUI element currently on screen
			GUIElements [i].SetActive (false);
		}

		playerPosition = finishingPositions.Count;	//the amount of racers in the list is equal to the player's finishing position

		Invoke ("DisplayStats", 1f);	//a second after completing your final lap, display the race stats
		}


	/// <summary>
	/// Once the race has finished this method is called which displays the positions of the vehicles, total race time, and prompt to restart
	/// </summary>
	public void DisplayStats(){


		//dependent on where the player finishes display the appropriate position text will be displayed
		switch (playerPosition) {

		case 0:
			PlayerRacePosition.text = "1st";
			break;
		case 1:
			PlayerRacePosition.text = "2nd";
			break;
		case 2:
			PlayerRacePosition.text = "3rd";
			break;
		case 3:
			PlayerRacePosition.text = "4th";
			break;
		}
			
		PlayerSprite.SetActive (true);		//the image showing the player is displayed next to their position

		float totalRaceTime = 0f;
		for (int i = 0; i < lapTimes.Count; i++) {		//the total race time is calculated by summing up all lap times
			totalRaceTime = totalRaceTime + lapTimes[i];
		}

		RaceCompleteText.text = "Race Complete";	//the race complete text is displayed

		var minutes = totalRaceTime / 120; 			//divide the GuiTime by 60 to get minutes
		var seconds = totalRaceTime % 60; 			//modulo division for seconds
		var fraction = (totalRaceTime * 100) % 100;	//get the remaining millisecs

		//update the label value to display the time
		PlayerRaceTime.text = string.Format ("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);

		PromptText.text = "press Enter to restart";	// anddd finally display the prompt to restart the game
	}
}
