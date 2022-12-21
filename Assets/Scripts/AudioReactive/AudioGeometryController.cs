using UnityEngine;

/// <summary>
/// Inherits from AudioController. Used to control scale of objects based on audio input
/// </summary>
public class AudioGeometryController : AudioController
{
    [SerializeField] private float _minScale = 0.3f;
    [SerializeField] private float _maxScale = 1f;
    
    protected override void Update()
    {
        base.Update();
        ControlScale();
    }

    private void ControlScale()
    {
        transform.localScale = Vector3.one * GetControlValue(_audioBandIntensityBuffer, _minScale, _maxScale);
    }
    
}
