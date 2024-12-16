
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Ajout pour g�rer le changement de sc�ne

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

        // V�rifie si la sant� atteint 0
        if (currentHealth <= 0)
        {
            GameOver(); // Appelle la m�thode GameOver si la sant� est 0
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

    // M�thode pour charger la sc�ne Game Over
    private void GameOver()
    {
        // Charge la sc�ne Game Over. Remplace "GameOver" par le nom de ta sc�ne Game Over
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