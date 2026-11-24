using UnityEngine;

public class SoclailButtonManager : MonoBehaviour
{
    [SerializeField] private MainMenuManager.SocialButtons _buttonType;
    public void ButtonClicked()
    {
        MainMenuManager._.SocialButtonClicked(_buttonType);
    }

}
