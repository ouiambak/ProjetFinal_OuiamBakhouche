using System.Collections;
using UnityEngine;

public class ObjectToSpawner : MonoBehaviour
{
    [SerializeField] private float _selfDestroyMaxDelay = 10f;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float directionChangeInterval = 1f;
    [SerializeField] private Animator _animator;  
    private Vector3 randomDirection;

    void Start()
    {
        GameManager._instance.IncreaseNbOfObject();
        _meshRenderer.material.color = Random.ColorHSV(0f, 1f);
        Destroy(gameObject, Random.Range(0f, _selfDestroyMaxDelay));
        StartCoroutine(ChangeDirectionPeriodically());
        _animator.SetBool("IsWalking", true);  
    }

    void Update()
    {
        if (randomDirection != Vector3.zero)
        {
            transform.Translate(randomDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    private IEnumerator ChangeDirectionPeriodically()
    {
        while (true)
        {
            SetRandomDirection();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void SetRandomDirection()
    {
        randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }

    private void OnDestroy()
    {
        GameManager._instance.DecreaseNbOfObject();
        _animator.SetBool("IsWalking", false);  
    }
}