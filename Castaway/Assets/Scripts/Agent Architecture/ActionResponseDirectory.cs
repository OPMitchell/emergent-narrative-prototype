using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionResponseDirectory : MonoBehaviour {

	private GameManager manager;
	[SerializeField] private ActionResponse[] ActionResponseList;
	private ActionDirectory actionDirectory;

	void Awake()
	{
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
		actionDirectory = GetComponent<ActionDirectory>();
	}

    public Action FindResponse(Action receivedAction)
    {
		foreach(ActionResponse response in ActionResponseList)
		{
			if(response.ReceivedActionName.ToLower() == receivedAction.Name.ToLower())
			{
				foreach(Response r in response.Responses)
				{
					Action a = actionDirectory.GetAction(r.ResponseActionName);
					bool satisfied = true;
					foreach(Precondition precondition in r.Preconditions)
					{
						if(!precondition.IsSatisfied(gameObject, gameObject))
						{
							satisfied = false;
						}
					}
					if(satisfied)
					{
						a.Priority = 0;
						return a;
					}
				}
			}
		}
		Testing.WriteToLog(transform.name, transform.name + " found no valid response found for action: " + Testing.GetActionInfo(receivedAction));
        return null;
    }
}
