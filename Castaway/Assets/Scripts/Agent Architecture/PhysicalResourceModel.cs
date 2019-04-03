using System;
using System.Collections.Generic;
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
        return Findphysical(resourceName).PhysicalValue;
    }

    public float GetPhysicalValue(string resourceName)
    {
        return Findphysical(resourceName).PhysicalValue;
    }

    public float GetphysicalThreshold(PhysicalRef resourceName)
    {
        return Findphysical(resourceName).Threshold;
    }

    public float GetphysicalThreshold(string resourceName)
    {
        return Findphysical(resourceName).Threshold;
    }

    public float GetphysicalDecay(PhysicalRef resourceName)
    {
        return Findphysical(resourceName).Decay;
    }

    public float GetphysicalDecay(string resourceName)
    {
        return Findphysical(resourceName).Decay;
    }

    public void AddToValue(PhysicalRef resourceName, float amount)
    {
        Findphysical(resourceName).PhysicalValue += amount;
    }

    public void AddToValue(string resourceName, float amount)
    {
        Findphysical(resourceName).PhysicalValue += amount;
    }

    private PhysicalResource Findphysical(PhysicalRef resourceName)
    {
        foreach (PhysicalResource p in Resources)
        {
            if (p.Name == resourceName)
                return p;
        }
        return null;
    }

    private PhysicalResource Findphysical(string resourceName)
    {
        foreach (PhysicalResource p in Resources)
        {
            if (p.Name.ToString() == resourceName)
                return p;
        }
        return null;
    }
    
}
