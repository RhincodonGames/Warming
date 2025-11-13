using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;

    public bool isDisplayed = false;
    
    //Should have save file progress
    public void ContinueGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    //Should erase old save file and restart game to oringal scene (time = 0)
    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void DisplaySettings()
    {
        Debug.Log("Settings Displayed");

        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);

        isDisplayed = true;
    }

    public void Back()
    {
        Debug.Log("Settings Screen Closed");

        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        isDisplayed = false;
    }

    public void Update()
    {
        if (isDisplayed && Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
}
