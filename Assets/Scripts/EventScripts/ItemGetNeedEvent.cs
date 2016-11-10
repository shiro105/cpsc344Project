﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This event occurs if the player inspects it. The player will be given an item/uses an item if they have it. 
// If the player is getting an item, the item list MUST have only one item.
// If you want this event to accept any item for a slot, have "..." be in a slot in the itemAvailableOrNeed list.
public class ItemGetNeedEvent : MonoBehaviour {

	public List<string> itemAvailableOrNeed = new List<string>();			//What items can the player acquire/use here?
	public bool isUsingItemEvent;											//Is the player using an item here?
	public bool deactivateOnComplete;										//Will this object get deactivated upon being complete?

	//Checks whether this event will be destroyed once it's complete.
	void Update()
	{
		if(gameObject.GetComponent<HasSolvedEvent>().GetIfSolvedEvent() == true && deactivateOnComplete == true)
			gameObject.SetActive(false);
	}

	// Takes the particle emitter from the child and activates it when the player approaches this.
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player" && gameObject.transform.childCount > 0)
			gameObject.transform.GetChild(0).gameObject.SetActive(true);
	}

	// When the player steps away from this object, the child will stop emitting particles
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player" && gameObject.transform.childCount > 0)
			gameObject.transform.GetChild(0).gameObject.SetActive(false);
	}

	//This determines if the player is inspecting said spot and will either pick up an item or use an item.
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.name == "Player")
		{
			PlayerActions player = other.gameObject.GetComponent<PlayerActions>();
			if(player.isInteracting == true)
			{
				if(gameObject.GetComponent<HasSolvedEvent>().GetIfSolvedEvent() == false)
				{
					if(isUsingItemEvent == true)
					{
						//If the items that the event is taking in have the value "...", this means anything can be accepted into the object.
						if(itemAvailableOrNeed[0] == "...")
						{
							GameObject.Find("Main Camera").GetComponent<PlayerMessage>().DisplayOneMessage("Used " + player.GetItem(0));
							player.RemoveFromInventory(player.GetItem(0));
							itemAvailableOrNeed.Remove(itemAvailableOrNeed[0]);

							if(itemAvailableOrNeed.Count == 0)
								gameObject.GetComponent<HasSolvedEvent>().SetIfSolvedEvent(true);
						}
						else
						{
							//We look through the player's inventory to see if this event has an item that the player can use. If the player has multiple items
							for(int i = 0; i < player.GetInventoryLength(); i++)
							{
								string currItem = player.GetItem(i);
								if(itemAvailableOrNeed.Contains(currItem) == true)
								{
									bool usedSucessful = player.RemoveFromInventory(currItem);

									if(usedSucessful == true)
									{
										GameObject.Find("Main Camera").GetComponent<PlayerMessage>().DisplayOneMessage("Used " + currItem);
										itemAvailableOrNeed.Remove(currItem);
										if(itemAvailableOrNeed.Count == 0)
											gameObject.GetComponent<HasSolvedEvent>().SetIfSolvedEvent(true);
									}
									break;
								}
							}
						}
					}
					else
					{
						bool addSuccess = player.AddToInventory(itemAvailableOrNeed[0]);
						if(addSuccess == true)
						{
							GameObject.Find("Main Camera").GetComponent<PlayerMessage>().DisplayOneMessage("Obtained " + itemAvailableOrNeed[0]);
							gameObject.GetComponent<HasSolvedEvent>().SetIfSolvedEvent(true);
						}
					}

					gameObject.GetComponent<HasSolvedEvent>().CheckIfPartOfChainEvent();
				}
				player.isInteracting = false;
			}
		}
	}
		
}
