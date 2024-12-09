using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _stopingDistance = 0.75f;
    [SerializeField] private float _attackCoolDown = 1.5f;
    [SerializeField] private int _damage = 5;
    [SerializeField] private LayerMask _clickable;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private Material _blueMaterial;  
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

    void Start()
    {
        _targetPosition = new Vector3(0, 2.3f, 0);
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray;
            RaycastHit hit;
            ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _clickable))
            {
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
    

    }

    private void Attack()
    {
        _animator.SetBool("IsAttacking", true);
        _attackIsActive = false;
        _currentEnemy.ReceiveDamage(_damage);
    }

    public void ResetAttack()
    {
        _animator.SetBool("IsAttacking", false);
    }

    private void ActivatePowerEffect()
    {
        if (_playerRenderer != null && _blueMaterial != null)
        {
            _playerRenderer.material = _blueMaterial;  
        }
    }

    public void DeactivatePowerEffect()
    {
        if (_playerRenderer != null && _originalMaterial != null)
        {
            _playerRenderer.material = _originalMaterial; 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _wallLayer) != 0)
        {
            ChangeColorToBlue();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _wallLayer) != 0)
        {
            ResetColor();
        }
    }

    private void ChangeColorToBlue()
    {
        if (_playerRenderer != null && _blueMaterial != null)
        {
            _playerRenderer.material = _blueMaterial;
            Debug.Log("Player color changed to blue.");
        }
    }

    private void ResetColor()
    {
        if (_playerRenderer != null && _originalMaterial != null)
        {
            _playerRenderer.material = _originalMaterial;
            Debug.Log("Player color restored.");
        }
    }
}
