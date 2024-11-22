using UnityEngine;

public class HealthAndDefense : MonoBehaviour
{
    [SerializeField] private int _health=100;
    public void ReceiveDamage(int damage)
    {
        _health = damage;
        Debug.Log("Health remaining"+_health);
    }
}
