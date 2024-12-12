using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;  
public class SceneChangerButton : MonoBehaviour
{
    [SerializeField] private string sceneNameToLoad;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Le bouton n'a pas été trouvé !");
        }
    }
    private void OnButtonClick()
    {
        SceneManager.LoadScene(sceneNameToLoad);
    }
}
