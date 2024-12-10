using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _stopingDistance = 0.75f;
    [SerializeField] private float _attackCoolDown = 1.5f;
    [SerializeField] private int _damage = 5;
    [SerializeField] private LayerMask _clickable;
    [SerializeField] private Material _blueMaterial;
    private Material _originalMaterial;
    private Renderer _playerRenderer;

    private Animator _animator;
    private Rigidbody _rigidBody;
    private Camera _camera;
    private Vector3 _targetPosition;
    private HealthAndDefense _currentEnemy;
    private bool _isWalking = false;

    // Variables pour gérer l'état du pouvoir spécial
    private bool _isPowerEffectActive = false;

    void Start()
    {
        _targetPosition = transform.position;
        _camera = Camera.main;
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetBool("IsWalking", false);

        _playerRenderer = GetComponentInChildren<Renderer>();
        if (_playerRenderer != null)
        {
            _originalMaterial = _playerRenderer.material;
        }
        else
        {
            Debug.LogWarning("Renderer not found on the player.");
        }
    }

    void Update()
    {
        // Activation/Désactivation du pouvoir spécial avec la touche 'T'
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (_isPowerEffectActive)
            {
                DeactivatePowerEffect();
                _isPowerEffectActive = false;
            }
            else
            {
                ActivatePowerEffect();
                _isPowerEffectActive = true;
            }
        }

        // Mouvement du joueur
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _clickable))
            {
                _targetPosition = hit.point;
                transform.LookAt(_targetPosition);
            }
        }

        if (Input.GetMouseButton(1))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _clickable))
            {
                HealthAndDefense enemy = hit.collider.GetComponent<HealthAndDefense>();
                if (enemy != null)
                {
                    _currentEnemy = enemy;
                    FireAttack();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_currentEnemy != null)
            {
                KnifeAttack();
            }
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
    }

    private void FireAttack()
    {
        if (_currentEnemy != null)
        {
            _animator.SetTrigger("IsAttacking");
            _currentEnemy.ReceiveDamage(_damage);
        }
    }

    private void KnifeAttack()
    {
        if (_currentEnemy != null)
        {
            _animator.SetTrigger("IsCasting");
            _currentEnemy.ReceiveDamage(_damage);
        }
    }

    public void ResetAttack()
    {
        _animator.SetTrigger("IsIdle");
    }

    private void ActivatePowerEffect()
    {
        if (_playerRenderer != null && _blueMaterial != null)
        {
            _playerRenderer.material = _blueMaterial;
        }
    }

    private void DeactivatePowerEffect()
    {
        if (_playerRenderer != null && _originalMaterial != null)
        {
            _playerRenderer.material = _originalMaterial;
        }
    }
}
