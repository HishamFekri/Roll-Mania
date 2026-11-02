using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Rigidbody playerRigidbody;
    
    [Header("Camera Settings")]
    public float cameraDistance = 5f; // Distance behind the player
    public float cameraHeight = 2f; // Height above the player
    public float rotationSpeed = 5f;
    public float minVelocityThreshold = 0.1f; // Minimum velocity to rotate camera
    public float lookAheadDistance = 2f; // How far ahead to look (for better view)
    
    private Vector3 currentDirection = Vector3.back; // Default camera direction

    void Start()
    {
        if (player != null)
        {
            playerRigidbody = player.GetComponent<Rigidbody>();
            // Initialize camera direction based on current offset
            Vector3 initialOffset = transform.position - player.transform.position;
            initialOffset.y = 0f;
            if (initialOffset.magnitude > 0.1f)
            {
                currentDirection = -initialOffset.normalized;
            }
        }
    }

    void LateUpdate()
    {
        if (player == null) return;
        
        Vector3 targetDirection = currentDirection;
        
        // Update camera direction based on player movement
        if (playerRigidbody != null)
        {
            Vector3 velocity = playerRigidbody.velocity;
            velocity.y = 0f; // Ignore vertical velocity
            
            // Only update direction if ball is moving with sufficient speed
            if (velocity.magnitude > minVelocityThreshold)
            {
                // Camera should be behind the player, so use negative velocity direction
                targetDirection = -velocity.normalized;
            }
        }
        
        // Smoothly interpolate camera direction
        currentDirection = Vector3.Slerp(currentDirection, targetDirection, rotationSpeed * Time.deltaTime);
        currentDirection.Normalize();
        
        // Calculate camera position: behind player at specified distance and height
        Vector3 targetPosition = player.transform.position + (currentDirection * cameraDistance);
        targetPosition.y = player.transform.position.y + cameraHeight;
        
        // Smoothly move camera to target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, rotationSpeed * Time.deltaTime);
        
        // Make camera look at player (or slightly ahead in movement direction)
        Vector3 lookTarget = player.transform.position;
        if (playerRigidbody != null && playerRigidbody.velocity.magnitude > minVelocityThreshold)
        {
            Vector3 velocity = playerRigidbody.velocity;
            velocity.y = 0f;
            lookTarget += velocity.normalized * lookAheadDistance;
        }
        
        transform.LookAt(lookTarget);
    }
}
