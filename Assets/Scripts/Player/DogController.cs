using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class DogController : MonoBehaviour
{
    [SerializeField] private InputActionAsset dogActions;
    private InputAction mousePos;
    private InputAction run;

    [Header("Movement Settings")]
    [SerializeField] private float moveForce = 20f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float turnThresholdAngle = 60f;
    [SerializeField] private float brakingForce = 3f;

    private Rigidbody rb;
    private Vector3 targetPosition;
    private Plane plane = new Plane(Vector3.up, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (run.ReadValue<float>() > 0f)
        {
            UpdateTargetPosition();
            
            if (targetPosition != Vector3.zero)
            {
                Vector3 directionToTarget = (targetPosition - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

                // If angle is too large, stop and turn
                if (angleToTarget > turnThresholdAngle)
                {
                    // Apply braking force
                    ApplyBrakingForce();
                }

                // Handle rotation
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                );

                // Only move forward if we're facing roughly the right direction
                if (angleToTarget <= turnThresholdAngle)
                {
                    // Calculate how much additional speed we need
                    float currentSpeed = rb.linearVelocity.magnitude;
                    if (currentSpeed < maxSpeed)
                    {
                        // Calculate force needed to reach but not exceed max speed
                        float speedDiff = maxSpeed - currentSpeed;
                        float appliedForce = Mathf.Min(moveForce, speedDiff * moveForce / maxSpeed);
                        rb.AddForce(transform.forward * appliedForce, ForceMode.Force);
                    }
                    // else if (currentSpeed > maxSpeed)
                    // {
                    //     // Gently reduce speed if we're going too fast
                    //     Vector3 normalizedVelocity = rb.linearVelocity.normalized;
                    //     rb.linearVelocity = normalizedVelocity * maxSpeed;
                    // }
                }
            }
        }
        else
        {
            ApplyBrakingForce();
        }

        // Ensure velocity stays in the XZ plane
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
    }

    private void ApplyBrakingForce()
    {
        // Calculate braking force based on current speed
        Vector3 brakingVelocity = -rb.linearVelocity;
        float brakingMagnitude = Mathf.Min(brakingForce * rb.linearVelocity.magnitude, brakingForce);
        rb.AddForce(brakingVelocity.normalized * brakingMagnitude, ForceMode.Acceleration);
    }

    private void UpdateTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            targetPosition = ray.GetPoint(distance);
            targetPosition.y = transform.position.y;
        }
    }

    private void OnEnable()
    {
        mousePos = dogActions.FindActionMap("Main").FindAction("mousePos");
        mousePos.Enable();
        run = dogActions.FindActionMap("Main").FindAction("run");
        run.Enable();
    }

    private void OnDisable()
    {
        mousePos.Disable();
        run.Disable();
    }

    void OnDrawGizmos()
    {
        if (targetPosition != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetPosition, 0.5f);
        }
    }
}
