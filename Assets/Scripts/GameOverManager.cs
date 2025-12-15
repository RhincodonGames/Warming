using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject GameOverOverlay;
    public PlayerState playerState;
    public PlayerMovement playerMovement;

    // Respawn Settings
    public float respawnHealthPercent = 1f;     // 1 = full health
    public float respawnStaminaPercent = 1f;

    private bool gameOverTriggered = false;
    private Vector3 deathPosition;

    public void Update()
    {
        // Check if player health is 0 or below
        if (playerState.currentHealth <= 0 && !gameOverTriggered)
        {
            // Save the death position
            deathPosition = playerState.playerBody.transform.position;
            GameOver();
        }
    }

    public void GameOver()
    {
        gameOverTriggered = true;
        Time.timeScale = 0f;
        GameOverOverlay.SetActive(true);

        playerMovement.enabled = false;
    }

    // Call this method from the "Continue" button
    public void Continue()
    {
        // Reset time scale before reloading
        Time.timeScale = 1f;
        GameOverOverlay.SetActive(false);
        gameOverTriggered = false;

        RespawnPlayer();

        playerMovement.enabled = true;
    }

    void RespawnPlayer()
    {
        CharacterController controller = playerState.playerBody.GetComponent<CharacterController>();

        controller.enabled = false;
        playerState.playerBody.transform.position = deathPosition;
        controller.enabled = true;

        // Reset Stats
        playerState.currentHealth = playerState.maxHealth * respawnHealthPercent;

        playerState.currentStamina = playerState.maxStamina * respawnStaminaPercent;

        playerState.currentHunger = playerState.maxHunger;
        playerState.currentHydrationPercent = playerState.maxHydrationPercent;

        playerState.timeSinceStaminaUsed = 0f;

        // Reset Movement
        playerMovement.ResetMovement();

        playerState.currentHealth = Mathf.Max(playerState.currentHealth, 1f);
    }


    // Call this method from the "Quit" button - returns to main menu
    public void Quit()
    {
        Time.timeScale = 1f;
        gameOverTriggered = false;

        SceneManager.LoadScene("MenuScene"); // Change "MainMenu" to your actual menu scene name
    }
}