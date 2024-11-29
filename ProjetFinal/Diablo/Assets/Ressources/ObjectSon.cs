using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSon : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 5f; 

    private Transform _target;
    public event Action _OnDestroyed;

    [SerializeField] private float _speed = 5f; 
    [SerializeField] private float _minDistance = 2f; 
    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    void Update()
    {
        if (_target != null)
        {
            
            float distanceToTarget = Vector3.Distance(transform.position, _target.position);

            if (distanceToTarget > _minDistance)
            {
                
                Vector3 direction = (_target.position - transform.position).normalized;
                transform.position += direction * _speed * Time.deltaTime;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (_target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_target.position, _minDistance); 
        }
    }

   private void OnDestroy()
   {
        _OnDestroyed?.Invoke();
   }
}


