using System.Collections;
using System.Collections.Generic;
using Pixelplacement.XRTools;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        // RoomMapper.Instance.OnRoomMapped += StartAudio;
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
