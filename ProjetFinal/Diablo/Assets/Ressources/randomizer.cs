using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomizer : MonoBehaviour
{
    [SerializeField] private GameObject _interactiveElement;
    [SerializeField] private GameObject _meleeEnemyGenerator;
    [SerializeField] private GameObject _rangeEnemeyGenerator;
    private List<Vector3> _allPosition = new List<Vector3>();
    [SerializeField] private Vector3 _interactifOffset = new Vector3(0f,2f,0f);
    void Start()
    {
        foreach (Transform child in transform)
        {
            _allPosition.Add(child.position);
        }
        int index = Random.Range(0, _allPosition.Count);
        Vector3 randomPos = _allPosition[index];
        Instantiate(_interactiveElement, randomPos+ _interactifOffset, Quaternion.identity, transform);
        _allPosition.RemoveAt(index);

        index = Random.Range(0, _allPosition.Count);
        randomPos = _allPosition[index];
        Instantiate(_rangeEnemeyGenerator, randomPos, Quaternion.identity, transform);
        _allPosition.RemoveAt(index);

        foreach (Vector3 position in _allPosition)
        {

            Instantiate(_meleeEnemyGenerator, position, Quaternion.identity, transform);
        }
    }


    void Update()
    {

    }
}
