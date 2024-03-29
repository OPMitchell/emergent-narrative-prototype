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
            if(action.Status != Status.ResendSent)
                Testing.WriteToLog(transform.name, transform.name + " is executing action: " + Testing.GetActionInfo(action));
            character.currentAction = action;
            StartCoroutine(Execute(action));
        }
    }

    private IEnumerator Execute(Action action)
    {
        StartCoroutine(GetComponent<ActionExecutor>().ExecuteAction(action));
        yield return new WaitUntil(() => !GetComponent<ActionExecutor>().Executing);
        character.currentAction = null;
        //store a memory of the action
        AppraiseAction(action);
        AddMemory(action);
    }

    private void AppraiseAction(Action action)
    {
        GetComponent<EmotionalPersonalityModel>().AppraiseActionAsSender(action);
        GetComponent<PhysicalResourceModel>().AppraiseActionAsSender(action);
    }

    private void AddMemory(Action action)
    {
        GetComponent<MemoryManager>().AddSentMemoryPattern(new MemoryPattern(GetComponent<MemoryManager>().CurrentID, action, true));
    }
}
