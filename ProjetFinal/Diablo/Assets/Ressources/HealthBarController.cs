using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image _healthBarImage; 
    [SerializeField] private float _maxHealth = 100f; 
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth; 
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth); 
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (_healthBarImage != null)
        {
            float healthPercentage = _currentHealth / _maxHealth;
            _healthBarImage.fillAmount = healthPercentage; 
        }
    }
}
