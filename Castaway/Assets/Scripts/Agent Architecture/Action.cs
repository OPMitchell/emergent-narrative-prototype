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
    Resend = 6,
    ResendSent = 7,
};

public enum BooleanCondition
{
	LessThan = 1,
    LessThanOrEqualTo = 2,
	EqualTo = 3,
    GreaterThanOrEqualTo = 4,
	GreaterThan = 5,
};

public enum ActionType
{
    WalkToTarget = 1,
    TalkToTarget = 2,
    GiveItemToTarget = 3,
    PickUpItem = 4,
    Cut = 5,
    Haul = 6,
    Build = 7,
    Eat = 8,
};

[System.Serializable]
public class Action
{
	[System.Serializable]
	public class SenderActionEffect
	{
		[SerializeField] private StatName stat;
        public StatName Stat
        {
            get
            {
                return stat;
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
    [System.Serializable]
	public class TargetActionEffect
	{
		[SerializeField] private StatName stat;
        public StatName Stat
        {
            get
            {
                return stat;
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
	[SerializeField] private ActionType type;
    public ActionType Type
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
    [SerializeField] private string dialog;
    public string Dialog
    {
        get
        {
            return dialog;
        }
        private set
        {
            dialog = value;
        }
    }
    [Range(1,10)][SerializeField] private int priority;
	public int Priority
    {
        get
        {
            return priority;
        }
        set
        {
            priority = value;
        }
    }
    [SerializeField] private bool negativeAction;
    public bool NegativeAction
    {
        get
        {
            return negativeAction;
        }
        private set
        {
            negativeAction = value;
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
    [SerializeField] private SenderActionEffect senderEffect;
    public SenderActionEffect SenderEffect
    {
        get
        {
            return senderEffect;
        }
        private set
        {
            senderEffect = value;
        }
    }
    [SerializeField] private TargetActionEffect targetEffect;
    public TargetActionEffect TargetEffect
    {
        get
        {
            return targetEffect;
        }
        private set
        {
            targetEffect = value;
        }
    }
	public Status Status {get; private set;}

    public Action()
    {
        SetStatus(Status.notSent);
    }

    public Action(Action action)
    {
        Name = action.name;
        Type = action.type;
        SendingCharacter = action.sendingCharacter;
        TargetObject = action.targetObject;
        SenderEffect = action.senderEffect;
        TargetEffect = action.targetEffect;
        Priority = action.priority;
        precondition = action.precondition;
        NegativeAction = action.negativeAction;
        Dialog = action.dialog;
        Status = Status.notSent;
    }

    public Action(string name, ActionType type, GameObject sender, GameObject target, int priority)
    {
        Name = name;
        Type = type;
        SendingCharacter = sender;
        TargetObject = target;
        Priority = priority;
        SetStatus(Status.notSent);
    }

    public bool Compare(Action action)
    {
        if(
            Name == action.Name &&
            Type == action.Type &&
            SendingCharacter == action.SendingCharacter &&
            TargetObject == action.TargetObject &&
            Dialog == action.Dialog
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
        if(Precondition.HoldingItem == null && precondition.Stat == StatName.None)
            return false;
        return true;
    }

    public bool IsPreconditionSatisfied()
    {
        if(precondition == null)
            return true;
        else
        {
            return precondition.IsSatisfied(sendingCharacter, targetObject);
        }
    }
}
