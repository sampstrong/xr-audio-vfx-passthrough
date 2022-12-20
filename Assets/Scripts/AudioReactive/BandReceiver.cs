using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BandReceiver : MonoBehaviour
{
    [SerializeField] protected int _band;
    
    protected virtual void Start()
    {
        BandTrigger.Instance.onBandTriggered[_band].AddListener(OnBandTriggered);
        BandTrigger.Instance.onBandTriggered[_band].AddListener(OnBandReleased);
    }

    protected abstract void OnBandTriggered();

    protected abstract void OnBandReleased();
}
