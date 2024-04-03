using System;
using UnityEngine;

[Serializable]
public class LightCicle 
{
    [SerializeField]
    string name;

    [SerializeField]
    Color color;

    [SerializeField]
    float time;

    public float getTime() 
    { 
        return time;
    }

    public Color getColor()
    {
        return color;
    }
}
