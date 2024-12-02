using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image healthBarImage; 
    [SerializeField] private float maxHealth = 100f; 
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth; 
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); 
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            float healthPercentage = currentHealth / maxHealth;
            healthBarImage.fillAmount = healthPercentage; 
        }
    }
}
