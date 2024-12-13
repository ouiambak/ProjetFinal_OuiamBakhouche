using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _enemyKillCountText;
    [SerializeField] private Transform _playerTransform;
    public static GameManager _instance;
    private int _numberOfObject = 0;
    public static Transform PlayerTransform
    {
        get
        {
            if (_instance != null)
            {
                return _instance._playerTransform;
            }
            Debug.LogError("GameManager instance is null! Make sure it is initialized.");
            return null;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance == this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    public void IncreaseNbOfObject()
    {
        _numberOfObject++;
        UpdateDisplay();
    }

    public void DecreaseNbOfObject()
    {
        _numberOfObject--;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (_enemyKillCountText != null)
        {
            _enemyKillCountText.text = "Enemies Killed: " + _numberOfObject.ToString(); // Affiche le nombre
        }
    }
    

 
}
