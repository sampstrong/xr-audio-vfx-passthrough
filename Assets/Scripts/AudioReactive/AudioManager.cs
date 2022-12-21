using UnityEngine;

/// <summary>
/// Class handles audio start and stop based on interaction manager events
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    
    void Start()
    {
        InteractionManager.Instance.onInteractionStateChanged.AddListener(StartAudio);
    }

    private void StartAudio()
    {
        if (InteractionManager.Instance.CurrentInteractionState != InteractionManager.InteractionState.Active)
        {
            _audioSource.Pause();
        }
        else
        {
            if (_audioSource.isPlaying) return;
            _audioSource.Play();    
        }
    }
}
