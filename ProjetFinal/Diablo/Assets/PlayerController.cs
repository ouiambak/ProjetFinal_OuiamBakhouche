using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed=5f;
    [SerializeField] private float _stoppingDistance=0.75f;
    [SerializeField] private float _attackCoolDown=1.5f;
    [SerializeField] private int _damage=5;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private Camera _cam ;
    private Vector3 _targetPosition;
    private HealthAndDefense _currentEnemy;


    private bool _attackIsActive;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetBool("IsWalking", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)){

            Ray ray;
            RaycastHit hit;
            ray=_cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                HealthAndDefense enemy= hit.collider.GetComponent<HealthAndDefense>();
                if (enemy != null)
                {
                    _currentEnemy = enemy;
                    _attackIsActive = true;
                    

                }
                else
                {
                    _currentEnemy = null;
                    _targetPosition = hit.point;
                    transform.LookAt( _targetPosition );
                }
                
            }
        }

        if (_currentEnemy!=null) { 
            _targetPosition= _currentEnemy.transform.position;
            transform.LookAt(_currentEnemy.transform.position);
        }

        float _distance = (transform.position - _targetPosition).magnitude;
        Vector3 _direction = (_targetPosition - transform.position).normalized;

        if (_distance > _stoppingDistance) { 

            _rigidbody.velocity = _movementSpeed * _direction;
            _animator.SetBool("IsWalking", true);

        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
            _animator.SetBool("IsWalking", false);
        }

        if (_attackIsActive && _distance<_stoppingDistance) 
        {
            Attack();
        }
    }

    private void Attack()
    {
        _animator.SetBool("IsAttacking", true);
        _attackIsActive= false;
        _currentEnemy.ReceiveDamage(_damage);
    }
    public void ResetAttack()
    {
        _animator.SetBool("IsAttacking", false);
    }
    
}
