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

public enum BooleanCondition
{
	LessThan = 0,
	EqualTo = 1,
	GreaterThan = 2
};

[System.Serializable]
public class Action
{
	[System.Serializable]
	public class ActionEffect
	{
		[SerializeField] private EmotionRef emotion;
        public EmotionRef Emotion
        {
            get
            {
                return emotion;
            }
        }
		[Range(-1.0f,1.0f)][SerializeField] private float change;
        public float Change
        {
            get
            {
                return change;
            }
        }
		[SerializeField] private GameObject pickedUpItem;
        public GameObject PickedUpItem
        {
            get
            {
                return pickedUpItem;
            }
        }
	}
	[SerializeField] private string name;
    public string Name
    {
        get
        {
            return name;
        }
        private set
        {
            name = value;
        }
    }
	[SerializeField] private string type;
    public string Type
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
	[SerializeField] private GameObject sendingCharacter;
    public GameObject SendingCharacter
    {
        get
        {
            return sendingCharacter;
        }
        private set
        {
            sendingCharacter = value;
        }
    }
	[SerializeField] private GameObject targetObject;
    public GameObject TargetObject
    {
        get
        {
            return targetObject;
        }
        private set
        {
            targetObject = value;
        }
    }
	[SerializeField] private ActionEffect effect;
    public ActionEffect Effect
    {
        get
        {
            return effect;
        }
        private set
        {
            effect = value;
        }
    }
	[SerializeField] private Precondition precondition;
    public Precondition Precondition
    {
        get
        {
            return precondition;
        }
        private set
        {
            precondition = value;
        }
    }
	public int Priority {get; private set;}
	public Status Status {get; private set;}

    public Action()
    {
        SetStatus(Status.notSent);
    }

    public Action(string name, string type, GameObject sender, GameObject target)
    {
        Name = name;
        Type = type;
        SendingCharacter = sender;
        TargetObject = target;
        SetStatus(Status.notSent);
    }

    public bool Compare(Action action)
    {
        if(
            Name == action.Name &&
            Type == action.Type &&
            SendingCharacter == action.SendingCharacter &&
            TargetObject == action.TargetObject
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
        if(Precondition.HoldingItem == null && precondition.Emotion == EmotionRef.None)
            return false;
        return true;
    }

    public bool IsPreconditionSatisfied()
    {
        return false;
    }
}
