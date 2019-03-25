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

public class Goal
{
    [XmlAttribute("type")]
    public GoalType Type { get; private set; }
    [XmlAttribute("target")]
    public string Target { get; private set; }
    [XmlAttribute("parameters")]
    public string SuccessCondition {get; private set;}
    public bool Complete {get; set;}

    [XmlIgnore]
    public Plan Plan {get; private set;}
    public List<Action> FailedActions { get; private set; }
    public int TimesFailed { get; set; }

    public Goal()
    {
        FailedActions = new List<Action>();
        Complete = false;
    }

    public Goal(GoalType type, string target, string parameters)
    {
        FailedActions = new List<Action>();
        Type = type;
        Target = target;
        SuccessCondition = parameters;
        Complete = false;
    }

    public void SetPlan(Plan p)
    {
        Plan = p;
    }

    public void AddFailedAction(Action action)
    {
        if(!FailedActions.Any(x => x.Compare(action)))
        {
            FailedActions.Add(new Action(action));
        }
    }

    public bool IsSatisfied()
    {
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        string[] successConditionSplit = manager.SplitParameterString(SuccessCondition);
        float targetValue = float.Parse(successConditionSplit[2], System.Globalization.CultureInfo.InvariantCulture);
        if(successConditionSplit[1] == "gt")
        {
            if(manager.GetStatValue(Target, successConditionSplit[0]) > targetValue)
                return true;
        }
        else if(successConditionSplit[1] == "lt")
        {
            if(manager.GetStatValue(Target, successConditionSplit[0]) < targetValue)
                return true;
        }
        return false;
    }
}
