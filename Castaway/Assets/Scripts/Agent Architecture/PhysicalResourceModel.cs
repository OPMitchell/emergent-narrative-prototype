using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// Class used by each Character object to represent a physical profile of that character.
/// Unique physical profiles are created by reading in each character's stats.
/// </summary>

public class PhysicalResourceModel : MonoBehaviour 
{
    //Each character has individual physical resources. Hold these in an array.
    [SerializeField] private PhysicalResource[] Resources;

    public float GetPhysicalValue(PhysicalRef resourceName)
    {
        return FindPhysical(resourceName).PhysicalValue;
    }

    public float GetPhysicalValue(string resourceName)
    {
        return FindPhysical(resourceName).PhysicalValue;
    }

    public float GetPhysicalThreshold(PhysicalRef resourceName)
    {
        return FindPhysical(resourceName).Threshold;
    }

    public float GetPhysicalThreshold(string resourceName)
    {
        return FindPhysical(resourceName).Threshold;
    }

    public float GetPhysicalDecay(PhysicalRef resourceName)
    {
        return FindPhysical(resourceName).Decay;
    }

    public float GetPhysicalDecay(string resourceName)
    {
        return FindPhysical(resourceName).Decay;
    }

    public void AddToValue(PhysicalRef resourceName, float amount)
    {
        PhysicalResource pr = FindPhysical(resourceName);
        if(pr.PhysicalValue + (pr.Threshold * amount) > 1.0f)
            pr.PhysicalValue = 1.0f;
        else
            pr.PhysicalValue += pr.Threshold * amount;
    }

    public void AddToValue(string resourceName, float amount)
    {
        PhysicalResource pr = FindPhysical(resourceName);
        if(pr.PhysicalValue + (pr.Threshold * amount) > 1.0f)
            pr.PhysicalValue = 1.0f;
        else
            pr.PhysicalValue += pr.Threshold * amount;
    }

    private PhysicalResource FindPhysical(PhysicalRef resourceName)
    {
        foreach (PhysicalResource p in Resources)
        {
            if (p.Name == resourceName)
                return p;
        }
        return null;
    }

    private PhysicalResource FindPhysical(string resourceName)
    {
        foreach (PhysicalResource p in Resources)
        {
            if (p.Name.ToString() == resourceName)
                return p;
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
                if((int)stat >= Precondition.physicalIndex && (int)stat < Precondition.limitIndex)
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
                if((int)stat >= Precondition.physicalIndex && (int)stat < Precondition.limitIndex)
                    AddToValue(stat.ToString(), change);
            }
        }
    }

    void Start()
    {
        StartCoroutine(Decay());
    }

    IEnumerator Decay()
    {
        while(true)
        {
            foreach(PhysicalResource pr in Resources)
            {
                if(pr.PhysicalValue + pr.Decay > 1.0f)
                    pr.PhysicalValue = 1.0f;
                else
                    pr.PhysicalValue += pr.Decay;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public PhysicalResource[] GetPhysicalResources()
    {
        return Resources;
    }
    
}
