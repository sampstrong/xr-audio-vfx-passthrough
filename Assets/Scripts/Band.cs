using UnityEngine;
using UnityEngine.Events;

public class Band
{
    public float Intensity;
    public bool Triggered;

    public Band(float intensity, bool triggered)
    {
        Intensity = intensity;
        Triggered = triggered;
    }
}
