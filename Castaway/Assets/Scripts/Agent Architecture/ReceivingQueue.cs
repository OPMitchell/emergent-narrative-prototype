using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivingQueue : EventPriorityQueue
{
    private ActionResponseDirectory actionResponseDirectory;

    void Awake()
    {
        actionResponseDirectory = GetComponent<ActionResponseDirectory>();
    }

    public override void CheckQueue()
    {
        //Respond to incoming actions from other characters
        if(!queue.IsEmpty())
        {
            Action receivedAction = queue.Remove();
            Testing.WriteToLog(transform.name, transform.name + " received action: " + Testing.GetActionInfo(receivedAction));
            //appraise
            AppraiseAction(receivedAction);
            //store in memory
                // TO-DO: implement memory storage
            //find response
            Action response = actionResponseDirectory.FindResponse(receivedAction);
            if(response != null)
            {
                Testing.WriteToLog(transform.name, transform.name + " responding with action: " + Testing.GetActionInfo(response));
                GetComponent<ActionQueue>().QueueAction(response);   
            }
        }
    }

    public bool IsEmpty()
    {
        return queue.IsEmpty();
    }

    private void AppraiseAction(Action receivedAction)
    {
        StatName stat = receivedAction.Effect.Stat;
        float change = receivedAction.Effect.Change;
        if(stat != StatName.None)
        {
            // TO-DO: implement threshold and decay for emotions
            if((int)stat > 0 && (int)stat < Precondition.physicalIndex)
			    GetComponent<EmotionalPersonalityModel>().AddToValue(stat.ToString(), change);
            else if((int)stat >= Precondition.physicalIndex && (int)stat < Precondition.limitIndex)
			    GetComponent<PhysicalResourceModel>().AddToValue(stat.ToString(), change);
        }
    }


}
