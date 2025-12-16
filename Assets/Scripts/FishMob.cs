using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMob : InteractableObject
{
    // Health
    public float maxHealth = 20f;
    public float currentHealth;

    // Movement
    public float swimSpeed = 2f;
    public float rotationSpeed = 2f;
    public float changeDirectionInterval = 3f;
    private float changeDirectionTimer;
    private Vector3 currentDirection;

    // Pond Bounds
    public Transform pondCenter;
    public float pondRadius = 5f;
    public float surfaceLevel = 0.5f;  // How high fish swim from pond bottom
    public float depthRange = 1f;      // Vertical movement range

    // Drops
    public GameObject fishDropPrefab;
    public int minDrops = 1;
    public int maxDrops = 2;

    // Fleeing
    public float fleeSpeed = 4f;
    public float fleeDuration = 2f;
    private bool isFleeing = false;
    private float fleeTimer;
    private Vector3 fleeDirection;

    private void Start()
    {
        currentHealth = maxHealth;
        ItemCategory = "Attackable";
        ItemName = "Fish";

        // Find pond center if not assigned
        if (pondCenter == null)
        {
            GameObject pond = GameObject.FindGameObjectWithTag("Pond");
            if (pond != null)
                pondCenter = pond.transform;
            else
                pondCenter = transform; // Use self if no pond found
        }

        // Start with random direction
        PickNewDirection();
        changeDirectionTimer = changeDirectionInterval;
    }

    private void Update()
    {
        if (isFleeing)
        {
            HandleFleeing();
        }
        else
        {
            HandleNormalSwimming();
        }

        // Keep fish within pond bounds
        KeepInPond();
    }

    void HandleNormalSwimming()
    {
        // Timer to change direction periodically
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0)
        {
            PickNewDirection();
            changeDirectionTimer = changeDirectionInterval;
        }

        // Move fish
        transform.position += currentDirection * swimSpeed * Time.deltaTime;

        // Rotate to face direction
        if (currentDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void HandleFleeing()
    {
        fleeTimer -= Time.deltaTime;

        if (fleeTimer <= 0)
        {
            isFleeing = false;
            PickNewDirection();
            return;
        }

        // Move away quickly
        transform.position += fleeDirection * fleeSpeed * Time.deltaTime;

        // Rotate to face flee direction
        if (fleeDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(fleeDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * 2f * Time.deltaTime);
        }
    }

    void PickNewDirection()
    {
        // Random direction in horizontal plane
        float randomAngle = Random.Range(0f, 360f);
        float randomX = Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float randomZ = Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        // Add slight vertical movement
        float randomY = Random.Range(-0.3f, 0.3f);

        currentDirection = new Vector3(randomX, randomY, randomZ).normalized;
    }

    void KeepInPond()
    {
        if (pondCenter == null) return;

        Vector3 directionFromCenter = transform.position - pondCenter.position;
        directionFromCenter.y = 0; // Only check horizontal distance

        float distanceFromCenter = directionFromCenter.magnitude;

        // If too far from center, turn back
        if (distanceFromCenter > pondRadius)
        {
            Vector3 directionToCenter = -directionFromCenter.normalized;
            currentDirection = new Vector3(directionToCenter.x, currentDirection.y, directionToCenter.z).normalized;

            // Move back towards center
            transform.position = Vector3.MoveTowards(transform.position, pondCenter.position, swimSpeed * Time.deltaTime);
        }

        // Keep at proper depth
        float minY = pondCenter.position.y + surfaceLevel;
        float maxY = pondCenter.position.y + surfaceLevel + depthRange;

        if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
            currentDirection.y = Mathf.Abs(currentDirection.y);
        }
        else if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
            currentDirection.y = -Mathf.Abs(currentDirection.y);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Flee when hit
        StartFleeing();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void StartFleeing()
    {
        isFleeing = true;
        fleeTimer = fleeDuration;

        // Flee away from player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;
            fleeDirection = new Vector3(directionAwayFromPlayer.x, 0f, directionAwayFromPlayer.z);
        }
        else
        {
            // Random flee direction if no player found
            fleeDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        }
    }

    void Die()
    {
        SpawnDrops();
        Destroy(gameObject);
    }

    void SpawnDrops()
    {
        if (fishDropPrefab == null)
        {
            Debug.LogWarning("No fish drop prefab assigned!");
            return;
        }

        int dropCount = Random.Range(minDrops, maxDrops + 1);

        for (int i = 0; i < dropCount; i++)
        {
            Vector3 spawnOffset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                0.5f,
                Random.Range(-0.5f, 0.5f)
            );

            Vector3 spawnPos = transform.position + spawnOffset;
            Instantiate(fishDropPrefab, spawnPos, Quaternion.identity);
        }
    }

    // Visualize pond bounds in editor
    private void OnDrawGizmosSelected()
    {
        if (pondCenter == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pondCenter.position, pondRadius);

        Gizmos.color = Color.blue;
        Vector3 surfaceCenter = pondCenter.position + Vector3.up * surfaceLevel;
        Gizmos.DrawWireSphere(surfaceCenter, pondRadius);
    }
}
