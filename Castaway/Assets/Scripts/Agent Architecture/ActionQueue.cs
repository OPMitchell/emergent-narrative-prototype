using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : EventPriorityQueue
{
    Coroutine coroutine;

    public PriorityQueue GetQueue()
    {
        return queue;
    }

    public override void CheckQueue()
    {
        if(!queue.IsEmpty() && !GetComponent<ActionExecutor>().Executing)
        {
            Action action = queue.Remove();
            StartCoroutine(Execute(action));
        }
    }

    private IEnumerator Execute(Action action)
    {
        StartCoroutine(GetComponent<ActionExecutor>().ExecuteAction(action));
        yield return new WaitUntil(() => !GetComponent<ActionExecutor>().Executing);
        Debug.Log("Finished executing");
        //queue.Remove();
        /*
        if(action.Status == Status.Successful)
        {
            Transform target = GameObject.Find(action.Target).transform;
            if(target.GetComponent<ReceivingQueue>() != null && target.tag == "Character")
            {
                target.GetComponent<ReceivingQueue>().QueueAction(action);
                if(action.Target != transform.name)
                    GetComponent<ReceivingQueue>().QueueAction(action);
            }
        }
        */
    }

}
