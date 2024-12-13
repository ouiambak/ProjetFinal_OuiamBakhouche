

/*public class PlayerHealthAndDefense : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private HealthBarController _healthBarController;

    private float _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        if (_healthBarController != null)
        {
            _healthBarController.UpdateHealth(_currentHealth / _maxHealth); // Initialise la barre de vie
        }
    }

    public void ReceiveDamage(int damage)
    {
        if (damage <= 0) return; // Évite des dégâts négatifs ou nuls

        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        Debug.Log($"Player received {damage} damage. Current health: {_currentHealth}");

        if (_healthBarController != null)
        {
            _healthBarController.UpdateHealth(_currentHealth / _maxHealth); // Met à jour la barre de vie
        }

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player is dead.");

        // Désactiver les contrôles du joueur
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Déclencher une animation de mort si un Animator est présent
        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("IsDead");
        }

        // Exemple : Afficher un écran de Game Over (si implémenté)
        // GameManager.Instance.ShowGameOverScreen();

        // Exemple : Désactiver temporairement le GameObject après la mort
        // gameObject.SetActive(false);
    }

    public void Heal(float healAmount)
    {
        if (healAmount <= 0) return; // Évite des soins nuls ou négatifs

        _currentHealth += healAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        Debug.Log($"Player healed by {healAmount}. Current health: {_currentHealth}");

        if (_healthBarController != null)
        {
            _healthBarController.UpdateHealth(_currentHealth / _maxHealth); // Met à jour la barre de vie
        }
    }
}
*/
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
 
