using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _stopingDistance = 0.75f;
    [SerializeField] private float _attackCoolDown = 1.5f;
    [SerializeField] private int _damage = 5;

    private Animator _animator;
    private Rigidbody _rigidBody;
    private Camera _camera;
    private Vector3 _targetPosition;
    private HealthAndDefense _currentEnemy;

    private bool _attackIsActive;
    private bool _isWalking = false;


    void Start()
    {
        _targetPosition = new Vector3(0,2.3f,0);
        _camera = Camera.main;
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetBool("IsWalking", false);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray;
            RaycastHit hit;
            ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
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
}
