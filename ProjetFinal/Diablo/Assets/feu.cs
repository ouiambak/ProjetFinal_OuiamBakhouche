using UnityEngine;

public class feu: MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 5f;
    private int _damage; 
    private Transform _target;
    private Vector3 _targetPosition;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    private void Update()
    {
        if (_target != null)
        {
            _targetPosition = _target.position;
        }
        Vector3 direction = (_targetPosition - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    public void SetTarget(Transform target)
    {
        _target = target;
        _targetPosition = target.position;
    }
    public void SetDamage(int damage)
    {
        _damage = damage; 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthAndDefense playerHealth = other.GetComponent<HealthAndDefense>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
