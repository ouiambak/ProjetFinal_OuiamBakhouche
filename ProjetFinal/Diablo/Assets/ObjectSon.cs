using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSon : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 5f; 

    private Transform _target;
    public event Action _OnDestroyed;

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    void Update()
    {
        if (_target == null) return;

        
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _followSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        _OnDestroyed?.Invoke();
    }
}


