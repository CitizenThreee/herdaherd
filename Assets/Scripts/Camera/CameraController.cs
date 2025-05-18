using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    
    [Header("Camera Settings")]
    [SerializeField] private float height = 2f;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float angle = 45f;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float maxSmoothSpeed = 15f;
    [SerializeField] private float distanceMultiplier = 2f;
    
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Awake()
    {
        player = Object.FindFirstObjectByType<DogController>().gameObject;
        // Set initial rotation
        transform.rotation = Quaternion.Euler(angle, 0, 0);
    }

    void LateUpdate()
    {
        if(player != null)
        {
            // Calculate target position with offset
            targetPosition = player.transform.position;
            targetPosition -= Vector3.forward * distance;
            targetPosition += Vector3.up * height;

            // Calculate current distance from target
            float currentDistance = Vector3.Distance(transform.position, targetPosition);
            
            // Adjust smooth speed based on distance
            float adjustedSmoothSpeed = smoothSpeed;
            if (currentDistance > distance)
            {
                // Increase speed based on how far we are from desired position
                float speedMultiplier = 1f + (currentDistance - distance) * distanceMultiplier;
                adjustedSmoothSpeed = Mathf.Min(smoothSpeed * speedMultiplier, maxSmoothSpeed);
            }

            // Smoothly move the camera towards target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, adjustedSmoothSpeed * Time.deltaTime);
        }
    }

    // Optional: Method to adjust camera settings during runtime
    public void UpdateCameraSettings(float newHeight, float newDistance, float newAngle, float newSmoothSpeed)
    {
        height = newHeight;
        distance = newDistance;
        angle = newAngle;
        smoothSpeed = newSmoothSpeed;
        transform.rotation = Quaternion.Euler(angle, 0, 0);
    }
}
