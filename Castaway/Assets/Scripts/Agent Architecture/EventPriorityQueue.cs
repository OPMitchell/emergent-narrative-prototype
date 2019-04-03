using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventPriorityQueue : MonoBehaviour 
{
	protected PriorityQueue queue = new PriorityQueue();

    public void QueueAction(Action action)
    {
        if(action.Status != Status.ResendSent)
        {
            if(this.GetType() == typeof(ActionQueue))
                Testing.WriteToLog(transform.name, transform.name + " added action: " + Testing.GetActionInfo(action) + " to their action queue");
            else if(this.GetType() == typeof(ReceivingQueue))
                Testing.WriteToLog(transform.name, transform.name + " added action: " + Testing.GetActionInfo(action) + " to their receiving queue");
        }
        queue.Add(action.Priority, action); //TODO: implement priority system!
    }

    void Update()
    {
        CheckQueue();
    }

    public abstract void CheckQueue();
}
