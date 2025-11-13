using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public PlayerMovement isSprinting;

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

        if (distanceTraveled >= 10 && isSprinting)
        {
            distanceTraveled = 0;
            currentHunger -= 0.20f;
            currentHydrationPercent -= 0.25f;
        }
        else if (distanceTraveled >= 10 && !isSprinting)
        {
            distanceTraveled = 0;
            currentHunger -= 0.15f;
            currentHydrationPercent -= 0.1f;
        }

        //Testing Purposes
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
            currentHunger -= 10;
        }
    }
}
