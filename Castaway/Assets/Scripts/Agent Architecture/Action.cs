using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Globalization;
using UnityEngine;

public enum Status
{
    notSent = 1,
    Sent = 2,
    Failed = 3,
    Successful = 4,
    Interrupted = 5,
};

public class Action
{
    [XmlAttribute("name")]
    public string Name { get; set; }
    [XmlAttribute("type")]
    public string Type { get; set; }
    [XmlAttribute("sender")]
    public string Sender { get; set; }
    [XmlAttribute("target")]
    public string Target { get; set; }
    [XmlIgnore]
    public GameObject TargetGameObject {get;set;}
    [XmlAttribute("dialogid")]
    public string DialogID { get; set; }
    [XmlAttribute("precondition")]
    public string Precondition { get; set; }
    [XmlAttribute("effect")]
    public string Effect { get; set; }
    [XmlAttribute("parameters")]
    public string Parameters { get; set; }
    [XmlAttribute("priority")]
    public int Priority { get; set; }
    public Status Status{ get; private set;}

    public Action()
    {
        SetStatus(Status.notSent);
    }

    public Action(Action action)
    {
        Replace(action);
        Status = Status.notSent;
    }

    public Action(string name, string type, string sender, string target, string dialogid, string precondition, string effect, string parameters, int priority)
    {
        Name = name;
        Type = type;
        Sender = sender;
        Target = target;
        TargetGameObject = GameObject.Find(Target);
        DialogID = dialogid;
        Precondition = precondition;
        Effect = effect;
        Parameters = parameters;
        Priority = priority;
        Status = Status.notSent;
    }

    public void Replace(Action newAction)
    {
        this.Name = newAction.Name;
        this.Type = newAction.Type;
        this.Sender = newAction.Sender;
        this.Target = newAction.Target;
        this.DialogID = newAction.DialogID;
        this.Precondition = newAction.Precondition;
        this.Effect = newAction.Effect;
        this.Parameters = newAction.Parameters;
        this.Priority = newAction.Priority;
    }

    public bool Compare(Action a)
    {
       if(this.Name == a.Name
            && this.Type == a.Type
            && this.Sender == a.Sender
            && this.Target == a.Target
            && this.DialogID == a.DialogID
            && this.Precondition == a.Precondition
            && this.Effect == a.Effect
            && this.Parameters == a.Parameters
            && this.Priority == a.Priority
            )
                return true;
            return false;
    }

    public void SetStatus(Status s)
    {
        Status = s;
    }

    public bool HasPrecondition()
    {
        return (Precondition != "");
    }
}
