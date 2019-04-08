using System;
using System.Collections;
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
        Emotion e = FindEmotion(emotionName);
        if(e.EmotionValue + (e.Threshold * amount) > 1.0f)
            e.EmotionValue = 1.0f;
        else
            e.EmotionValue += e.Threshold * amount;
    }

    public void AddToValue(string emotionName, float amount)
    {
        Emotion e = FindEmotion(emotionName);
        if(e.EmotionValue + (e.Threshold * amount) > 1.0f)
            e.EmotionValue = 1.0f;
        else
            e.EmotionValue += e.Threshold * amount;
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

    public void AppraiseActionAsReceiver(Action action)
    {
        if(action.TargetEffect != null)
        {
            StatName stat = action.TargetEffect.Stat;
            float change = action.TargetEffect.Change;
            if(stat != StatName.None)
            {
                if((int)stat > 0 && (int)stat < Precondition.physicalIndex)
                    AddToValue(stat.ToString(), change);
            }
        }
    }

    public void AppraiseActionAsSender(Action action)
    {
        if(action.SenderEffect != null)
        {
            StatName stat = action.SenderEffect.Stat;
            float change = action.SenderEffect.Change;
            if(stat != StatName.None)
            {
                if((int)stat > 0 && (int)stat < Precondition.physicalIndex)
                    AddToValue(stat.ToString(), change);
            }
        }
    }

    public Emotion[] GetEmotions()
    {
        return Emotions;
    }

    void Start()
    {
        StartCoroutine(Decay());
    }

    IEnumerator Decay()
    {
        while(true)
        {
            foreach(Emotion e in Emotions)
            {
                if(e.EmotionValue - e.Decay < 0.0f)
                    e.EmotionValue = 0.0f;
                else
                    e.EmotionValue -= e.Decay;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
