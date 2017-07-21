using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays the raycast beam on screen. Used for debugging.
/// </summary>
public class RaycastDebugBeam : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		float theDistance;

		//Debug
		Vector3 forward = transform.TransformDirection(Vector3.forward)*30;
		Debug.DrawRay (transform.position, forward, Color.green);

		if (Physics.Raycast (transform.position, (forward), out hit)) {
			theDistance = hit.distance;
			print (theDistance + " " + hit.collider.gameObject.name);
		} else {
			print (" nothing hit");
		}
}
}
