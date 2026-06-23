using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f; // How fast the character turns
    private Rigidbody rb;
    
    // Drag your "Main Camera" here in the Inspector
    public Camera playerCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (playerCamera == null) playerCamera = Camera.main;
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 1. Calculate Forward and Right based on where the Camera is looking
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0; // Flatten to ground
        cameraForward.Normalize();

        Vector3 cameraRight = playerCamera.transform.right;
        cameraRight.y = 0; // Flatten to ground
        cameraRight.Normalize();

        // 2. Combine them (W+D becomes Diagonal)
        Vector3 moveDirection = (cameraForward * vertical) + (cameraRight * horizontal);

        // 3. Move and Rotate
        if (moveDirection.magnitude > 0.1f)
        {
            // Rotate character to face movement direction smoothly
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // Move the physics body
            rb.linearVelocity = moveDirection.normalized * speed;
        }
        else
        {
            // Stop moving when no keys are pressed
            rb.linearVelocity = Vector3.zero;
        }
    }
}