using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// Class used by each Character object to represent an emotional profile of that character.
/// Unique emotional profiles are created by reading in each character's named .xml file.
/// </summary>

public class EmotionalPersonalityModel : MonoBehaviour 
{
    //Each character has individual emotions. Hold these in an array.
    [SerializeField] private Emotion[] Emotions;

    public float GetEmotionValue(EmotionRef emotionName)
    {
        return FindEmotion(emotionName).EmotionValue;
    }

    public float GetEmotionThreshold(EmotionRef emotionName)
    {
        return FindEmotion(emotionName).Threshold;
    }

    public float GetEmotionDecay(EmotionRef emotionName)
    {
        return FindEmotion(emotionName).Decay;
    }

    public void AddToValue(EmotionRef emotionName, float amount)
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
    
}
