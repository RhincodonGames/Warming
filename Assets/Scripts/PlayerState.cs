using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public PlayerMovement playerMovement;

    // Player Health //
    public float currentHealth;
    public float maxHealth;

    // Player Hunger //
    public float currentHunger;
    public float maxHunger;
    
    float distanceTraveled = 0;
    Vector3 lastPosition;
    public GameObject playerBody;

    // Player Hydration //
    public float currentHydrationPercent;
    public float maxHydrationPercent;
    public bool isHydrationActive;

    // Player Stamina
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 10f;    // Stamina recovered per second
    public float staminaRegenDelay = 2f;    // Delay before starting regeneration
    public float timeSinceStaminaUsed = 0f;
    
    public GameObject staminaBar;

    public static PlayerState Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentHydrationPercent = maxHydrationPercent;
        currentStamina = maxStamina;

        if (staminaBar != null)
        {
            staminaBar.SetActive(false);
        }

        StartCoroutine(decreaseHydration());
        StartCoroutine(decreaseHunger());
        StartCoroutine(increaseHealth());
    }

    IEnumerator decreaseHydration()
    {
        while (true)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(10);
        }
    }

    IEnumerator decreaseHunger()
    {
        while (true)
        {
            currentHunger -= 0.1f;
            yield return new WaitForSeconds(10);
        }
    }

    IEnumerator increaseHealth()
    {
        while (true)
        {
            if (currentHunger > 75)
            {
                currentHealth += 1f;
            }
            yield return new WaitForSeconds(5);
        }
    }

    void Update()
    {
        //Check distance player traveled add distances
        distanceTraveled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTraveled >= 10 && playerMovement.isSprinting)
        {
            distanceTraveled = 0;
            currentHunger -= 0.20f;
            currentHydrationPercent -= 0.25f;
        }
        else if (distanceTraveled >= 10 && !playerMovement.isSprinting)
        {
            distanceTraveled = 0;
            currentHunger -= 0.15f;
            currentHydrationPercent -= 0.1f;
        }

        // Stamina Regeneration
        RegenerateStamina();

        // Show/Hide Stamina Bar
        if (staminaBar != null && currentStamina != maxStamina)
        {
            staminaBar.SetActive(true);
        }
        else if (staminaBar != null && currentStamina >= maxStamina)
        {
            staminaBar.SetActive(false);
        }

        //Testing Purposes
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
            currentHunger -= 10;
        }
    }

    // Stamina Methods
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