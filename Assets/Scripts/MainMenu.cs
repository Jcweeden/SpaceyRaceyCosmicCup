using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script controls the main menu. Simply rotates the camera and waits for the player to press return to play.
/// </summary>
public class MainMenu : MonoBehaviour {

	GameObject MenuCamera;
	bool inMainMenu;



	void Awake() {
		//limit the framerate in the game
		QualitySettings.vSyncCount = 0; 
		Application.targetFrameRate = 60;
	}

	void Start() {
		MenuCamera = GameObject.Find ("MainMenu Camera");	//get the camera so it can be rotated
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) { 	//press return to start the game
			SceneManager.LoadScene("a");			
		} 
		MenuCamera.transform.Rotate(new Vector3(0,0.1f,0));	//simply rotates the camera for a cool effect (I think so anyway)
	}
}
