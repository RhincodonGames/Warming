using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject GameOverOverlay;
    public PlayerState playerState;
    public bool isPaused = false;

    private bool gameOverTriggered = false;

    public void Update()
    {
        // Check if player health is 0 or below
        if (playerState.currentHealth <= 0 && !gameOverTriggered)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        gameOverTriggered = true;
        GameOverOverlay.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Unlock cursor so player can click buttons
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }

    // Call this method from the "Continue" button
    public void Continue()
    {
        // Reset all player stats to full
        playerState.currentHealth = playerState.maxHealth;
        playerState.currentHunger = playerState.maxHunger;
        playerState.currentHydrationPercent = playerState.maxHydrationPercent;
        playerState.currentStamina = playerState.maxStamina;

        // Unpause the game
        GameOverOverlay.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        gameOverTriggered = false;

        // Re-lock cursor for gameplay (adjust based on your game's cursor settings)
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Call this method from the "Quit" button - returns to main menu
    public void Quit()
    {
        Time.timeScale = 1f;
        isPaused = false;
        gameOverTriggered = false;

        SceneManager.LoadScene("MenuScene"); // Change "MainMenu" to your actual menu scene name
    }
}