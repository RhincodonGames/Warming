using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Inherit from InteractableObject so SelectionManager can detect it
public class WaterSource : InteractableObject
{
    public float hydrationRestored = 25f;
    public float cooldownTime = 2f;
    private float lastDrinkTime = -999f;

    private void Start()
    {
        ItemName = "Water";
        ItemCategory = "Interactable";
    }

    // Override the base Interact method - use 'new' keyword to hide base implementation
    public new void Interact()
    {
        // Check cooldown
        if (Time.time - lastDrinkTime < cooldownTime)
        {
            Debug.Log("Too soon to drink again! Wait a moment.");
            return;
        }

        PlayerState playerState = PlayerState.Instance;
        if (playerState != null)
        {
            // Restore hydration
            playerState.currentHydrationPercent += hydrationRestored;

            // Cap at max hydration
            if (playerState.currentHydrationPercent > playerState.maxHydrationPercent)
            {
                playerState.currentHydrationPercent = playerState.maxHydrationPercent;
            }

            lastDrinkTime = Time.time;
            Debug.Log("Drank water! Restored " + hydrationRestored + " hydration. Current: " + playerState.currentHydrationPercent);
        }

        // DON'T call base.Interact() - we don't want to add water to inventory or destroy it
    }
}