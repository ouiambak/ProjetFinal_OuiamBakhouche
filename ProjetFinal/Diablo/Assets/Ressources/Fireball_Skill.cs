using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball_Skill : MonoBehaviour
{
    [SerializeField] private FireBalle _fireball;
    [SerializeField] private Transform _characterHand;
    [SerializeField] private float _collDownDelay=5f;
    [SerializeField] private float _animationDelay = 0.5f;
    [SerializeField] private Animator _animator;
    private float _timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse1)&& _timer>_collDownDelay) 
        {
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                HealthAndDefense health = hit.collider.GetComponent<HealthAndDefense>();
                if (health != null)
                {
                    _animator.transform.parent.LookAt(health.transform.position);
                    StartCoroutine(SendFireball(health.transform));
                }
            }
        }
    }
    public float GetCoolDownRatio(){
        return 1f-(_timer/_collDownDelay);
    }
    private IEnumerator SendFireball(Transform target)
    {
        _animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(_animationDelay);
        FireBalle newFireBalle = Instantiate(_fireball, _characterHand.position, Quaternion.identity);
        newFireBalle.SetTarget(target);
        _timer = 0;
        yield return new WaitForSeconds(_animationDelay);
        _animator.SetBool("IsAttacking", false);
    }
}
