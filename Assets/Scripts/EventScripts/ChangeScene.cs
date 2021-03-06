﻿using UnityEngine;
using System.Collections;

// This script changes the scene the player is in to another one, via a transition. 
// It is activated by simply approaching it or when an associated gameobject has its event complete.
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	public GameObject activeUponComplete;	//Will this script activate when the player has completed the specificed gameobject's event?
	public float timeLimit;					//Is this transition forced after X time? Else, the player needs to walk into the area of the gameobject to trigger it.
	public string nameOfScene;				//What's the name of the scene to change to?

	private bool hasActivatedAlready = false;	//Used if this loadEvent's associated with another gameObject being complete.
	private bool inTrigger = false;				//Used to check if the player is in the gameObject's vicinity.

	// If this load scene is associated with a gameobject finishing, the player will be taken to a new scene at X time.
	void Update()
	{
		if(activeUponComplete != null && hasActivatedAlready == false)
		{
			if(activeUponComplete.GetComponent<HasSolvedEvent>().GetIfSolvedEvent() == true)
			{
				hasActivatedAlready = true;
				Invoke("TransitionToScene",timeLimit);
			}
		}
		else if(inTrigger == true)
			Invoke("TransitionToScene",timeLimit);
	}

	// If the player enters the hitbox of this gameobject, it loads the next scene.
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player" && gameObject.tag != "InspectEvent")
		{
			other.GetComponent<PlayerActions>().canMove = false;
			Invoke("TransitionToScene",timeLimit);
		}
	}

	// If the gameobject is an InspectEvent, it'll activate a boolean that will simulate the player is staying in it.
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Player" && gameObject.tag == "InspectEvent")
		{
			if(other.gameObject.GetComponent<PlayerActions>().isInteracting == true)
				inTrigger = true;
			else
				inTrigger = false;
		}
	}

	// Loads the next scene
	void TransitionToScene()
	{
		SceneManager.LoadScene(nameOfScene);
	}
}
