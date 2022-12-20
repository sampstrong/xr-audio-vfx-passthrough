using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private GameObject _positionReference;

    private bool _isInstantiated;

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
