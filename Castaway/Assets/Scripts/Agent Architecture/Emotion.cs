using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Globalization;
using UnityEngine;

/// <summary>
/// Class to hold data about individual emotions
/// </summary>

[System.Serializable]
public class Emotion
{
    [SerializeField] private EmotionRef name;
    public EmotionRef Name
    {
        get
        {
            return name;
        }
    }

    [SerializeField] [Range(0.0f, 1.0f)] private float threshold;
    public float Threshold
    {
        get
        {
            return threshold;
        }
    }

   [SerializeField] [Range(0.0f, 1.0f)] private float decay;
    public float Decay
    {
        get
        {
            return decay;
        }
    }

    [SerializeField] [Range(-1.0f, 1.0f)] private float emotionValue;
    public float EmotionValue
    {
        get
        {
            return emotionValue;
        }
        set
        {
            emotionValue = value;
        }
    }
}

/*
public enum EmotionRef
{
    Joy = 100,
    Distress = 101,
    Hope= 102,
    Fear = 103,
    Satisfaction = 104,
    FearsConfirmed = 105,
    Relief = 106,
    Disappointment = 107,
    Pride = 108,
    Shame = 109,
    Gratification = 110,
    Remorse = 111,
    HappyFor = 200,
    Pity = 201,
    Gloating = 202,
    Resentment = 203,
    Admiration = 204,
    Reproach = 205,
    Gratitude = 206,
    Anger = 207,
    Love = 208,
    Hate = 209
}
*/

public enum EmotionRef
{
    None = 0,
    Joy = 1,
    Distress = 2,
    Fear = 3,
    Satisfaction = 4,
    Disappointment = 5,
}
