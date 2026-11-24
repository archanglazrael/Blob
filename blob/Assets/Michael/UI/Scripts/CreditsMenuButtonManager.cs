using UnityEngine;

public class CreditsMenuButtonManager : MonoBehaviour
{
    [SerializeField] private MainMenuManager.SocialButtons _buttonType;
    public void ButtonClicked()
    {
        MainMenuManager._.SocialButtonClicked(_buttonType);
    }
}
