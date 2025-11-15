using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public PauseMenuManager pauseMenuManager;
    public GameObject craftingScreenUI;
    public bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !pauseMenuManager.isPaused)
        {
            isOpen = !isOpen;
            craftingScreenUI.SetActive(isOpen);

            if (isOpen)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        }
    }
}
