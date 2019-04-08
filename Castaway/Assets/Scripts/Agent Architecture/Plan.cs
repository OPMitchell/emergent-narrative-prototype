using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

public class Plan
{
    private LinkedList<Action> actions = new LinkedList<Action>();
    public float Score { get; private set; }

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

    private void SetScore(float score)
    {
        Score = score;
    }

    private void AddToScore(float amount)
    {
        Score += amount;
    }

    public void Evaluate(MemoryManager memoryManager, ActionQueue actionQueue)
    {
        SetScore(0.0f);

        //Heuristic 1 - get length
        int numberOfActions = actions.Count;
        AddToScore((-0.05f)*numberOfActions);

        //Heuristic 2 - check similar memories
        foreach(Action action in actions)
        {
            MemoryPattern memory = memoryManager.RetrieveSentMemoryPattern(action.Name);
            if(memory != null)
                AddToScore(memory.Desirability);
        }

        //Heuristic 3 - forecast result on emotions
        foreach (Action action in actions)
        {
            AddToScore(SelfStatEffectDesirability(action));
            AddToScore(TargetStatEffectDesirability(action));

            for(int i = memoryManager.CurrentID; i > memoryManager.CurrentID - 2; i--)
            {
                MemoryPattern mp = memoryManager.RetrieveSentMemoryPatternByID(i);
                if(mp != null && mp.Action.Compare(action))
                {
                    AddToScore(-1.0f);
                }
            }
        }

    }

    private float SelfStatEffectDesirability(Action action)
    {
        StatName stat = action.SenderEffect.Stat;
        float change = action.SenderEffect.Change;
        if(stat != StatName.None)
        {
            if((int)stat > 0 && (int)stat < Precondition.physicalIndex)
                return change;
            else if ((int)stat >= Precondition.physicalIndex && (int)stat < Precondition.limitIndex)
                return change*(-1.0f);
        }
        return 0.0f;
    }

    private float TargetStatEffectDesirability(Action action)
    {
        float desirability = 0.0f;
        StatName stat = action.TargetEffect.Stat;
        float change = action.TargetEffect.Change;
        if(stat != StatName.None)
        {
            if((int)stat > 0 && (int)stat < Precondition.physicalIndex)
                desirability += change;
            else if ((int)stat >= Precondition.physicalIndex && (int)stat < Precondition.limitIndex)
                desirability += change*(-1.0f);

            if(action.NegativeAction)
                desirability*=-1.0f;
        }
        return desirability;
    }
}