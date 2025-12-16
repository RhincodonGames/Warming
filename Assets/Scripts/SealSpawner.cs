using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealSpawner : MonoBehaviour
{
    public GameObject sealPrefab;
    public int initialSealCount = 3;
    public float spawnRadius = 20f;
    public float respawnDelay = 60f;  // Time before respawning seals
    public int maxSealCount = 5;

    private List<GameObject> activeSeals = new List<GameObject>();

    private void Start()
    {
        SpawnInitialSeals();
    }

    void SpawnInitialSeals()
    {
        for (int i = 0; i < initialSealCount; i++)
        {
            SpawnSeal();
        }
    }

    void SpawnSeal()
    {
        if (sealPrefab == null)
        {
            Debug.LogWarning("No seal prefab assigned to SealSpawner!");
            return;
        }

        if (activeSeals.Count >= maxSealCount)
            return;

        // Random position within spawn radius
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

        GameObject newSeal = Instantiate(sealPrefab, spawnPos, Quaternion.identity);
        activeSeals.Add(newSeal);
    }

    private void Update()
    {
        // Remove destroyed seals from list
        activeSeals.RemoveAll(seal => seal == null);

        // Respawn seals if below initial count
        if (activeSeals.Count < initialSealCount)
        {
            StartCoroutine(RespawnSealAfterDelay());
        }
    }

    IEnumerator RespawnSealAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (activeSeals.Count < initialSealCount)
        {
            SpawnSeal();
        }
    }

    // Visualize spawn area in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}