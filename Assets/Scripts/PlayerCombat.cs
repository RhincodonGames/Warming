using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;

    // Base Stats
    public float baseDamageDealt = 10f;
    public float baseDamageReduction = 0f;

    // Final Stats
    public float damageDealt;
    public float damageReduction;

    // Basic Attack
    public float basicAttackCooldown = 0.5f;
    public float basicAttackStaminaCost = 10f;

    // Heavy Attack
    public float heavyAttackCooldown = 1.5f;
    public float heavyAttackStaminaCost = 25f;
    public float heavyChargeTime = 2f;
    public float heavyDamageMultiplier = 2f;

    private float basicCooldownTimer;
    private float heavyCooldownTimer;

    private bool isChargingHeavy = false;
    private float currentChargeTime = 0f;

    private HelmetType equippedHelmet = HelmetType.None;

    private PlayerState playerState;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        playerState = PlayerState.Instance;
        playerMovement = GetComponent<PlayerMovement>();

        RecalculateStats();
    }

    private void Update()
    {
        UpdateCooldowns();
        HandleInput();
    }

    void HandleInput()
    {
        // Can't attack while sprinting
        if (playerMovement.isSprinting)
            return;

        // Basic Attack (Left Click)
        if (Input.GetMouseButtonDown(0))
        {
            tryBasicAttack();
        }

        // Heavy Attack (Right Click Hold)
        if (Input.GetMouseButtonDown(1))
        {
            StartHeavyCharge();
        }

        if (Input.GetMouseButton(1) && isChargingHeavy)
        {
            ChargeHeavyAttack();
        }

        if (Input.GetMouseButtonUp(1) && isChargingHeavy)
        {
            ReleaseHeavyAttack();
        }
    }
    void tryBasicAttack()
    {
        if (basicCooldownTimer > 0)
            return;

        if (!playerState.UseStamina(basicAttackStaminaCost))
            return;

        basicCooldownTimer = basicAttackCooldown;

        float finalDamage = damageDealt;

        PerformAttack(finalDamage);

        Debug.Log("Basic Attack: " + finalDamage);

        //Animation
    }

    void StartHeavyCharge()
    {
        if (heavyCooldownTimer > 0)
            return;

        if (!playerState.HasStamina(heavyAttackStaminaCost))
            return;

        isChargingHeavy = true;
        currentChargeTime = 0f;

        Debug.Log("Started charging heavy attack");
    }

    void ChargeHeavyAttack()
    {
        currentChargeTime += Time.deltaTime;
        currentChargeTime = Mathf.Min(currentChargeTime, heavyChargeTime);
    }

    void ReleaseHeavyAttack()
    {
        isChargingHeavy = false;

        if (!playerState.UseStamina(heavyAttackStaminaCost))
            return;

        float chargePercent = currentChargeTime / heavyChargeTime;
        float damageMultiplier = Mathf.Lerp(1f, heavyDamageMultiplier, chargePercent);

        float finalDamage = damageDealt * damageMultiplier;

        heavyCooldownTimer = heavyAttackCooldown;

        PerformAttack(finalDamage);

        Debug.Log("Heavy Attack: " + finalDamage + "(Charged at: " + chargePercent + " percent)");
    }

    void PerformAttack(float damage)
    {
        // THIS is where you:
        // - Raycast
        // - Hitbox overlap
        // - Animation events
        // - EnemyHealth.TakeDamage(damage)

        // Placeholder:F
        Debug.Log("");
    }

    // Cooldown Logic
    void UpdateCooldowns()
    {
        if (basicCooldownTimer > 0)
            basicCooldownTimer -= Time.deltaTime;

        if (heavyCooldownTimer > 0)
            heavyCooldownTimer -= Time.deltaTime;
    }

    // Equipemnt (Helmets)
    public void EquipHelmet(HelmetType newHelmet)
    {
        equippedHelmet = newHelmet;
        RecalculateStats();
    }

    void RecalculateStats()
    {
        damageDealt = baseDamageDealt;
        damageReduction = baseDamageReduction;

        switch (equippedHelmet)
        {
            case HelmetType.Wood:
                damageReduction += 5f;
                break;

            case HelmetType.Ice:
                damageReduction += 3f;
                break;

            case HelmetType.Stone:
                damageReduction += 10f;
                break;

            case HelmetType.IceSpikedWood:
                damageReduction += 8f;
                damageDealt += 3f;
                break;

            case HelmetType.IceSpikedStone:
                damageReduction += 13f;
                damageDealt += 3f;
                break;
        }
    }

    public float CalculateIncomingDamage(float incomingDamage)
    {
        return Mathf.Max(incomingDamage - damageReduction, 0f);
    }

    public HelmetType GetEquippedHelmet()
    {
        return equippedHelmet;
    }

    public void UnequipHelmet()
    {
        equippedHelmet = HelmetType.None;
        RecalculateStats();
    }

    //void SpecialAbilites()
    //{
    //    //Movements

    //    //Animations

    //void AbilitySwitching()
    //{
    //    //UI Element

    //    //Animation
    //}

}
