using System;
using Unity.VisualScripting;
using UnityEngine;

public class HealthAndDefense : MonoBehaviour
{
    [SerializeField] private int _health = 100;

    public delegate void OnDeath();
    public event OnDeath OnEnemyDeath;
    public float Health { get; private set; } = 100f;
    public bool IsAlive => Health > 0;
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;
    private bool _isDead = false;
    private void Start()
    {
        _currentHealth = _maxHealth;
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
        OnEnemyDeath?.Invoke(); // Déclenche l'événement
        Destroy(gameObject); // Supprime l'ennemi de la scène
    }

    public bool IsDead()
    {
        return _isDead;
    }
}
