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
}