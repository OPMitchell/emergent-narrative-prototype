﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HaulingController : MonoBehaviour
{
	private GameManager manager;
	private Character character;
	private ActionQueue actionQueue;
	public bool CanHaul {get;set;}
	private Action currentAction;

	void Start () 
	{
		CanHaul = true;
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
		character = GetComponent<Character>();
		actionQueue = GetComponent<ActionQueue>();	
		currentAction = character.currentAction;
	}
	
	void Update() 
	{
		List<GameObject> tiles = manager.GetEmptyStockpileTiles();
		if(tiles.Count > 0)
		{
			if(currentAction != null && currentAction.Status == Status.Sent)
			{
				return;
			}
			foreach(GameObject item in manager.GetItemsOutsideStockpile())
			{
				if(item.GetComponent<Item>().GetLock(gameObject))
				{
					currentAction = new Action("HaulItem", "HaulItem", name, item.name, 3);
					if(!actionQueue.Contains(currentAction))
					{
						currentAction.Status = Status.Sent;
						GetComponent<ActionQueue>().QueueAction(currentAction);
						break;
					}
				}
			}
		}
	}

}