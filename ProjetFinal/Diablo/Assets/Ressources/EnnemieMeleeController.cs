using UnityEngine;

public class EnnemieMeleeController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private int _damage = 10;

    private Transform _hero;
    private bool isDead = false;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    private void Start()
    {
        _hero = GameManager.PlayerTransform;
        if (_hero == null)
        {
            Debug.LogError("PlayerTransform not found in GameManager! Ensure it is assigned.");
            enabled = false;
            return;
        }

        HealthAndDefense healthComponent = GetComponent<HealthAndDefense>();
        if (healthComponent != null)
        {
            healthComponent.OnEnemyDeath += HandleDeath;
        }
    }

    private void Update()
    {
        if (!isDead && _hero != null)
        {
            float distanceToHero = Vector3.Distance(transform.position, _hero.position);

            if (distanceToHero > _attackRange)
            {
                FollowHero();
            }
            else
            {
                AttackHero();
            }
        }
        else if (!isDead)
        {
            _animator.SetBool("IsBreathing", true);
        }

        if (isAttacking && !IsAttacking())
        {
            isAttacking = false; 
            _animator.SetBool("IsWalking", false); 
        }
    }

    private void FollowHero()
    {
        if (isAttacking) return;

        Vector3 directionToHero = (_hero.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToHero);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        transform.Translate(directionToHero * _moveSpeed * Time.deltaTime, Space.World);

        _animator.SetBool("IsWalking", true);
        _animator.SetBool("IsBreathing", false);
    }

    private void AttackHero()
    {
        StopMoving();

        if (Time.time - lastAttackTime >= _attackCooldown)
        {
            lastAttackTime = Time.time;
            _animator.SetTrigger("IsAttacking");
            isAttacking = true;
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
        _animator.SetBool("IsWalking", false);
    }

    private bool IsAttacking()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("Attack"); 
    }

    private void HandleDeath()
    {
        isDead = true;
        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsDying", true);
        _animator.SetBool("IsBreathing", false);
    }
}
