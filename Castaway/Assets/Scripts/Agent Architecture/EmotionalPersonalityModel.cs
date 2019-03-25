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

    [SerializeField] private TextAsset EmotionalPersonalityFile;
    public List<PersonalEmotion> Emotions { get; private set; }

    void Start()
    {
        this.Emotions = ConfigReader.ReadEmotionData(EmotionalPersonalityFile.name + ".xml");
    }

    public object GetEmotionValue(EmotionRef emotionName)
    {
        return FindEmotion(emotionName).GetValue();
    }

    public int GetEmotionThreshold(EmotionRef emotionName)
    {
        return FindEmotion(emotionName).Threshold;
    }

    public int GetEmotionDecay(EmotionRef emotionName)
    {
        return FindEmotion(emotionName).Decay;
    }

    private Emotion FindEmotion(EmotionRef emotion)
    {
        foreach (Emotion e in Emotions)
        {
            if (e.Name == emotion.ToString())
                return e;
        }
        return null;
    }
    
}
