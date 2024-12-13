using UnityEngine;

public class feu : MonoBehaviour
{
    private Transform _target; 
    private int _damage; 

    [SerializeField] private float _speed = 10f;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    void Update()
    {
        if (_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _target.position) <= 0.1f)
            {
                DestroyProjectile();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _target)
        {
            PlayerHealthAndDefense health = other.GetComponent<PlayerHealthAndDefense>();
            if (health != null)
            {
                health.ReceiveDamage(_damage); 
                Destroy(gameObject); 
                Debug.Log("Projectile hit the hero and dealt damage!");
            }
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject); 
    }
}
