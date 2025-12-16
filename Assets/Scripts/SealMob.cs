using System.Collections;
using UnityEngine;

public class SealMob : InteractableObject
{
    // Health
    public float maxHealth = 50f;
    public float currentHealth;

    // Movement
    public float moveSpeed = 3f;
    public float rotationSpeed = 3f;
    public float wanderRadius = 20f;
    public float changeDirectionInterval = 5f;
    private float changeDirectionTimer;
    private Vector3 wanderTarget;
    private Vector3 spawnPosition;

    // Combat
    public float detectionRange = 10f;
    public float attackDamage = 15f;
    public float chaseSpeed = 5f;

    // Drops
    public GameObject meatDropPrefab;
    public int minDrops = 2;
    public int maxDrops = 4;

    // States
    private enum SealState { Wandering, Chasing }
    private SealState currentState = SealState.Wandering;

    private Transform player;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        ItemCategory = "Attackable";
        ItemName = "Seal";

        spawnPosition = transform.position;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("SealMob: Player not found!");

        PickNewWanderTarget();
        changeDirectionTimer = changeDirectionInterval;
    }

    private void Update()
    {
        if (isDead) return;

        switch (currentState)
        {
            case SealState.Wandering:
                HandleWandering();
                CheckForPlayer();
                break;

            case SealState.Chasing:
                HandleChasing();
                break;
        }
    }

    void HandleWandering()
    {
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0)
        {
            PickNewWanderTarget();
            changeDirectionTimer = changeDirectionInterval;
        }

        MoveTowards(wanderTarget, moveSpeed);

        if (Vector3.Distance(transform.position, wanderTarget) < 1f)
            PickNewWanderTarget();
    }

    void HandleChasing()
    {
        if (player == null)
        {
            currentState = SealState.Wandering;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRange * 1.5f)
        {
            currentState = SealState.Wandering;
            PickNewWanderTarget();
            return;
        }

        MoveTowards(player.position, chaseSpeed);
    }

    void CheckForPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
            currentState = SealState.Chasing;
    }

    void MoveTowards(Vector3 target, float speed)
    {
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0;

        transform.position += dir * speed * Time.deltaTime;

        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Trigger damage only if the other object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            PlayerState ps = PlayerState.Instance;
            if (ps != null)
            {
                Debug.Log("Seal hit player! Dealing " + attackDamage + " damage.");
                ps.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Seal took " + damage + " damage! Health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        SpawnDrops();
        Destroy(gameObject);
    }

    void SpawnDrops()
    {
        if (meatDropPrefab == null) return;

        int count = Random.Range(minDrops, maxDrops + 1);
        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
            Instantiate(meatDropPrefab, transform.position + offset, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        Vector3 spawnPos = Application.isPlaying ? spawnPosition : transform.position;
        Gizmos.DrawWireSphere(spawnPos, wanderRadius);
    }

    void PickNewWanderTarget()
    {
        // Pick a random point within the wander radius around the spawn position
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = spawnPosition + new Vector3(randomCircle.x, 0, randomCircle.y);
    }
}
