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
    
    [SerializeField] private GameObject owner;
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

    [Range(1,10)][SerializeField] private int priority;
    public int Priority
    {
        get
        {
            return priority;
        }
        private set
        {
            priority = value;
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

    public Plan Plan {get; private set;}
    public List<Action> FailedActions { get; private set; }
    public int TimesFailed { get; set; }
    public bool Dormant {get;set;}

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
        return successCondition.IsSatisfied(owner, targetCharacter);
    }
}
