using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : EventPriorityQueue
{
    Coroutine coroutine;
    Character character;
    ReceivingQueue receivingQueue;

    void Awake()
    {
        character = GetComponent<Character>();
        receivingQueue = GetComponent<ReceivingQueue>();
    }

    public PriorityQueue GetQueue()
    {
        return queue;
    }

    public override void CheckQueue()
    {
        if(!queue.IsEmpty() && !GetComponent<ActionExecutor>().Executing && character.IsFree() && receivingQueue.IsEmpty())
        {
            Action action = queue.Remove();
            if(action.IsSatisfied())
            {            
                Testing.WriteToLog(transform.name, transform.name + " is executing action: " + Testing.GetActionInfo(action));
                character.currentAction = action;
                StartCoroutine(Execute(action));
            }
            else
            {
                Testing.WriteToLog(transform.name, transform.name + " tried to execute action: " + Testing.GetActionInfo(action) + ", but failed the precondition.");
            }
        }
    }

    private IEnumerator Execute(Action action)
    {
        StartCoroutine(GetComponent<ActionExecutor>().ExecuteAction(action));
        yield return new WaitUntil(() => !GetComponent<ActionExecutor>().Executing);
        character.currentAction = null;
        Debug.Log("Finished executing");
        //store a memory of the action
        GetComponent<MemoryManager>().AddSentMemoryPattern(new MemoryPattern(GetComponent<MemoryManager>().CurrentID, action, true));
    }

}
