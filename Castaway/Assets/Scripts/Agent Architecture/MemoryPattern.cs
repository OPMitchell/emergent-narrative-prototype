using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPattern
{
    public int ID {get; private set;}
    public string[] Keywords {get; private set;}
    public ActionType Type {get; private set;}
    public float Weight {get; private set;}
    public bool Sender {get; private set;}
    public bool PositiveAction{get; private set;}
    public float Desirability {get; private set;}
    
    public MemoryPattern(int id, Action action, bool sender)
    {
        ID = id;
        Sender = sender;
        Keywords = GenerateKeywords(action);
        Type = action.Type;
        Weight = 1.0f; //TO-DO: Implement weights for memory-patterns
        Desirability = CalculateDesirability(action);
    }

    private string[] GenerateKeywords(Action action)
    {
        List<string> keywords = new List<string>();
        keywords.Add(action.SendingCharacter.name);
        keywords.Add(action.TargetObject.name);
        keywords.Add(action.Type.ToString());
        keywords.Add(action.Name.ToString());
        return keywords.ToArray();
    }

    private float CalculateDesirability(Action action)
    {
        float desirability = 0.0f;

        desirability += SuccessFailDesirability(action);
        desirability += SelfStatEffectDesirability(action);
        desirability += TargetStatEffectDesirability(action);

        return desirability;
    }

    private float SuccessFailDesirability(Action action)
    {
        if(Sender)
        {
            if(action.Status == Status.Failed)
                return -0.2f;
            else if (action.Status == Status.Successful)
                return 0.2f;
        }
        return 0.0f;
    }

    private float SelfStatEffectDesirability(Action action)
    {
        StatName stat;
        float change;
        if(!Sender)
        {
            stat = action.TargetEffect.Stat;
            change = action.TargetEffect.Change;
        }
        else
        {
            stat = action.SenderEffect.Stat;
            change = action.SenderEffect.Change;
        }
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
        StatName stat;
        float change;
        float desirability = 0.0f;
        if(Sender)
        {
            stat = action.TargetEffect.Stat;
            change = action.TargetEffect.Change;
            if(stat != StatName.None)
            {
                if((int)stat > 0 && (int)stat < Precondition.physicalIndex)
                    desirability += change;
                else if ((int)stat >= Precondition.physicalIndex && (int)stat < Precondition.limitIndex)
                    desirability += change*(-1.0f);

                if(action.NegativeAction)
                    desirability*=-1.0f;
                
                return desirability;
            }
        }
        return 0.0f;
    }
}
