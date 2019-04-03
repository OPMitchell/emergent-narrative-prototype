using System.Collections;
using System.Collections.Generic;

public class MemoryPattern
{
    public int ID {get; private set;}
    public string[] Keywords {get; private set;}
    public ActionType Type {get; private set;}
    public float Weight {get; private set;}
    public float Desirability {get; private set;}
    
    public MemoryPattern(int id, Action action)
    {
        ID = id;
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

        return desirability;
    }
}
