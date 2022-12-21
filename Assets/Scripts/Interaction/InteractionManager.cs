using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Class handles global interaction state and interactions for debugging
/// </summary>
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
        ToggleDebugOnSystemGesture();
    }

    /// <summary>
    /// Toggles debug log on when left palm faces HMD
    /// </summary>
    private void ToggleDebugOnSystemGesture()
    {
        if (_leftHand.IsSystemGestureInProgress)
        {
            _consoleCanvas.alpha = 1;
            _console.transform.rotation = _leftHand.transform.rotation;
        }
        else
        {
            _consoleCanvas.alpha = 0;
        }
    }
}
