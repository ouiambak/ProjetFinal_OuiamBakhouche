using UnityEngine;

public class PlayerHealthAndDefense : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; 
    private float currentHealth;
    [SerializeField] private HealthBarController healthBarController; 

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthBarController != null)
        {
            healthBarController.TakeDamage(0); 
        }
    }

    public void ReceiveDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBarController != null)
        {
            healthBarController.TakeDamage(damage); 
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player is dead.");
    }
}
