using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Globalization;
using UnityEngine;

/// <summary>
/// Class to hold data about individual physical resources
/// </summary>

[System.Serializable]
public class PhysicalResource
{
    [SerializeField] private PhysicalRef name;
    public PhysicalRef Name
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

    [SerializeField] [Range(0.0f, 1.0f)] private float physicalValue;
    public float PhysicalValue
    {
        get
        {
            return physicalValue;
        }
        set
        {
            physicalValue = value;
        }
    }
}

public enum PhysicalRef
{
    None = 0,
    Hunger = 1,
    Tiredness = 2,
    Health = 3,
}
