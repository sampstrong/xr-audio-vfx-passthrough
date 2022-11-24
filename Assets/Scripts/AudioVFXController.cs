using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AudioVFXController : MonoBehaviour
{
    [SerializeField] private int _band;
    [SerializeField] private float _minRate, _maxRate;
    [SerializeField] private float _colorThreshold = 0.5f;
    
    [SerializeField] private VisualEffect _vFX;
    

    void Update()
    {
        float _audioAmplitudeBuffer = AudioSpectrumReader._AmplitudeBuffer; // overall amplitude
        float _audioBandIntensityBuffer = AudioSpectrumReader._audioBandIntensityBuffer[_band]; // individual band
        
        ControlVFXSpeed(_audioBandIntensityBuffer);
        ControlVFXColor(_audioBandIntensityBuffer);
    }

    private void ControlVFXSpeed(float intensity)
    {
        _vFX.playRate = (intensity * _maxRate) + _minRate;
    }

    private void ControlVFXColor(float intensity)
    {
        if (intensity > _colorThreshold)
        {
            _vFX.SetBool("ColorToggle", true);
        }
        else
        {
            _vFX.SetBool("ColorToggle", false);
        }
    }
}
