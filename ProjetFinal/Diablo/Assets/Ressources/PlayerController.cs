using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _stoppingDistance = 0.75f;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private int _damage = 5;
    [SerializeField] private LayerMask _clickable;
    [SerializeField] private Material _blueMaterial;
    [SerializeField] private float _powerEffectDuration = 5f;
    private Material _originalMaterial;
    private Renderer _playerRenderer;

    private Animator _animator;
    private Rigidbody _rigidBody;
    private Camera _camera;
    private Vector3 _targetPosition;
    private HealthAndDefense _currentEnemy;
    private bool _isPowerEffectActive = false;
    private Coroutine _powerEffectCoroutine;
    private float _lastAttackTime = 0f;

    [Header("Audio")]
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _knifeAttackSound;
    [SerializeField] private AudioClip _shieldCollectSound;
    private AudioSource _audioSource;

    void Start()
    {
        _targetPosition = transform.position;
        _camera = Camera.main;
        if (_camera == null) Debug.LogError("Main camera not found!");

        _rigidBody = GetComponent<Rigidbody>();
        if (_rigidBody == null) Debug.LogError("Rigidbody not found on the player!");

        _animator = GetComponentInChildren<Animator>();
        if (_animator == null) Debug.LogError("Animator not found in children!");

        _playerRenderer = GetComponentInChildren<Renderer>();
        if (_playerRenderer != null)
        {
            _originalMaterial = _playerRenderer.material;
        }
        else
        {
            Debug.LogWarning("Renderer not found on the player.");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Detect closest enemy
        DetectClosestEnemy();

        // Move player to target position
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _clickable))
            {
                _targetPosition = hit.point;
                transform.LookAt(new Vector3(_targetPosition.x, transform.position.y, _targetPosition.z));
            }
        }

        // Knife attack (space)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            KnifeAttack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            FireAttack();
        }
        // Player movement
        float distance = Vector3.Distance(transform.position, _targetPosition);
        if (distance > _stoppingDistance)
        {
            Vector3 direction = (_targetPosition - transform.position).normalized;
            Vector3 newPosition = transform.position + (_movementSpeed * direction * Time.deltaTime);
            _rigidBody.MovePosition(newPosition);
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
            _rigidBody.velocity = Vector3.zero;
        }
    }
    // Ajoutez cette méthode dans PlayerController
    private void FireAttack()
    {
        float fireRadius = 5f; // Rayon de l'attaque de feu

        // Détection des ennemis dans la zone
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, fireRadius);

        foreach (var hitCollider in hitColliders)
        {
            HealthAndDefense enemy = hitCollider.GetComponent<HealthAndDefense>();
            if (enemy != null)
            {
                enemy.Kill(); // Tuer chaque ennemi touché
            }
        }

        _animator.SetTrigger("IsAttacking");
        PlaySound(_attackSound);
        Debug.Log("Fire attack performed. Enemies killed.");
    }

    private void KnifeAttack()
    {
        if (_currentEnemy == null)
        {
            Debug.Log("No enemy targeted for KnifeAttack.");
            ResetAttack();
            return;
        }

        if (Time.time >= _lastAttackTime + _attackCooldown)
        {
            if (Vector3.Distance(transform.position, _currentEnemy.transform.position) < 2f)
            {
                _lastAttackTime = Time.time;
                _animator.SetTrigger("IsCasting");
                PlaySound(_knifeAttackSound);
                _currentEnemy.ReceiveDamage(_damage);
                Debug.Log("Knife attack successful.");
            }
            else
            {
                Debug.Log("Enemy is out of range.");
                ResetAttack();
            }
        }
        else
        {
            Debug.Log("KnifeAttack is on cooldown.");
        }
    }

    public void ResetAttack()
    {
        StartCoroutine(ResetAttackAnimation());
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Temps pour terminer l'animation
        _animator.SetTrigger("IsIdle");
        Debug.Log("Attack animation reset.");
    }

    private void DetectClosestEnemy()
    {
        float detectionRadius = 2f; // Rayon de détection pour les ennemis
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        float closestDistance = Mathf.Infinity;
        HealthAndDefense closestEnemy = null;

        foreach (var hitCollider in hitColliders)
        {
            HealthAndDefense enemy = hitCollider.GetComponent<HealthAndDefense>();
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        _currentEnemy = closestEnemy;
        Debug.Log($"Detected enemy: {_currentEnemy?.name ?? "None"}");
    }

    private void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (_audioSource != null && clip != null)
        {
            _audioSource.PlayOneShot(clip, volume);
        }
    }

    public void CollectShield()
    {
        PlaySound(_shieldCollectSound);
    }

    public void TakeDamage(float damage)
    {
        HealthBarController healthBar = GetComponent<HealthBarController>();
        if (healthBar != null)
        {
            healthBar.UpdateHealth(damage);
        }
        else
        {
            Debug.LogError("HealthBarController not found on player!");
        }
    }
}
