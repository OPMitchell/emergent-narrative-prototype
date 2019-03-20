using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivingQueue : EventPriorityQueue
{
    public override void CheckQueue()
    {
        //Respond to incoming actions from other characters
        if(!queue.IsEmpty())
        {
            Action receivedAction = queue.Remove();
            //GetComponent<ActionQueue>().QueueAction(response);        
        }
    }
}
