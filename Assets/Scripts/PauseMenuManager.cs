using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenuManager : MonoBehaviour
{
    public GameObject PauseOverlay;
    public GameObject QuestsPanel;
    public GameObject InventoryPanel;
    public GameObject SystemPanel;
    
    //All UI elements should pause game (all movement and audio -> switch to general audio if in fight)
    public bool isPaused = false;

    //public bool isSystemDisplayed = false;

    //private bool isSaved = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
                
        }
    }

    public void Pause()
    {
        PauseOverlay.SetActive(true);
        openSystemPanel();
        
        //Pauses everything using Time.deltaTime (so animation should be based on this later)
        //Should also stop all animations etc (in future)
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        InventoryPanel.SetActive(false);
        QuestsPanel.SetActive(false);
        SystemPanel.SetActive(false);
        
        PauseOverlay.SetActive(false);

        //Resume with 1
        Time.timeScale = 1f;

        isPaused = false;
    }

    public void openSystemPanel()
    {
        SystemPanel.SetActive(true);
        InventoryPanel.SetActive(false);
        QuestsPanel.SetActive(false);
    }

    public void openInventoryPanel()
    {
        InventoryPanel.SetActive(true);
        SystemPanel.SetActive(false);
        QuestsPanel.SetActive(false);
    }

    public void openQuestsPanel()
    {
        QuestsPanel.SetActive(true);
        InventoryPanel.SetActive(false);
        SystemPanel.SetActive(false);
    }
}
