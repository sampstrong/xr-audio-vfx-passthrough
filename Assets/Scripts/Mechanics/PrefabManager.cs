using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Use class to spawn a prefab at a predefined position
/// </summary>
public class PrefabManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private GameObject _positionReference;

    private bool _isInstantiated;

    /// <summary>
    /// Spawns prefab at position defined in the inspector. Public access allows for gesture based spawning
    /// while _isInstantiated bool ensures the prefab is only instantiated once.
    /// </summary>
    [Button]
    public void SpawnPrefab()
    {
        Debug.Log($"Prefab Spawned at position: {_positionReference.transform.position}");
        if (_isInstantiated) return;
        Instantiate(_prefabToSpawn, _positionReference.transform.position, Quaternion.identity);
        _isInstantiated = true;
    }
    
    public void Reset()
    {
        _isInstantiated = false;
    }
}
