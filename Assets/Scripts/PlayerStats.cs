using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Stamina System
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 10f;    // Stamina recovered per second
    public float staminaRegenDelay = 2f;    // Delay before starting regeneration
    public float timeSinceStaminaUsed = 0f;
    //private bool staminaUpgrade = false;

    //Health System
    //public float maxHealth = 100f;
    //public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize stats
        currentStamina = maxStamina;
        //currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        RegenerateStamina();
    }

    public bool UseStamina(float amount)
    {
        // Check if player has enought stamina for movement/attack
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            currentStamina = Mathf.Max(currentStamina, 0);

            // Reset regeneration timer
            timeSinceStaminaUsed = 0f;

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasStamina(float amount)
    {
        return currentStamina >= amount;
    }

    void RegenerateStamina()
    {
        // Don't regenerate if stamina is full
        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
            return;
        }

        // Increment timer
        timeSinceStaminaUsed += Time.deltaTime;

        // Only regen after delay
        if (timeSinceStaminaUsed >= staminaRegenDelay)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);     // Don't exceed max stamina
        }
    }

    public void RestoreStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Min(currentStamina, maxStamina);     // Don't exceed max stamina
    }

    
}
