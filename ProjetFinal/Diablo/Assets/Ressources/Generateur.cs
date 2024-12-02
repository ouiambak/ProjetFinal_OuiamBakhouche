using System.Collections;
using UnityEngine;

public class Generateur : MonoBehaviour
{
    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private int _maxInstances = 20;
    [SerializeField] private float _spawnDelay = 1f; 

    private int _currentInstances = 0;
    private bool _isSpawning = false;

    void Update()
    {
        if (!_isSpawning && _currentInstances < _maxInstances)
        {
            StartCoroutine(SpawnObjectWithDelay());
        }
    }

    private IEnumerator SpawnObjectWithDelay()
    {
        _isSpawning = true; 
        while (_currentInstances < _maxInstances)
        {
            SpawnObject();
            yield return new WaitForSeconds(_spawnDelay); 
        }
        _isSpawning = false; 
    }

    private void SpawnObject()
    {
        GameObject spawnedObject = Instantiate(_prefabToSpawn, transform.position, Quaternion.identity);
        _currentInstances++;
        if (spawnedObject.TryGetComponent<ObjectSon>(out ObjectSon objectSon))
        {
            objectSon._OnDestroyed += HandleObjectDestroyed;
        }
    }

    private void HandleObjectDestroyed()
    {
        _currentInstances--;
    }
}
