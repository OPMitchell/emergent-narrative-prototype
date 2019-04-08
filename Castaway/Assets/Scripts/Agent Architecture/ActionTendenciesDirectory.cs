using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionTendency
{
	[SerializeField] private Precondition trigger;
	public Precondition Trigger
	{
		get
		{
			return trigger;
		}
	}
	[SerializeField] private string action;
	public string Action
	{
		get
		{
			return action;
		}
	}

	public bool triggered {get;set;}
}

public class ActionTendenciesDirectory : MonoBehaviour
{
	[SerializeField] private ActionTendency[] actionTendencies;
	public ActionTendency[] ActionTendencies
	{
		get
		{
			return actionTendencies;
		}
	}

	void Update()
	{
		foreach(ActionTendency aT in actionTendencies)
		{
			if(!aT.Trigger.IsSatisfied(gameObject,gameObject))
			{
				aT.triggered = false;
			}
			if(!aT.triggered && aT.Trigger.IsSatisfied(gameObject, gameObject))
			{
				aT.triggered = true;
				Action action = GetComponent<ActionDirectory>().GetAction(aT.Action);
				if(action != null)
				{
					action.Priority = 0;
					GetComponent<ActionQueue>().QueueAction(action);
				}
			}
		}
	}
}
