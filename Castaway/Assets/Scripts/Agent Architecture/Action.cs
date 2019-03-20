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
    public GameObject TargetGameObject {get;set;}
    [XmlAttribute("priority")]
    public int Priority { get; set; }
    public Status Status{ get; set; }

    public Action()
    {
        Status = Status.notSent;
    }

    public Action(string name, string type, string sender, string target, int priority)
    {
        Name = name;
        Type = type;
        Sender = sender;
        Target = target;
        TargetGameObject = GameObject.Find(Target);
        Priority = priority;
        Status = Status.notSent;
    }

    public bool Compare(Action action)
    {
        if( Name == action.Name &&
            Type == action.Type &&
            Sender == action.Sender &&
            Target == action.Target &&
            Priority == action.Priority)
            return true;
        return false;
    }
}
