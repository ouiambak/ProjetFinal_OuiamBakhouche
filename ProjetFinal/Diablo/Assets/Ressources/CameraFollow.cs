using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset = new Vector3(0, 5, -10);
    [SerializeField] private float _smoothTime = 0.3f;

    [Header("Boundary Settings")]
    [SerializeField] private Vector3 _minBoundary; 
    [SerializeField] private Vector3 _maxBoundary; 

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (_player != null)
        {
            Vector3 targetPosition = _player.position + _offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, _smoothTime);
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, _minBoundary.x, _maxBoundary.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, _minBoundary.y, _maxBoundary.y);
            smoothedPosition.z = Mathf.Clamp(smoothedPosition.z, _minBoundary.z, _maxBoundary.z);
            transform.position = smoothedPosition;
            transform.LookAt(_player);
        }
    }
}
