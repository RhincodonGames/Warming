using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Basic Movement Variables
    public CharacterController controller;
    public float speed = 6f;
    public float swimSpeed = 4f;

    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    // Special Movement Variables
    public float sprintSpeed = 8f;
    public float swimSprintSpeed = 6f;

    public float dashSpeed = 10f;

    public float crouchSpeed = 3f;

    // Update is called once per frame
    void Update()
    {
        BasicMovement();
        SpecialMovement();
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

            // Move in that direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    void SpecialMovement()
    {
        // Sprint

        // Swim Sprint

        // Dash

        // Crouch
    }
}
