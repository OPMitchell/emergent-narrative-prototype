using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CuttingController : MonoBehaviour
{
	private GameManager manager;
	private Character character;
	private ActionQueue actionQueue;
	public bool CanCut {get;set;}
	private Action currentAction;

	void Start () 
	{
		CanCut = true;
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
		character = GetComponent<Character>();
		actionQueue = GetComponent<ActionQueue>();	
		currentAction = character.currentAction;
	}
	
	void Update() 
	{
		//If we already have an action on the go that hasn't finished then return
		if(currentAction != null && currentAction.Status == Status.Sent)
		{
			return;
		}
		//If we've completed our previous action or haven't got one then proceed
		foreach(GameObject tile in manager.GetTaggedTreeTiles())
		{
			if(tile.GetComponent<Tile>().GetLock(gameObject))
			{
				currentAction = new Action("Cut", "Cut", name, tile.name, null, null, null, null, 1);
				if(!actionQueue.Contains(currentAction))
				{
					currentAction.SetStatus(Status.Sent);
					GetComponent<ActionQueue>().QueueAction(currentAction);
					break;
				}
			}
		}
	}

}
