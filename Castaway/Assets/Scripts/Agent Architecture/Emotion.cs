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

    [SerializeField] [Range(0.0f, 2.0f)] private float threshold;
    public float Threshold
    {
        get
        {
            return threshold;
        }
    }

   [SerializeField] [Range(0.0f, 2.0f)] private float decay;
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
