using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenuManager : MonoBehaviour
{
    public GameObject PauseOverlay;
    
    //All UI elements should pause game (all movement and audio -> switch to general audio if in fight)
    private bool isPaused = false;

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
        
        //Pauses everything using Time.deltaTime (so animation should be based on this later)
        //Should also stop all animations etc (in future)
        Time.timeScale = 0f;

        isPaused = true;
    }

    public void Resume()
    {
        PauseOverlay.SetActive(false);

        //Resume with 1
        Time.timeScale = 1f;

        isPaused = false;
    }

    public void Save()
    {
        //saves game to new load file
        //max load files will be 3 (to reduce game strain)
    }

    //Load Selected Save File
    public void Load()
    {
        //Opens Save Files
        //Player Selects File
        //Loads back to that save
    }

    public void ReturnToMainMenu()
    {
        //if (!isSaved)
        //{
        //    //Display warning dialogue and give player chance to save or ignore warning
        //}

        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");

    }
}
