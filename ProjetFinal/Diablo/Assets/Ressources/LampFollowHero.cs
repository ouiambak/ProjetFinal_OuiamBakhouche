using UnityEngine;

public class LampFollowHero : MonoBehaviour
{
    [SerializeField] private Transform _hero; 
    [SerializeField] private Vector3 _offset = new Vector3(0, 3f, 0); 
    [SerializeField] private float _followSpeed = 5f; 

    private void Start()
    {
        if (_hero == null)
        {
            Debug.LogError("Le héros n'est pas assigné dans le script de la lampe.");
            enabled = false; 
        }
    }

    private void Update()
    {
        Vector3 targetPosition = _hero.position + _offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);
    }
}
