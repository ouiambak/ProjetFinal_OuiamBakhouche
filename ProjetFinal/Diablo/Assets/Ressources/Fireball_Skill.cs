using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_Skill : MonoBehaviour
{
    [SerializeField] private FireBalle _fireball;
    [SerializeField] private Transform _characterHand;
    [SerializeField] private float _collDownDelay = 5f;
    [SerializeField] private float _animationDelay = 0.5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _FireSound;
    private float _timer;
    private AudioSource _audioSource;

    void Start() { }

    void Update()
    {
        _timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse1) && _timer > _collDownDelay)
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

    public float GetCoolDownRatio()
    {
        return 1f - (_timer / _collDownDelay);
    }

    private IEnumerator SendFireball(Transform target)
    {
        _animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(_animationDelay);

        FireBalle newFireBalle = Instantiate(_fireball, _characterHand.position, Quaternion.identity);
        newFireBalle.SetTarget(target);
        PlaySound(_FireSound);
        _timer = 0;

        yield return new WaitForSeconds(_animationDelay);
        _animator.SetBool("IsAttacking", false);
    }
    private void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (_audioSource != null && clip != null)
        {
            _audioSource.PlayOneShot(clip, volume);
        }
    }
}
