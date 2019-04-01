using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Precondition
{
	[SerializeField] private GameObject holdingItem;
    public GameObject HoldingItem
    {
        get 
        {
            return holdingItem;
        }
    }
	[SerializeField] private EmotionRef emotion;
    public EmotionRef Emotion
    {
        get
        {
            return emotion;
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
        bool emotionCondition = IsEmotionCorrect(target);
        return (holdingItem && emotionCondition);
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

    private bool IsEmotionCorrect(GameObject target)
    {
        if(Emotion == EmotionRef.None)
			return true;
        GameObject targetCharacter = GameObject.Find(target.name);
        if(targetCharacter.tag == "Character")
        {
            EmotionalPersonalityModel epm = targetCharacter.GetComponent<EmotionalPersonalityModel>();
            float value = (float)epm.GetEmotionValue(Emotion);

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