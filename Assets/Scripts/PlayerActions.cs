﻿using UnityEngine;
using System.Collections.Generic;

// This class will simply have the player controls. This class will have an interact button. This will do various things with the enviroment. 
// This  will also contain the player's inventory as well as methods to modify it.
// I used https://www.youtube.com/watch?v=5pkeRlpjFzQ&index=4&list=WL to help fix some bugs regarding movement.

public class PlayerActions : MonoBehaviour {

	public List<string> itemList = new List<string>();	//The items the player will collect will be stored as strings, since they aren't that complicated.
	public float walkAcceleleration = 100f;				//How fast will the player accelerate?
	public float maxWalkSpeed = 2f;						//The max speed the player can move
	public float maxFallSpeed = 10f;
	public bool isInteracting = false;					//Is the player interacting with anything?
	public bool isGrounded = false;						//Is the player on the floor?
	public GameObject cameraControl;					//The camera control attatched to the player

	private Rigidbody playerControl;					//The rigidbody controller attatched to the player.
	private Vector3 horizontalMovement;					//Used to constrain the player's top speed
	private Vector3 verticalMovement;					//Used to constrain the player's falling speed.

	//Makes sure there's only one player in the room.
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	//Assigns the playerController to a private variable.
	void Start()
	{
		playerControl = gameObject.GetComponent<Rigidbody>();
	}
	
	//Checks if the player's hit the interact button. Else, the player is moving around.
	void Update () 
	{
		if(isInteracting == false)
		{
			transform.rotation = Quaternion.Euler(0,cameraControl.GetComponent<MouseCamera>().currentYRotation,0);
			if(isGrounded == false)
			{
				verticalMovement = new Vector3(0,playerControl.velocity.y,0);
				if(verticalMovement.magnitude > maxFallSpeed)
				{
					verticalMovement.Normalize();
					verticalMovement *= maxFallSpeed;
					playerControl.velocity = verticalMovement;
				}
			}
			else
			{
				if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
					playerControl.velocity = Vector3.zero;
				else
				{
					playerControl.AddRelativeForce(Input.GetAxis("Horizontal") * walkAcceleleration, 0, Input.GetAxis("Vertical") * walkAcceleleration);
					horizontalMovement = new Vector3(playerControl.velocity.x, 0, playerControl.velocity.z);
					if(horizontalMovement.magnitude > maxWalkSpeed)
					{
						horizontalMovement.Normalize();
						horizontalMovement *= maxWalkSpeed;
						playerControl.velocity = horizontalMovement;
					}
				}
			}
		}
	}

	//Checks if the player is off the ground.
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Ground")
			isGrounded = false;
	}

	//Checks if the player is on the ground or if they entered an area that has an approach event.
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "ApproachEvent")
			isInteracting = true;
		if(other.gameObject.tag == "Ground")
			isGrounded = true;
	}

	//If the player is within range of an interactive object, this will make the player inspect it.
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "InspectEvent" && Input.GetKeyDown(KeyCode.E) == true)
			isInteracting = true;
	}

	//Removes an item from the inventory
	public void RemoveFromInventory(string itemToRemove)
	{
		if(CheckIfPlayerHasItem(itemToRemove) == true)
		{
			itemList.Remove(itemToRemove);
			print("You removed " + itemToRemove);
		}
	}

	//Adds an item to the inventory.
	public void AddToInventory(string itemToAdd)
	{
		itemList.Add(itemToAdd);
		print("You got " + itemToAdd);
	}

	//Checks if the player has the said item in their inventory and returns true if they do.
	public bool CheckIfPlayerHasItem(string itemToCheck)
	{
		if(itemList.Count == 0)
			return false;
		else if(itemList.Contains(itemToCheck))
			return true;
		return false;
	}

	//Returns the item at the currentIndex
	public string GetItem(int index)
	{
		return itemList[index];
	}

	//Returns the length of the inventory
	public int GetInventoryLength()
	{
		return itemList.Count;
	}
}