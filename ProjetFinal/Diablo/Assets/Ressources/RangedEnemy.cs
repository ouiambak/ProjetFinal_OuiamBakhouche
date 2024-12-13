using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _attackRange = 10f;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint; 
    [SerializeField] private float _projectileSpeed = 8f;
    [SerializeField] private int _damage = 10;

    private Transform _hero;
    private bool _isDead = false;
    private float _lastAttackTime = 0f;
    private Rigidbody _rigidBody;

    private void Start()
    {
        _hero = GameManager.PlayerTransform;
        if (_hero == null)
        {
            Debug.LogError("PlayerTransform not found in GameManager! Ensure it is assigned.");
            enabled = false;
            return;
        }

        if (_firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned in the inspector!");
        }

        _rigidBody = GetComponent<Rigidbody>();
        if (_rigidBody == null)
        {
            Debug.LogError("Rigidbody not found on the RangedEnemy.");
        }
    }

    private void Update()
    {
        if (_isDead || _hero == null) return;

        float distanceToHero = Vector3.Distance(transform.position, _hero.position);

        if (distanceToHero > _attackRange)
        {
            FollowHero();
        }
        else
        {
            StopMoving();
            AttackHero();
        }
    }

    private void FollowHero()
    {
        Vector3 directionToHero = (_hero.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(directionToHero);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        Vector3 moveDirection = directionToHero * _moveSpeed;
        _rigidBody.velocity = new Vector3(moveDirection.x, _rigidBody.velocity.y, moveDirection.z); 

        _animator.SetBool("IsWalking", true);
    }

    private void AttackHero()
    {
        if (Time.time - _lastAttackTime >= _attackCooldown)
        {
            _lastAttackTime = Time.time;
            _animator.SetTrigger("IsShutting");

            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        if (_projectilePrefab == null || _firePoint == null) return;

        GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);

        feu proj = projectile.GetComponent<feu>();
        if (proj != null)
        {
            proj.SetTarget(_hero); 
            proj.SetDamage(_damage); 
        }

        Debug.Log("Projectile fired towards the hero!");
    }

    private void StopMoving()
    {
        _rigidBody.velocity = Vector3.zero;
        _animator.SetBool("IsWalking", false);
    }

    private void HandleDeath()
    {
        _isDead = true;
        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsDying", true);
    }
}