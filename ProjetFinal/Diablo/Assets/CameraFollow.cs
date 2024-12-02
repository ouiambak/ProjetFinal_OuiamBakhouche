using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset = new Vector3(0, 5, -10);  
    [SerializeField] private float _smoothTime = 0.3f; 

    private Vector3 velocity = Vector3.zero; 

    void LateUpdate()
    {
        if (_player != null)
        {
           
            Vector3 targetPosition = _player.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, _smoothTime);
            transform.LookAt(_player);
        }
    }
}
