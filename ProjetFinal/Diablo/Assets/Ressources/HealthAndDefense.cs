using UnityEngine;

public class HealthAndDefense : MonoBehaviour
{
    [SerializeField] private int _health = 100;

    public delegate void OnDeath();
    public event OnDeath OnEnemyDeath;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        Debug.Log($"{gameObject.name} a perdu {damage} points de vie. Vie restante : {_health}");
        if (_health <= 0)
        {
            Die();
        }
    }

    public void ReceiveDamage(int damage)
    {
        _health = damage;
        Debug.Log("Health remaining: " + _health);
    }

    private void Die()
    {
        OnEnemyDeath?.Invoke();
        Destroy(gameObject, 0.5f);
    }

    public bool IsDead()
    {
        return _health <= 0;
    }
}
