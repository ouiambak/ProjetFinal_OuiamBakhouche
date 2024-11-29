using UnityEngine;

public class HealthAndDefense : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    [SerializeField] private Animator _animator;
 
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
        Debug.Log("Health remaining" + _health);
    }
    private void Die()
    {
        
        _animator.SetBool("IsDying", true);
        Destroy(gameObject, 2f);
    }
    public bool IsDead()
    {
        return _health <= 0;

    }
}
