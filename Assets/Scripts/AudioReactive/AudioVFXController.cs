using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Controls VFX speed and color based on audio band intensity
/// </summary>
public class AudioVFXController : MonoBehaviour
{
    [SerializeField] private int _band;
    [SerializeField] private float _minRate, _maxRate;
    [SerializeField] private float _colorThreshold = 0.5f;
    
    [SerializeField] private VisualEffect _vFX;
    

    void Update()
    {
        float _audioAmplitudeBuffer = AudioSpectrumReader.amplitudeBuffer; // overall amplitude
        float _audioBandIntensityBuffer = AudioSpectrumReader.audioBandIntensityBuffer[_band]; // individual band
        
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
