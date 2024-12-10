using UnityEngine;
using UnityEngine.SceneManagement;

public class ThroneScript : MonoBehaviour
{
    [SerializeField] private string winSceneName = "YouWin";
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hero touched the throne! You win!");
            SceneManager.LoadScene(winSceneName);
        }
    }
}
