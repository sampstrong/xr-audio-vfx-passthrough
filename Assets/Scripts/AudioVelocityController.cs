using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioVelocityController : AudioController
{
    [SerializeField] private NodeGroup _nodeGroup;
    [SerializeField] private float _speedMin = 1f;
    [SerializeField] private float _speedMax = 1.001f;
    [SerializeField] private Material _matOne;
    [SerializeField] private Material _matTwo;

    private bool _aboveTriggerThreshold = false;
    private float _currentSpeed;

    private UnityEvent onKickTriggered;
    private UnityEvent onKickReleased;

    private int _beatCount = 0;

    private void Start()
    {
        //onKickTriggered.AddListener(IncreaseVelocity);
        //onKickReleased.AddListener(DecreaseVelocity);
        ChangeMaterial(_matOne);
    }
    
    protected override void Update()
    {
        base.Update();
        GetBeatTrigger();
    }
    
    
    // should rework/rename this class to BeatTrigger and just send events that other scripts can pick up
    // make a singleton for easy access across all scripts
    private void GetBeatTrigger()
    {
        if (_audioBandIntensityBuffer > 0.5f && !_aboveTriggerThreshold)
        {
            _aboveTriggerThreshold = true;
            //onKickTriggered.Invoke();
            IncreaseVelocity();
            ChangeMaterial(_matTwo);
            _beatCount++;
            if (_beatCount >= 4)
            {
                foreach (var node in _nodeGroup.Nodes)
                {
                    node.rb.velocity = _nodeGroup.SetVelocity();  
                }

                _beatCount = 0;
            }
        }
        else if (_audioBandIntensityBuffer < 0.5 && _aboveTriggerThreshold)
        {
            _aboveTriggerThreshold = false;
            //onKickReleased.Invoke();
            DecreaseVelocity();
            ChangeMaterial(_matOne);
        }
    }

    private void IncreaseVelocity()
    {
        if (_nodeGroup.Nodes.Count < _nodeGroup.NumberOfNodes) return;

        foreach (var node in _nodeGroup.Nodes)
        {
            _currentSpeed = SetSpeed(_speedMin, _speedMax);
            node.rb.velocity *= _currentSpeed;
        }
    }

    private void DecreaseVelocity()
    {
        if (_nodeGroup.Nodes.Count < _nodeGroup.NumberOfNodes) return;

        foreach (var node in _nodeGroup.Nodes)
        {
            node.rb.velocity /= _currentSpeed;
        }
    }

    private float SetSpeed(float min, float max)
    {
        float speed = GetControlValue(_audioBandIntensityBuffer, min, max);
        return speed;
    }

    private void ChangeMaterial(Material mat)
    {
        foreach (var node in _nodeGroup.Nodes)
        {
            node.rend.material = mat;
        }
    }
}
