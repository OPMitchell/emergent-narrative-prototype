using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// Class used by each Character object to represent an emotional profile of that character.
/// Unique emotional profiles are created by reading in each character's stats.
/// </summary>

public class EmotionalPersonalityModel : MonoBehaviour 
{
    //Each character has individual emotions. Hold these in an array.
    [SerializeField] private Emotion[] Emotions;

    public float GetEmotionValue(EmotionRef emotionName)
    {
        return FindEmotion(emotionName).EmotionValue;
    }

    public float GetEmotionValue(string emotionName)
    {
        return FindEmotion(emotionName).EmotionValue;
    }

    public float GetEmotionThreshold(EmotionRef emotionName)
    {
        return FindEmotion(emotionName).Threshold;
    }

    public float GetEmotionThreshold(string emotionName)
    {
        return FindEmotion(emotionName).Threshold;
    }

    public float GetEmotionDecay(EmotionRef emotionName)
    {
        return FindEmotion(emotionName).Decay;
    }

    public float GetEmotionDecay(string emotionName)
    {
        return FindEmotion(emotionName).Decay;
    }

    public void AddToValue(EmotionRef emotionName, float amount)
    {
        FindEmotion(emotionName).EmotionValue += amount;
    }

    public void AddToValue(string emotionName, float amount)
    {
        FindEmotion(emotionName).EmotionValue += amount;
    }

    private Emotion FindEmotion(EmotionRef emotion)
    {
        foreach (Emotion e in Emotions)
        {
            if (e.Name == emotion)
                return e;
        }
        return null;
    }

    private Emotion FindEmotion(string emotion)
    {
        foreach (Emotion e in Emotions)
        {
            if (e.Name.ToString() == emotion)
                return e;
        }
        return null;
    }
    
}
