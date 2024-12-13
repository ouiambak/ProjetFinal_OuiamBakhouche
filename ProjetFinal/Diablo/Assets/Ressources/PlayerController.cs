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

    [SerializeField] private float _stopingDistance = 0.75f;
    [SerializeField] private float _attackCoolDown = 1.5f;

    private Material _originalMaterial;
    private Renderer _playerRenderer;
    
    private Animator _animator;
    private Rigidbody _rigidBody;
    private Camera _camera;
    private Vector3 _targetPosition;
    private HealthAndDefense _currentEnemy;
    private bool _isPowerEffectActive = false;
    private bool _attackIsActive;
    private bool _isWalking = false;
    private bool _isAttacking = false;
    private bool _isCasting = false;
    private bool _isDying = false;
    private bool _isBlueEffectActive = false;
    private Coroutine _powerEffectCoroutine;
    private float _lastAttackTime = 0f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip _knifeAttackSound;
    [SerializeField] private AudioClip _shieldCollectSound;
    private AudioSource _audioSource;
    
    
    void Start()
    {
        _targetPosition = new Vector3(0, 2.3f, 0);
        _camera = Camera.main;
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetBool("IsWalking", false);
        _audioSource = GetComponent<AudioSource>();
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _clickable))
            {
                _targetPosition = hit.point;
                transform.LookAt(new Vector3(_targetPosition.x, transform.position.y, _targetPosition.z));
           
                    HealthAndDefense enemy = hit.collider.GetComponent<HealthAndDefense>();

                    if (enemy != null)
                    {
                        _currentEnemy = enemy;
                        _attackIsActive = true;
                    }
                    else
                    {
                        _currentEnemy = null;
                        _targetPosition = hit.point;
                        transform.LookAt(_targetPosition);
                    }
                
            }
            
        }

        if (_currentEnemy != null)
        {
            _targetPosition = _currentEnemy.transform.position;
            transform.LookAt(_currentEnemy.transform.position);
        }


        float distance = (transform.position - _targetPosition).magnitude;
        Vector3 direction = (_targetPosition - transform.position).normalized;

        if (distance > _stopingDistance)
        {
            _rigidBody.velocity = _movementSpeed * direction;
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
            _rigidBody.velocity = Vector3.zero;
        }

        if (_attackIsActive && distance < _stopingDistance)
        {
            Attack();
        }
        else
        {
            ResetAttack();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (_isBlueEffectActive)
            {
               
                ResetBlueEffect();
            }
            else
            {
               
                ActivateBlueEffect();
            }
        }

    }

    private void Attack()
    {
        _animator.SetBool("IsAttacking", true);
        _attackIsActive = false;
        PlaySound(_knifeAttackSound);
        _currentEnemy.ReceiveDamage(_damage);
    }

    public void ResetAttack()
    {
        _animator.SetBool("IsAttacking", false);
    }

    private void SetWalkingAnimation(bool isWalking)
    {
        _animator.SetBool("isWalking", isWalking);
    }
    private void SetAttackingAnimation(bool isWalking)
    {
        _animator.SetBool("isAttacking", isWalking);
    }
    private void SetCastingAnimation(bool isWalking)
    {
        _animator.SetBool("isCasting", isWalking);
    }
    private void SetDyingAnimation(bool isWalking)
    {
        _animator.SetBool("isDying", isWalking);
    }
    

    private void DetectClosestEnemy()
    {
        float detectionRadius = 2f; 
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

    public void TakeDamage()
    {
        HealthBarController healthBar = GetComponent<HealthBarController>();
        if (healthBar != null)
        {
            healthBar.UpdateHealth();
        }
        else
        {
            Debug.LogError("HealthBarController not found on player!");
        }
    }

    public void Die()
    {
        if (_animator.GetBool("isDying")) return;

        _animator.SetTrigger("isDying");
        Debug.Log("Player is dying.");
    }

    private void ActivateBlueEffect()
    {
        if (_playerRenderer != null && _blueMaterial != null)
        {
            _playerRenderer.material = _blueMaterial; 
            _isBlueEffectActive = true;               
            Debug.Log("Blue effect activated!");
        }
    }

    private void ResetBlueEffect()
    {
        if (_playerRenderer != null && _originalMaterial != null)
        {
            _playerRenderer.material = _originalMaterial; 
            _isBlueEffectActive = false;                  
            Debug.Log("Blue effect reset!");
        }
    }

}

