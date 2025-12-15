using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableObject : InteractableObject
{
    // Object Health
    public float maxHealth = 50f;
    public float currentHealth;

    // Drop Settings
    public GameObject[] stoneDrops; // for boulder
    public GameObject[] woodDrops;  // for tree
    public GameObject[] iceDrops;   // for iceberg

    public int minDrops = 1;
    public int maxDrops = 3;

    private void Start()
    {
        currentHealth = maxHealth;

        // Make sure ItemCategory is "Attackable"
        ItemCategory = "Attackable";
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        SpawnDrops();
        Destroy(gameObject);
    }

    void SpawnDrops()
    {
        GameObject[] possibleDrops = null;

        // Determine which prefab array to use based on ItemName
        switch (ItemName)
        {
            case "Boulder":
                possibleDrops = stoneDrops;
                break;
            case "Tree":
                possibleDrops = woodDrops;
                break;
            case "Iceberg":
                possibleDrops = iceDrops;
                break;
            default:
                Debug.LogWarning("Unknown attackable type: " + ItemName);
                return;
        }

        if (possibleDrops == null || possibleDrops.Length == 0)
            return;

        int dropCount = Random.Range(minDrops, maxDrops + 1);

        for (int i = 0; i < dropCount; i++)
        {
            GameObject dropPrefab = possibleDrops[Random.Range(0, possibleDrops.Length)];

            // Spawn in front of the object
            Vector3 forwardDir = transform.forward;
            Vector3 offset = forwardDir * Random.Range(1f, 2f); // 1-2 units in front
            offset += new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, Random.Range(-0.5f, 0.5f));

            Vector3 spawnPos = transform.position + offset;

            Instantiate(dropPrefab, spawnPos, Quaternion.identity);
        }
    }
}
