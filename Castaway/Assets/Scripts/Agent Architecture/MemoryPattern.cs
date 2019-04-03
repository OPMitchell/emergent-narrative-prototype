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

        if(Sender)
        {
            if(action.Status == Status.Failed)
                desirability -= 1.0f;
            else if (action.Status == Status.Successful)
                desirability += 1.0f;
        }

        if(!Sender)
        {
            StatName stat = action.Effect.Stat;
            float change = action.Effect.Change;
            
            if((int)stat > 0 && (int)stat < Precondition.physicalIndex)
            {
                desirability += change;
            }
            else if((int)stat >= Precondition.physicalIndex && (int)stat < Precondition.limitIndex)
            {
                desirability += (-1.0f*change);
            }
        }
        return desirability;
    }
}
