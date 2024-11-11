using System.Collections;
using System.Collections.Generic;
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


    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)){

            Ray ray;
            RaycastHit hit;
            ray=_cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {

                _targetPosition = hit.point;
                transform.LookAt( _targetPosition );


            }
        }

        float _distance = (transform.position - _targetPosition).magnitude;

        if (_distance > _stoppingDistance) { 

            Vector3 _direction = (_targetPosition - transform.position).normalized;
            _rigidbody.velocity = _movementSpeed * _direction;
            _animator.SetBool("IsWalking", true);

        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
            _animator.SetBool("IsWalking", false);
        }
        
            
    }
}
