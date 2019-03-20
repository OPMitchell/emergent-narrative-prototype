using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingController : MonoBehaviour
{
	private GameManager manager;
	private BuildManager buildManager;
	private Character character;
	private ActionQueue actionQueue;
	public bool CanBuild {get;set;}
	private Action currentAction;

	void Start () 
	{
		CanBuild = true;
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
		buildManager = GameObject.Find("GameManager").GetComponent<BuildManager>();
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
		foreach(GameObject t in manager.GetTaggedBuildingTiles())
		{
			Tile tile = t.GetComponent<Tile>();
			if(tile.GetLock(gameObject))
			{
				currentAction = new Action("Build", "Build", name, tile.name, 2);
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
