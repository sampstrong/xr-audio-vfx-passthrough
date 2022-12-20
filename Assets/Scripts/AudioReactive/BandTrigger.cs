using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BandTrigger : Singleton<BandTrigger>
{
    public Band[] Bands { get => _bands; }
    
    public UnityEvent[] onBandTriggered = new UnityEvent[8];
    public UnityEvent[] onBandReleased = new UnityEvent[8];
    
    private Band[] _bands = new Band[8];

    private void Start()
    {
        for (int i = 0; i < _bands.Length; i++)
        {
            _bands[i] = new Band(0, false);
        }
    }
    
    private void Update()
    {
        for (int i = 0; i < _bands.Length; i++)
        {
            _bands[i].Intensity = AudioSpectrumReader._audioBandIntensityBuffer[i];
        }
        
        GetBandTrigger();
    }

    private void GetBandTrigger()
    {
        for (int i = 0; i < _bands.Length; i++)
        {
            if (_bands[i].Intensity > 0.5f && !_bands[i].Triggered)
            {
                _bands[i].Triggered = true;
                onBandTriggered[i].Invoke();
            }
            else if (_bands[i].Intensity < 0.5 && _bands[i].Triggered)
            {
                _bands[i].Triggered = false;
                onBandReleased[i].Invoke();
            }
        }
    }

    /* Old - delete after testing new code
    private void GetBandTrigger()
    {
        for (int i = 0; i < _numberOfBands; i++)
        {
            if (_audioBandIntensity[i] > 0.5f && !_bandTriggered[i])
            {
                _bandTriggered[i] = true;
                onBandTriggered[i].Invoke();
            }
            else if (_audioBandIntensity[i] < 0.5f && _bandTriggered[i])
            {
                _bandTriggered[i] = false;
                onBandReleased[i].Invoke();
            }
        }
    }
    */
}


