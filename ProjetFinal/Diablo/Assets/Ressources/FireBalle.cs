using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBalle : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _explosionVFx;//pour activer le visual du explosion 
    [SerializeField] private float _yoffset = 1.5f;
    [SerializeField] private float _explosionDelay = 1.5f;
    private Transform _target;
    private Rigidbody _rigidbody;
    private bool _hasExploded;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_target == null) return; 

        Vector3 direction = ((_target.position + new Vector3(0, _yoffset, 0)) - transform.position).normalized;
        if (!_hasExploded)
        {
            _rigidbody.velocity = direction * _speed;
        }
    }

    public void SetTarget(Transform target)
    {
        if (target != null)
        {
            _target = target;
        }
        else
        {
            Debug.LogWarning("SetTarget called with a null target!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HealthAndDefense>() != null && !_hasExploded)
        {
            Explosion();
            Debug.Log("Boom!!"+ other.GetComponent<HealthAndDefense>().name);
        }
    }
    private void Explosion()
    {
        transform.localScale = Vector3.one * _radius * 2;
        _explosionVFx.SetActive(true);

        Collider[] hitCollider = Physics.OverlapSphere(transform.position, _radius);
        foreach (Collider c in hitCollider)
        {
            HealthAndDefense health = c.GetComponent<HealthAndDefense>();
            if (health != null)
            {
                health.ReceiveDamage(_damage);
            }
        }

        _hasExploded = true;
        _rigidbody.velocity = Vector3.zero;

        if (_explosionVFx != null)
        {
            Destroy(gameObject, _explosionDelay); // Détruire après le délai d'explosion
        }
        else
        {
            Debug.LogWarning("Explosion VFX is not assigned or destroyed.");
        }
    }

}
