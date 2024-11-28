using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generateur : MonoBehaviour
{
    [SerializeField] private GameObject _prefabToSpawn; 
    [SerializeField] private Transform _targetPlayer; 
    [SerializeField] private int _maxInstances = 50; 
    [SerializeField] private float _spawnInterval = 5f; 
    [SerializeField] private Vector3 _spawnAreaMin = new Vector3(-10, 2.3f, 0); 
    [SerializeField] private Vector3 _spawnAreaMax = new Vector3(10, 2.3f, 0); 

    private int _currentInstances = 0;

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (_currentInstances < _maxInstances)
            {
                SpawnObject();
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnObject()
    {
        
        Vector3 randomPosition = new Vector3(
            Random.Range(-_spawnAreaMin.x, _spawnAreaMax.x),
            Random.Range(-_spawnAreaMin.y, _spawnAreaMax.y),
            Random.Range(-_spawnAreaMin.z, _spawnAreaMax.z)
        );

        
        GameObject spawnedObject = Instantiate(_prefabToSpawn, randomPosition, Quaternion.identity);


        ObjectSon follower = spawnedObject.GetComponent<ObjectSon>();
        if (follower != null)
        {
            follower.SetTarget(_targetPlayer);
        }

        _currentInstances++;
        spawnedObject.GetComponent<ObjectSon>()._OnDestroyed += HandleObjectDestroyed;
    }

    private void HandleObjectDestroyed()
    {
        _currentInstances--;
    }
}

