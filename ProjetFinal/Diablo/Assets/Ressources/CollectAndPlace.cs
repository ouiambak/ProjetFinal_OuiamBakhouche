using System.Collections;
using UnityEngine;

public class CollectAndPlace : MonoBehaviour
{
    [SerializeField] private Transform _heroBack;
    [SerializeField] private Transform _placementPoint;
    [SerializeField] private GameObject[] _doors;
    [SerializeField] private Vector3 _leftDoorOpenPosition; 
    [SerializeField] private Vector3 _rightDoorOpenPosition; 
    [SerializeField] private float _doorRotationSpeed = 2f; 
    [SerializeField] private float _interactionDistance = 2f;

    private GameObject _carriedObject;
    private bool _isCarrying = false;
    private bool _isPlaced = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!_isCarrying)
            {
                TryCollectObject();
            }
            else if (!_isPlaced)
            {
                TryPlaceObject();
            }
        }
    }

    private void TryCollectObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _interactionDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Collectable"))
            {
                Debug.Log($"Collectable detected: {hitCollider.name}");

                _carriedObject = hitCollider.gameObject;

                // Vérifiez si l'objet est actif
                Debug.Log($"Object active status before parenting: {_carriedObject.activeSelf}");

                _carriedObject.transform.SetParent(_heroBack);
                _carriedObject.transform.localPosition = Vector3.zero;

                Debug.Log($"Object parented to: {_carriedObject.transform.parent.name}");
                Debug.Log($"Object position after parenting: {_carriedObject.transform.position}");

                _isCarrying = true;
                return;
            }
        }
    }



    private void TryPlaceObject()
    {
        float distanceToPlacementPoint = Vector3.Distance(transform.position, _placementPoint.position);
        if (distanceToPlacementPoint <= _interactionDistance)
        {
            _carriedObject.transform.SetParent(null);
            _carriedObject.transform.position = _placementPoint.position;
            _isPlaced = true;
            _isCarrying = false;
            Debug.Log("Object placed!");

                OpenDoors();
        }
    }

    private void OpenDoors()
    {
        
        foreach (GameObject door in _doors)
        {
            if (door != null)
            {
                
                if (door.CompareTag("LeftDoor"))
                {
                    StartCoroutine(RotateDoor(door, _leftDoorOpenPosition)); 
                }
                else if (door.CompareTag("RightDoor"))
                {
                    StartCoroutine(RotateDoor(door, _rightDoorOpenPosition)); 
                }
                Debug.Log("Door opened!");
            }
        }
    }

    private IEnumerator RotateDoor(GameObject door, Vector3 openPosition)
    {
        Quaternion initialRotation = door.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(openPosition);
        float timeElapsed = 0f;
        while (timeElapsed < 1f)
        {
            door.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, timeElapsed);
            timeElapsed += Time.deltaTime * _doorRotationSpeed;
            yield return null;
        }
        door.transform.rotation = targetRotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _interactionDistance);
    }
}
