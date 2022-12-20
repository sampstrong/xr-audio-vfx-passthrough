using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;

public class PoseVisual : MonoBehaviour
{
    [SerializeField] private ActiveStateSelector[] _poses;
    [SerializeField] private GameObject[] _poseVisualPrefabs;

    private GameObject _currentVisual;
    
    private void Start()
    {
        //_poseVisual = Instantiate(_poseActiveVisualPrefab);
        //_poseVisual.SetActive(false);
        
        for (int i = 0; i < _poses.Length; i++)
        {
            int poseNumber = i;
            _poses[i].WhenSelected += () => ShowVisuals(poseNumber);
            _poses[i].WhenUnselected += () => HideVisuals(poseNumber);
        }
    }

    private void ShowVisuals(int poseNumber)
    {
        _currentVisual = Instantiate(_poseVisualPrefabs[poseNumber]);
        var hands = _poses[poseNumber].GetComponents<HandRef>();
        Vector3 visualsPos = Vector3.zero;
        Quaternion currentRotation = Quaternion.identity;
        foreach (var hand in hands)
        {
            hand.GetRootPose(out Pose wristPose);

            Vector3 forward;
            if (hand.Handedness == Handedness.Left)
            {
                forward = wristPose.right;
                currentRotation = wristPose.rotation;
            }
            else
            {
                forward = -wristPose.right;
                currentRotation = wristPose.rotation * Quaternion.Euler(0, 180, 0);
            }
            
            //Vector3 forward = hand.Handedness == Handedness.Left ? wristPose.right : -wristPose.right;
            visualsPos += wristPose.position + forward * .15f + Vector3.up * .02f;
        }
        _currentVisual.transform.position = visualsPos / hands.Length;
        _currentVisual.transform.rotation = currentRotation;
        _currentVisual.gameObject.SetActive(true);
    }

    private void HideVisuals(int poseNumber)
    {
        _currentVisual.gameObject.SetActive(false);
    }
}
