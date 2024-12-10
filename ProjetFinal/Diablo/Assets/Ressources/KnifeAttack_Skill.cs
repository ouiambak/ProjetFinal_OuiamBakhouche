using System.Collections;
using UnityEngine;
using TMPro;

public class KnifeAttack_Skill : MonoBehaviour
{
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _cooldownDelay = 1.5f;
    [SerializeField] private float _animationDelay = 0.5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private TextMeshProUGUI _killCounterText;
    private float _timer;
    private int _enemiesKilled = 0;
    private Transform _hero;

    private void Start()
    {
        _hero = GameManager.PlayerTransform;
        if (_hero == null)
        {
            Debug.LogError("PlayerTransform not found in GameManager! Ensure it is assigned.");
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (Input.GetMouseButtonDown(2) && _timer > _cooldownDelay && IsEnemyNearby())
        {
            StartCoroutine(PerformKnifeAttack());
        }
    }

    private bool IsEnemyNearby()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator PerformKnifeAttack()
    {
        // Lancer l'animation de casting immédiatement après l'entrée de la touche
        _animator.SetBool("IsCasting", true);
        // Attendez le délai d'animation avant d'effectuer l'attaque
        yield return new WaitForSeconds(_animationDelay);
        // Détection des ennemis à portée d'attaque
        Collider[] hits = Physics.OverlapSphere(transform.position, _attackRange);
        bool enemyAttacked = false;
        // Appliquez les dégâts aux ennemis à proximité
        foreach (var hit in hits)
        {
            HealthAndDefense enemyHealth = hit.GetComponent<HealthAndDefense>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10);
                DisplayDamageText("Dégâts: 10");
                if (enemyHealth.IsDead())
                {
                    IncrementKillCounter();
                }

                enemyAttacked = true;
            }
        }

        // Réinitialiser l'animation si aucun ennemi n'a été touché
        if (!enemyAttacked)
        {
            _animator.SetBool("IsCasting", false);
        }

        _timer = 0;
        yield return new WaitForSeconds(_animationDelay);

        // Réinitialiser l'animation après le délai
        _animator.SetBool("IsCasting", false);
    }


    private void DisplayDamageText(string damageMessage)
    {
        _damageText.text = damageMessage;
        StartCoroutine(HideDamageText());
    }

    private IEnumerator HideDamageText()
    {
        yield return new WaitForSeconds(2f);
        _damageText.text = "";
    }

    private void IncrementKillCounter()
    {
        _enemiesKilled++;
        _killCounterText.text = "Ennemis tués: " + _enemiesKilled;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
