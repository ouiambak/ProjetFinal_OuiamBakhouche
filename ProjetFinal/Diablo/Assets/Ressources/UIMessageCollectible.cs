using UnityEngine;
using UnityEngine.UI;

public class UIMessageCollectible : MonoBehaviour
{
    [SerializeField] private GameObject _messageText; 
    [SerializeField] private Transform _player; 
    [SerializeField] private float _displayDistance = 3f; 
    private GameObject _currentCollectable; 
    private bool _isCollecting = false; 

    void Start()
    {
        if (_messageText != null)
        {
            _messageText.SetActive(false); 
        }
    }

    void Update()
    {
        if (!_isCollecting)
        {
            CheckForCollectable();
        }
        else
        {
            _messageText.SetActive(false); 
        }

        if (Input.GetKeyDown(KeyCode.E) && _currentCollectable != null && !_isCollecting)
        {
            CollectObject();
        }
    }

    private void CheckForCollectable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_player.position, _displayDistance);
        bool isNearCollectable = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Collectable"))
            {
                isNearCollectable = true;
                _currentCollectable = hitCollider.gameObject; 
                break;
            }
        }

        if (isNearCollectable)
        {
            _messageText.SetActive(true);
        }
        else
        {
            _messageText.SetActive(false); 
        }
    }

    private void CollectObject()
    {
       
        if (_currentCollectable != null)
        {
           
            _isCollecting = true;
            _messageText.SetActive(false);
            Debug.Log("Objet collecté!");
            _currentCollectable = null;
        }
    }
}




