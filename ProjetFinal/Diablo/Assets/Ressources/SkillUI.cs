using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Image _skillCoolDown;  
    [SerializeField] private Color _blueColor = Color.blue;  
    [SerializeField] private Color _defaultColor = Color.white;  

    public void ActivateBluePower(bool isActive)
    {
        if (isActive)
        {
            _skillCoolDown.color = _blueColor;
           
        }
        else
        {
            _skillCoolDown.color = _defaultColor;
        }
    }
}
