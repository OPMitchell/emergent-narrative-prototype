using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

public class Plan
{
    private LinkedList<Action> actions = new LinkedList<Action>();
    public int Score { get; set; }

    public Plan()
    {
        actions = new LinkedList<Action>();
        Score = 0;
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
        return actions.ElementAt(0);
    }
}