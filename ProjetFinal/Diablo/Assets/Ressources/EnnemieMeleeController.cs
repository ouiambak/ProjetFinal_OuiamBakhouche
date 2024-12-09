using UnityEngine;

public class EnnemieMeleeController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private int _damage = 10;

    private Transform _hero;
    private bool _isDead = false;
    private bool _isAttacking = false;
    private float _lastAttackTime = 0f;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _hero = GameManager.PlayerTransform;
        if (_hero == null)
        {
            Debug.LogError("PlayerTransform not found in GameManager! Ensure it is assigned.");
            enabled = false;
            return;
        }

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
        }

        HealthAndDefense healthComponent = GetComponent<HealthAndDefense>();
        if (healthComponent != null)
        {
            healthComponent.OnEnemyDeath += HandleDeath;
        }
    }

    private void Update()
    {
        if (!_isDead && _hero != null)
        {
            float distanceToHero = Vector3.Distance(transform.position, _hero.position);

            if (distanceToHero > _attackRange)
            {
                FollowHero();
                _animator.SetBool("IsAttacking", false);
            }
            else
            {
                AttackHero();
            }
        }
        else if (!_isDead)
        {
            _animator.SetBool("IsBreathing", true);
        }

        if (_isAttacking && !IsAttacking())
        {
            _isAttacking = false;
            _animator.SetBool("IsWalking", false);
        }
    }

    private void FollowHero()
    {
        if (_isAttacking) return;

        Vector3 directionToHero = (_hero.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToHero);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        Vector3 moveDirection = directionToHero * _moveSpeed;
        _rigidbody.velocity = new Vector3(moveDirection.x, _rigidbody.velocity.y, moveDirection.z);  
        _animator.SetBool("IsWalking", true);
        _animator.SetBool("IsBreathing", false);
    }

    private void AttackHero()
    {
        StopMoving();

        if (Time.time - _lastAttackTime >= _attackCooldown)
        {
            _lastAttackTime = Time.time;
            _animator.SetTrigger("IsAttacking");
            _isAttacking = true;
            Invoke(nameof(DealDamage), 0.5f);
        }
    }

    private void DealDamage()
    {
        if (_hero.TryGetComponent<PlayerHealthAndDefense>(out PlayerHealthAndDefense playerHealth))
        {
            playerHealth.ReceiveDamage(_damage);
        }
    }

    private void StopMoving()
    {
        _rigidbody.velocity = Vector3.zero; 
        _animator.SetBool("IsWalking", false);
    }

    private bool IsAttacking()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("Attack");
    }

    private void HandleDeath()
    {
        _isDead = true;
        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsDying", true);
        _animator.SetBool("IsBreathing", false);
        _animator.SetBool("IsAttacking", false);
        _rigidbody.velocity = Vector3.zero; 
    }
}
