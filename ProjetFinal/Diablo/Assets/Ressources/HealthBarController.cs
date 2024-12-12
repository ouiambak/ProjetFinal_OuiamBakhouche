using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _healthBarImage;

    public void UpdateHealth(float healthPercentage)
    {
        if (_healthBarImage != null)
        {
            _healthBarImage.fillAmount = healthPercentage;
        }
    }
}
