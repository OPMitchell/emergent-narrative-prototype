using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

public class Plan
{
    private LinkedList<Action> actions = new LinkedList<Action>();
    public float Score { get; set; }

    public Plan()
    {
        actions = new LinkedList<Action>();
        Score = 0.0f;
    }

    public Plan(Plan plan)
    {
        actions = new LinkedList<Action>(plan.actions);
    }

    public void AddAction(Action action)
    {
        actions.AddFirst(action);
    }

    public void RemoveAction(Action action)
    {
        actions.Remove(action);
    }

    public bool isEqual(Plan p)
    {
        return (actions.SequenceEqual(p.actions));
    }

    public int CountActions()
    {
        return actions.Count;
    }

    public LinkedList<Action> GetActions()
    {
        return actions;
    }

    public Action GetCurrentAction()
    {
        if(actions.Count > 0)
        {
            return actions.ElementAt(0);
        }
        return null;
    }

    public void SetScore(float score)
    {
        Score = score;
    }

    public void Evaluate(MemoryManager memoryManager)
    {
        SetScore(0.0f);

        //Heuristic 1 - get length
        int numberOfActions = actions.Count;

        //Heuristic 2 - check similar memories
        foreach(Action action in actions)
        {
            MemoryPattern memory1 = memoryManager.RetrieveMemoryPattern(action.Name);
        }
    }
}