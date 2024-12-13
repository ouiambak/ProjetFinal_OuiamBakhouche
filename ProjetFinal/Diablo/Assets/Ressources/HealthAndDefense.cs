using System;
using System.Collections;
using UnityEngine;

public class HealthAndDefense : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    [SerializeField] private GameObject _fireball;

    public delegate void OnDeath();
    public event OnDeath OnEnemyDeath;
    public float Health { get; private set; } = 100f;
    public bool IsAlive => Health > 0;
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;
    private bool _isDead = false;
    private Animator _animator;

    void Start()
    {
        _currentHealth = _maxHealth;
        _animator = GetComponentInChildren<Animator>(); 
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        Debug.Log($"{gameObject.name} a perdu {damage} points de vie. Vie restante : {_health}");
        if (_health <= 0)
        {
            Die();
        }
    }

    public void Kill()
    {
        if (!_isDead)
        {
            _isDead = true;
            Debug.Log($"{gameObject.name} is killed.");
            Die();
        }
    }

    public void ReceiveDamage(int damage)
    {
        if (_isDead) return;

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        Debug.Log($"{gameObject.name} is dead.");

        if (_animator != null)
        {
            _animator.SetTrigger("Die"); 
        }

        OnEnemyDeath?.Invoke();

        GameManager._instance.IncreaseNbOfObject();

        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return _isDead;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FireBalle"))
        {
            ReceiveDamage(50);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Sword"))
        {
            ReceiveDamage(20); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireBalle"))
        {
            ReceiveDamage(50);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Sword"))
        {
            ReceiveDamage(20); 
        }
    }
}
