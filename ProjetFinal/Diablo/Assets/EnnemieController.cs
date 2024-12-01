using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemieController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _followDistance = 5f;
    [SerializeField] private Transform _hero;

    private bool isDead = false;

    private void Start()
    {
        if (_hero == null)
        {
            Debug.LogWarning("Hero transform not assigned! Please assign the hero.");
        }


        HealthAndDefense healthComponent = GetComponent<HealthAndDefense>();
        if (healthComponent != null)
        {
            healthComponent.OnEnemyDeath += HandleDeath;
        }
    }

    private void Update()
    {
        if (!isDead && _hero != null)
        {

            float distanceToHero = Vector3.Distance(transform.position, _hero.position);


            if (distanceToHero > _followDistance)
            {

                Vector3 directionToHero = (_hero.position - transform.position).normalized;
                transform.Translate(directionToHero * _moveSpeed * Time.deltaTime, Space.World);


                _animator.SetBool("IsWalking", true);
            }
            else
            {

                _animator.SetBool("IsWalking", false);
            }
        }
    }

    private void HandleDeath()
    {
        isDead = true;
        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsDying", true);
    }
}
