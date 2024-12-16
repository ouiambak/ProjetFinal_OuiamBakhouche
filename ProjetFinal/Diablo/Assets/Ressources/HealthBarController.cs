
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Ajout pour gérer le changement de scène

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealth();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealth();

        // Vérifie si la santé atteint 0
        if (currentHealth <= 0)
        {
            GameOver(); // Appelle la méthode GameOver si la santé est 0
        }
    }

    public void UpdateHealth()
    {
        if (healthBarImage != null)
        {
            float healthPercentage = currentHealth / maxHealth;
            healthBarImage.fillAmount = healthPercentage;
        }
    }

    // Méthode pour charger la scène Game Over
    private void GameOver()
    {
        // Charge la scène Game Over. Remplace "GameOver" par le nom de ta scène Game Over
        SceneManager.LoadScene("GameOver");
    }
}
/*using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealth();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        if (healthBarImage != null)
        {
            float healthPercentage = currentHealth / maxHealth;
            healthBarImage.fillAmount = healthPercentage;
        }
    }
}*/