using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;

/// <summary>
/// Class handles the instantiation of appropriate VFX to give visual
/// feedback when a hand pose is detected.
/// </summary>
public class PoseVisual : MonoBehaviour
{
    // array of poses
    [SerializeField] private ActiveStateSelector[] _poses;
    
    // array of visuals corresponding to each pose by index
    [SerializeField] private GameObject[] _poseVisualPrefabs;

    private GameObject _currentVisual;
    
    private void Start()
    {
        for (int i = 0; i < _poses.Length; i++)
        {
            int poseNumber = i;
            _poses[i].WhenSelected += () => ShowVisuals(poseNumber);
            _poses[i].WhenUnselected += () => HideVisuals(poseNumber);
        }
    }

    /// <summary>
    /// Instantiates a pose visual based on the pose index of the recognized pose
    /// </summary>
    /// <param name="poseNumber"></param>
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

    /// <summary>
    /// Hides visuals when pose is no longer recognized
    /// </summary>
    /// <param name="poseNumber"></param>
    private void HideVisuals(int poseNumber)
    {
        _currentVisual.gameObject.SetActive(false);
    }
}
