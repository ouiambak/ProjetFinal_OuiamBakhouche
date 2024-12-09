using UnityEngine;

public class PlayerHealthAndDefense : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f; 
    [SerializeField] private HealthBarController _healthBarController;
    
    private float _currentHealth;
    private void Start()
    {
        _currentHealth = _maxHealth;
        if (_healthBarController != null)
        {
            _healthBarController.TakeDamage(0); 
        }
    }

    public void ReceiveDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (_healthBarController != null)
        {
            _healthBarController.TakeDamage(damage); 
        }

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player is dead.");
    }
}
