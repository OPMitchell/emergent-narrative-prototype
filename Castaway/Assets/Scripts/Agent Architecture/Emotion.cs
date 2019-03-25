using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Globalization;
using UnityEngine;

/// <summary>
/// Class to hold data about individual emotions
/// </summary>
public abstract class Emotion
{
    [XmlAttribute("name")]
    public string Name { get; set; }
    [XmlAttribute("threshold")]
    public int Threshold { get; set; }
    [XmlAttribute("decay")]
    public int Decay { get; set; }
    public abstract object GetValue();
}

public abstract class GenericEmotion<T> : Emotion
{
    [XmlAttribute("value")]
    public T Value { get; set; }

    public override object GetValue()
    {
        return Value;
    }
}

public class PersonalEmotion : GenericEmotion<int> { }

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
    Joy = 1,
    Distress = 2,
    Fear = 3,
    Satisfaction = 4,
    Disappointment = 5,
}
