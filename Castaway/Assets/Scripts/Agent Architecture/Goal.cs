using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using UnityEngine;

public enum GoalType
{
    Pursuit = 1,
    Interest = 2
};

[System.Serializable]
public class Goal
{
    [SerializeField] private string name;
    public string Name
    {
        get
        {
            return name;
        }
    }
    
    [SerializeField] private GoalType type;
    public GoalType Type
    {
        get
        {
            return type;
        }
        private set
        {
            type = value;
        }
    }

    [SerializeField] private GameObject targetCharacter;
    public GameObject TargetCharacter
    {
        get
        {
            return targetCharacter;
        }
        private set
        {
            targetCharacter = value;
        }
    }
    
    [SerializeField] private Precondition successCondition;
    public Precondition SuccessCondition
    {
        get
        {
            return successCondition;
        }
        private set
        {
            successCondition = value;
        }
    }
    public bool Complete {get; set;}

    private GameObject owner;
    public GameObject Owner
    {
        get
        {
            return owner;
        }
        private set
        {
            owner = value;
        }
    }


    public Plan Plan {get; private set;}
    public List<Action> FailedActions { get; private set; }
    public int TimesFailed { get; set; }

    public Goal()
    {
        FailedActions = new List<Action>();
        Complete = false;
    }

    public Goal(GoalType type, GameObject targetCharacter, Precondition successCondition)
    {
        FailedActions = new List<Action>();
        Type = type;
        TargetCharacter = targetCharacter;
        SuccessCondition = successCondition;
        Complete = false;
    }

    public void SetPlan(Plan p)
    {
        Plan = p;
    }

    public bool IsSatisfied()
    {
        bool holdingItem = IsHoldingItem();
        bool emotionCondition = IsEmotionCorrect();
        return (holdingItem && emotionCondition);
    }

    private bool IsHoldingItem()
    {
        if(successCondition.HoldingItem != null)
        {
            Character c = owner.GetComponent<Character>();
            if(c.heldItem != null)
            {
                Item heldItem = c.heldItem.GetComponent<Item>();
                Item targetItem = successCondition.HoldingItem.GetComponent<Item>();
                if(heldItem.resource != targetItem.resource)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private bool IsEmotionCorrect()
    {
        EmotionalPersonalityModel epm = TargetCharacter.GetComponent<EmotionalPersonalityModel>();
        float value = (float)epm.GetEmotionValue(SuccessCondition.Emotion);

        if(successCondition.BoolCondition == BooleanCondition.LessThan)
        {
            if(value < successCondition.Value)
                return true;
            else
                return false;
        }
        else if(successCondition.BoolCondition == BooleanCondition.GreaterThan)
        {
            if(value > successCondition.Value)
                return true;
            else
                return false;
        }
        else if(successCondition.BoolCondition == BooleanCondition.EqualTo)
        {
            if(value == successCondition.Value)
                return true;
            else
                return false;
        }
        return false;
    }
}
