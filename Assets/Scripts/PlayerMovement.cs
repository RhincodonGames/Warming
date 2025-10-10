using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Basic and Special Movement Variables
    public CharacterController controller;
    public Animator animator;

    // Movement Speeds
    public float walkSpeed = 6f;
    public float swimSpeed = 4f;
    public float sprintSpeed = 8f;
    public float swimSprintSpeed = 6f;
    public float dashSpeed = 15f;
    public float crouchSpeed = 3f;

    // Movement Settings
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    // Dash Settings
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private float dashTimeRemaining = 0f;
    private float dashCooldownRemaining = 0f;
    private Vector3 dashDirection;

    // Stamina Settings
    public float sprintStaminaCost = 5f;
    public float swimSprintStaminaCost = 2.5f;
    public float dashStaminaCost = 20f;


    // State Variables
    private bool isSwimming = false;
    private bool isSprinting = false;
    private bool isSwimSprinting = false;
    private bool isDashing = false;
    private bool isCrouching = false;

    // Reference to PlayerStats 
    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        // Dash cooldown
        if (dashCooldownRemaining > 0)
        {
            dashCooldownRemaining -= Time.deltaTime;
        }

        if (isDashing)
        {
            HandleDash();
        }
        else
        {
            // Perform regular movement
            SpecialMovement();
            BasicMovement();
        }
    }

    void BasicMovement()
    {
        // Get Player Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Calculates movement direction (normalizes so diagonal movement isn't faster)
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Only moves/rotates if players is pressing keys
        if (direction.magnitude >= 0.1f)
        {
            // Calculate angle to face
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Smoothly rotate towards calculated angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            //Quaternion (how Unity stores rotations using Euler values) vs. Euler (degrees we can read, X, Y, Z)
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            float currentSpeed = GetCurrentSpeed();

            // Move in that direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

            // Insert Animations Here Later
        }
        else
        {
            // Insert Idle Animation (when not moving)
        }
    }

    void SpecialMovement()
    {
        // Sprint Logic
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && !isDashing)
        {
            // Check if player still has stamina (stamina > 1 to fix continous sprint bug)
            if (playerStats.currentStamina > 1)
            {
                if (!isSprinting)
                {
                    isSprinting = true;
                }

                // Drain stamina while sprinting
                playerStats.UseStamina(sprintStaminaCost * Time.deltaTime);

                // Animation Logic
            } 
            else
            {
                isSprinting = false;
            }
        }
        else
        {
            // Not pressing sprint, or crouching, or dashing (always stop sprinting)
            if (isSprinting)
            {
                isSprinting = false;
            }
        }

        // Swim Sprint Logic

        // Dash Logic
        if (Input.GetKey(KeyCode.Space) && dashCooldownRemaining <= 0 && !isCrouching)
        {
            if (playerStats.currentStamina >= dashStaminaCost)
            {
                StartDash();
            }
        }

        // Crouch Logic
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            isSprinting = false;

            // Crouch Animation
        }
        else
        {
            isCrouching = false;

            // Disable Crouch Animation
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimeRemaining = dashDuration;
        dashCooldownRemaining = dashCooldown;

        // Store direction to dash in
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDirection.magnitude < 0.1f)
        {
            dashDirection = transform.forward;
        }
        else
        {
            dashDirection = inputDirection;
        }

        playerStats.UseStamina(dashStaminaCost);

        // Dash Animation
    }

    void HandleDash()
    {
        dashTimeRemaining -= Time.deltaTime;

        // move in dash direction
        controller.Move(dashDirection * dashSpeed * Time.deltaTime);

        // End dash when time remaining = 0
        if (dashTimeRemaining <= 0)
        {
            isDashing = false;
        }
    }

    float GetCurrentSpeed()
    {
        // Determine speed based on current state
        if (isCrouching)
        {
            return crouchSpeed;
        }
        else if (isSprinting)
        {
            if (isSwimming)
                return swimSprintSpeed;
            else
                return sprintSpeed;
        }
        else
        {
            if (isSwimming)
                return swimSpeed;
            else
                return walkSpeed;
        }
    }
}
