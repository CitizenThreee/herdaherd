using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))] // Changed to 3D collider
[RequireComponent(typeof(Rigidbody))]
public class SheepLogic : MonoBehaviour
{
    [SerializeField] private Collider agentCollider;
    [SerializeField] private Rigidbody rb;
    
    [Header("Movement Settings")]
    public float maxForce = 10f;
    [SerializeField] private float turnSpeed = 180f; // Degrees per second

    [Header("Panic Settings")]
    [SerializeField] private float dogDetectionRadius = 3f;
    [SerializeField] private float sheepPanicDetectionRadius = 1f;
    [SerializeField] private float panicDecayRate = 0.2f;
    [SerializeField] private float panicCooldownTime = 2f;
    [SerializeField] private float highPanicThreshold = 0.8f;

    private float panicValue = 0f;
    private float timeSinceLastPanic = 0f;
    private bool isPanicked = false;

    public SheepBehaviour relaxedBehaviour;
    public SheepBehaviour panickedBehaviour;

    public Collider AgentCollider
    {
        get { return agentCollider; }
    }

    public float PanicValue => panicValue;
    public bool IsPanicked => isPanicked;

    private void Update()
    {
        UpdatePanicState();
    }

    private void UpdatePanicState()
    {
        // Decay panic value over time
        if (panicValue > 0)
        {
            panicValue = Mathf.Max(0, panicValue - panicDecayRate * Time.deltaTime);
        }

        // Check for dog proximity
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, dogDetectionRadius);
        bool dogDetected = false;
        bool highPanicSheepDetected = false;

        foreach (Collider collider in nearbyColliders)
        {
            // Check for dog using Player tag
            if (collider.CompareTag("Player"))
            {
                dogDetected = true;
                panicValue = 1f;
                break;
            }

            // Check for panicked sheep within closer radius using Herd tag
            if (collider.CompareTag("Herd") && collider.gameObject != gameObject)
            {
                float distanceToSheep = Vector3.Distance(transform.position, collider.transform.position);
                if (distanceToSheep <= sheepPanicDetectionRadius)
                {
                    SheepLogic nearbySheep = collider.GetComponent<SheepLogic>();
                    if (nearbySheep != null && nearbySheep.PanicValue > highPanicThreshold)
                    {
                        highPanicSheepDetected = true;
                        panicValue = Mathf.Max(panicValue, nearbySheep.PanicValue * 0.9f);
                        break;
                    }
                }
            }
        }

        if (!dogDetected && !highPanicSheepDetected)
        {
            timeSinceLastPanic += Time.deltaTime;
            if (timeSinceLastPanic >= panicCooldownTime)
            {
                isPanicked = false;
            }
        }
        else
        {
            isPanicked = true;
            timeSinceLastPanic = 0f;
        }
    }

    public void Move(Vector3 desiredVelocity)
    {
        desiredVelocity.y = 0;
        
        // Scale movement by panic value (minimum 0.5 to maintain some movement even when calm)
        float panicMultiplier = Mathf.Lerp(0.5f, 1f, panicValue);
        desiredVelocity *= panicMultiplier;
        
        // Calculate the force needed to achieve the desired velocity
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 force = (desiredVelocity - currentVelocity);
        
        // Limit the maximum force
        if (force.magnitude > maxForce)
        {
            force = force.normalized * maxForce;
        }
        
        // Apply the force
        rb.AddForce(force, ForceMode.Acceleration);
        
        // Rotate to face movement direction if we're moving
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.linearVelocity);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }
    }

    public Vector3 CalculateMovement(List<Transform> context, SheepManager manager, DogController dog)
    {
        if (isPanicked)
        {
            return panickedBehaviour.calculateMove(this, context, manager, dog);
        }
        return relaxedBehaviour.calculateMove(this, context, manager, dog);
    }
}