using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class InteractionManager : Singleton<InteractionManager>
{
    public Camera MainCamera { get => _mainCamera; }
    
    public enum InteractionState
    {
        Inactive = 0,
        Active = 1,
        Spawning = 2
    }

    private InteractionState _interactionState;

    public InteractionState CurrentInteractionState
    {
        get => _interactionState;
        set => SetInteractionState(value); 
    }

    public UnityEvent onInteractionStateChanged;
    
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _debugConsole;
    [SerializeField] private OVRHand _leftHand;

    private GameObject _console;
    private CanvasGroup _consoleCanvas;
    
    
    void Start()
    {
        SetInteractionState(InteractionState.Inactive);
        _console = Instantiate(_debugConsole, _leftHand.transform);
        _consoleCanvas = _console.GetComponentInChildren<CanvasGroup>();
        _consoleCanvas.alpha = 0;
    }

    private void SetInteractionState(InteractionState state)
    {
        _interactionState = state;
        onInteractionStateChanged.Invoke();
    }

    public void SetStateByIndex(int index)
    {
        SetInteractionState((InteractionState)index);
    }
    
    void Update()
    {
        if (_leftHand.IsSystemGestureInProgress)
        {
            _consoleCanvas.alpha = 1;

            _console.transform.rotation = _leftHand.transform.rotation;
            return;
            
            /*
            _console.transform.up = Vector3.up;
            _console.transform.rotation = Quaternion.Euler(
                _console.transform.rotation.x, 
                _leftHand.transform.rotation.y, 
                _console.transform.rotation.z);
                */
            
        }
        else
        {
            _consoleCanvas.alpha = 0;
        }
        

        // if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.Hands)) {
        //     Debug.LogWarning("Hand-Tracking Start Button DOWN");
        //     GameObject console = Instantiate(_debugConsole, _leftHand.transform);
        // }
        //
        // if (OVRInput.GetUp(OVRInput.Button.Start, OVRInput.Controller.Hands)) {
        //     Debug.LogWarning("Hand-Tracking Start Button UP");
        // }
    }
}
