using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public int initialFishCount = 5;
    public float pondRadius = 5f;
    public float spawnHeight = 0.5f;  // Height above pond bottom where fish spawn

    public float respawnDelay = 30f;  // Time before respawning fish
    public int maxFishCount = 10;

    private List<GameObject> activeFish = new List<GameObject>();

    private void Start()
    {
        SpawnInitialFish();
    }

    void SpawnInitialFish()
    {
        for (int i = 0; i < initialFishCount; i++)
        {
            SpawnFish();
        }
    }

    void SpawnFish()
    {
        if (fishPrefab == null)
        {
            Debug.LogWarning("No fish prefab assigned to PondManager!");
            return;
        }

        if (activeFish.Count >= maxFishCount)
            return;

        // Random position within pond radius
        float randomAngle = Random.Range(0f, 360f);
        float randomDistance = Random.Range(0f, pondRadius * 0.8f); // Don't spawn at edge

        float x = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomDistance;
        float z = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomDistance;

        Vector3 spawnPos = transform.position + new Vector3(x, spawnHeight, z);

        GameObject newFish = Instantiate(fishPrefab, spawnPos, Quaternion.identity);

        // Set pond reference
        FishMob fishScript = newFish.GetComponent<FishMob>();
        if (fishScript != null)
        {
            fishScript.pondCenter = transform;
            fishScript.pondRadius = pondRadius;
            fishScript.surfaceLevel = spawnHeight;
        }

        activeFish.Add(newFish);
    }

    private void Update()
    {
        // Remove destroyed fish from list
        activeFish.RemoveAll(fish => fish == null);

        // Respawn fish if below initial count
        if (activeFish.Count < initialFishCount)
        {
            StartCoroutine(RespawnFishAfterDelay());
        }
    }

    IEnumerator RespawnFishAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (activeFish.Count < initialFishCount)
        {
            SpawnFish();
        }
    }

    // Visualize pond in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pondRadius);

        Gizmos.color = Color.cyan;
        Vector3 surfaceCenter = transform.position + Vector3.up * spawnHeight;
        for (int i = 0; i < 360; i += 30)
        {
            float x1 = Mathf.Cos(i * Mathf.Deg2Rad) * pondRadius;
            float z1 = Mathf.Sin(i * Mathf.Deg2Rad) * pondRadius;
            float x2 = Mathf.Cos((i + 30) * Mathf.Deg2Rad) * pondRadius;
            float z2 = Mathf.Sin((i + 30) * Mathf.Deg2Rad) * pondRadius;

            Gizmos.DrawLine(
                surfaceCenter + new Vector3(x1, 0, z1),
                surfaceCenter + new Vector3(x2, 0, z2)
            );
        }
    }
}
