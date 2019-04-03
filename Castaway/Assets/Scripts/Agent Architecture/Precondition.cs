using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using UnityEngine;

public enum StatName
{
    None = 0,
    //emotional
    Joy = 1,
    Distress = 2,
    Fear = 3,
    Satisfaction = 4,
    Disappointment = 5,
    //physical
    Hunger = 6,
    Tiredness = 7,
    Health = 8,
}

[System.Serializable]
public class Precondition
{
    public const int emotionalIndex = 1;
    public const int physicalIndex = 6;
    public const int limitIndex = 9;
	[SerializeField] private GameObject holdingItem;
    public GameObject HoldingItem
    {
        get 
        {
            return holdingItem;
        }
    }
	[SerializeField] private StatName stat;
    public StatName Stat
    {
        get
        {
            return stat;
        }
    }
	[SerializeField] private BooleanCondition boolCondition;
    public BooleanCondition BoolCondition
    {
        get
        {
            return boolCondition;
        }
    }
	[Range(-1.0f,1.0f)][SerializeField] private float value;
    public float Value
    {
        get
        {
            return value;
        }
    }

    public bool IsSatisfied(GameObject sender, GameObject target)
    {
        bool holdingItem = IsHoldingItem(sender);
        bool statCondition = IsStatCorrect(target);
        return (holdingItem && statCondition);
    }

    private bool IsHoldingItem(GameObject sender)
    {
        if(HoldingItem != null)
        {
            Character c = GameObject.Find(sender.name).GetComponent<Character>();
            if(c.heldItem != null)
            {
                Item heldItem = c.heldItem.GetComponent<Item>();
                Item targetItem = HoldingItem.GetComponent<Item>();
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

    private bool IsStatCorrect(GameObject target)
    {
        if(Stat == StatName.None)
			return true;
        GameObject targetCharacter = GameObject.Find(target.name);
        if(targetCharacter.tag == "Character")
        {
            float value = 0;
            if((int)Stat > 0 && (int)Stat < physicalIndex)
            {
                EmotionalPersonalityModel epm = targetCharacter.GetComponent<EmotionalPersonalityModel>();
                value = (float)epm.GetEmotionValue(Stat.ToString());
            }
            else if((int)Stat >= physicalIndex && (int)Stat < limitIndex)
            {
                PhysicalResourceModel prm = targetCharacter.GetComponent<PhysicalResourceModel>();
                value = (float)prm.GetPhysicalValue(Stat.ToString());
            }
            if(BoolCondition == BooleanCondition.LessThan)
            {
                if(value < Value)
                    return true;
                else
                    return false;
            }
            else if(BoolCondition == BooleanCondition.LessThanOrEqualTo)
            {
                if(value <= Value)
                    return true;
                else
                    return false;
            }
            else if(BoolCondition == BooleanCondition.EqualTo)
            {
                if(value == Value)
                    return true;
                else
                    return false;
            }
            else if(BoolCondition == BooleanCondition.GreaterThanOrEqualTo)
            {
                if(value >= Value)
                    return true;
                else
                    return false;
            }
            else if(BoolCondition == BooleanCondition.GreaterThan)
            {
                if(value > Value)
                    return true;
                else
                    return false;
            }
        }
        return false;
    }
}