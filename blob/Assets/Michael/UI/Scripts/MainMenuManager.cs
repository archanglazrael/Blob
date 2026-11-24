using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager _;
    [SerializeField] private bool _debugMode;
    public enum SocialButtons { start, credits, quit, MainMenu };
    public enum MainMenuButtons { page1, page2, page3, page4, page5, page6, page7 }
    [SerializeField] GameObject _Credits2Container;
    [SerializeField] GameObject _CreditsContainer;
    [SerializeField] GameObject _Credits3Container;
    [SerializeField] GameObject _Credits4Container;
    [SerializeField] GameObject _Credits5Container;
    [SerializeField] GameObject _Credits6Container;
    [SerializeField] GameObject _Credits7Container;

    public void Awake()
    {
        if (_ == null)
        {
            _ = this;
        }
        else
        {
            Debug.LogError("There are more than 1 MainMenuManager's in the scene");
        }

    }
    private void Start()
    {
        OpenCredits(_Credits2Container);
    }
    public void SocialButtonClicked(SocialButtons buttonClicked)
    {
        DebugMessage("ButtonClicked: " + buttonClicked.ToString());
        switch (buttonClicked)
        {
            case SocialButtons.start:
                StartClicked();
                break;
            case SocialButtons.credits:
                CreditsClicked();
                break;
            case SocialButtons.quit:
                QuitGame();
                break;
            case SocialButtons.MainMenu:
                MainMenu();
                break;
            default:
                Debug.Log("button click wansnt implimented in MainMenuManagerMethod");
                break;
        }
    }
    
    public void Page1()
    {
        OpenCredits(_Credits2Container);
    }
    public void Page2()
    {
        OpenCredits(_CreditsContainer);
    }
    public void Page3()
    {
        OpenCredits(_Credits3Container);
    }
    public void Page4()
    {
        OpenCredits(_Credits4Container);
    }
    public void Page5()
    {
        OpenCredits(_Credits5Container);
    }
    public void Page6()
    {
        OpenCredits(_Credits6Container);
    }
    public void Page7()
    {
        OpenCredits(_Credits7Container);
    }
    public void MainMenuButtonClicked(MainMenuButtons buttonClicked)
    {
        switch (buttonClicked)
        {
            case MainMenuButtons.page1:
                Page1();
                break;
            case MainMenuButtons.page2:
                Page2();
                break;
            case MainMenuButtons.page3:
                Page3();
                break;
            case MainMenuButtons.page4:
                Page4();
                break;
            case MainMenuButtons.page5:
                Page5();
                break;
            case MainMenuButtons.page6:
                Page6();
                break;
            case MainMenuButtons.page7:
                Page7();
                break;
        }
    }
    private void DebugMessage(string message)
    {
        if (_debugMode)
        {
            Debug.Log(message);
        }
    }
    public void StartClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void CreditsClicked()
    {
        SceneManager.LoadScene("Credits");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
    }
    public void OpenCredits(GameObject creditsToOpen)
    {
        _CreditsContainer.SetActive(creditsToOpen == _CreditsContainer);
        _Credits2Container.SetActive(creditsToOpen == _Credits2Container);
        _Credits3Container.SetActive(creditsToOpen == _Credits3Container);
        _Credits4Container.SetActive(creditsToOpen == _Credits4Container);
        _Credits5Container.SetActive(creditsToOpen == _Credits5Container);
        _Credits6Container.SetActive(creditsToOpen == _Credits6Container);
        _Credits7Container.SetActive(creditsToOpen == _Credits7Container);
    }
}
