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

    void Update()
    {
        _timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && _timer > _cooldownDelay)
        {
            StartCoroutine(PerformKnifeAttack());
        }
    }

    private IEnumerator PerformKnifeAttack()
    {
        _animator.SetBool("IsCasting", true);
        yield return new WaitForSeconds(_animationDelay);
        Collider[] hits = Physics.OverlapSphere(transform.position, _attackRange);
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
            }
        }

        _timer = 0;
        yield return new WaitForSeconds(_animationDelay);

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

